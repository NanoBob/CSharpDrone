using Drone.Api.Dto;
using Drone.Core.Structs;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drone.Api
{
    public enum WebSocketMessageType
    {
        Throttles,
        Orientation,
        Position,
        Flags,
        Authorized,
    }

    public static class WebSocketMessageFactory
    {
        public static byte[] CreateThrottlesMessage(Tuple<float, float, float, float> throttles)
        {
            return new byte[] { (byte)WebSocketMessageType.Throttles }
                .Concat(BitConverter.GetBytes(throttles.Item1))
                .Concat(BitConverter.GetBytes(throttles.Item2))
                .Concat(BitConverter.GetBytes(throttles.Item3))
                .Concat(BitConverter.GetBytes(throttles.Item4))
                .ToArray();
        }

        public static byte[] CreateOrientationMessage(Orientation orientation)
        {
            return new byte[] { (byte)WebSocketMessageType.Orientation }
                .Concat(BitConverter.GetBytes(orientation.Yaw))
                .Concat(BitConverter.GetBytes(orientation.Pitch))
                .Concat(BitConverter.GetBytes(orientation.Roll))
                .ToArray();
        }

        public static byte[] CreatePositionMessage(Position position)
        {
            return new byte[] { (byte)WebSocketMessageType.Position }
                .Concat(BitConverter.GetBytes(position.longitude))
                .Concat(BitConverter.GetBytes(position.latitude))
                .ToArray();
        }

        public static byte[] CreateFlagsMessage(DroneFlags value)
        {
            return new byte[] { (byte)WebSocketMessageType.Flags }
                .Concat(BitConverter.GetBytes((ushort)value))
                .ToArray();
        }

        public static byte[] CreateAuthorizedMessage()
        {
            return new byte[] { (byte)WebSocketMessageType.Authorized };
        }
    }
}
