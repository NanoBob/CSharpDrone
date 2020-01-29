using Drone.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Drone.Core.AuthorizationHandlers
{
    public class TimeBasedHmacAuthorizationHandler: IAuthorizationHandler
    {
        private readonly string secretKey;
        private readonly TimeSpan windowSize;
        private readonly int windowFlexibilty;

        public TimeBasedHmacAuthorizationHandler(string secretKey, TimeSpan windowSize, int windowFlexibilty)
        {
            this.secretKey = secretKey;
            this.windowSize = windowSize;
            this.windowFlexibilty = windowFlexibilty;
        }

        public bool IsAuthorized(HttpListenerContext context)
        {
            string bearer = context.Request.Headers["Authorization"];
            if (bearer == null)
            {
                return false;
            }
            string token = bearer.Replace("Bearer ", "");


            DateTime roundedNow = RoundDateTime(DateTime.UtcNow, this.windowSize);

            if (IsValidToken(token, roundedNow))
            {
                return true;
            }

            for (int i = 1; i <= windowFlexibilty; i++)
            {
                DateTime previous = roundedNow - (this.windowSize * i);
                if (IsValidToken(token, previous))
                {
                    return true;
                }

                DateTime next = roundedNow + (this.windowSize * i);
                if (IsValidToken(token, next))
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsValidToken(string hash, DateTime dateTime)
        {
            return GenerateHash(dateTime) == hash;
        }

        private DateTime RoundDateTime(DateTime date, TimeSpan timeSpan)
        {
            long ticks = date.Ticks / timeSpan.Ticks;

            return new DateTime(ticks * timeSpan.Ticks);
        }

        private string GenerateHash(DateTime timestamp)
        {
            int unixTime = (int)(timestamp.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string input = unixTime.ToString();
            byte[] binaryInput = Encoding.ASCII.GetBytes(input);

            byte[] key = Encoding.ASCII.GetBytes(this.secretKey);
            HMACSHA256 hmac = new HMACSHA256(key);

            byte[] hash = hmac.ComputeHash(binaryInput);
            return Convert.ToBase64String(hash);
        }
    }
}
