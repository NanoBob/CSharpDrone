using Drone.Core.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Controllers
{
    public class MotorController : IController
    {
        public string BaseRoute => "/motors";

        public MotorController()
        {
        }

        public void AddRoutes(WebServer webserver, Drone drone)
        {
            webserver.AddAction("GET", BaseRoute, (context) =>
            {
                var throttles = drone.GetMotorThrottles();
                return new
                {
                    frontLeft = throttles.Item1,
                    frontRight = throttles.Item2,
                    rearLeft = throttles.Item3,
                    rearRight = throttles.Item4
                };
            });

            webserver.AddAction("POST", BaseRoute, (context) =>
            {
                drone.EnableMotors();

                return new
                {
                    message = "Motors enabled"
                };
            });

            webserver.AddAction("DELETE", BaseRoute, (context) =>
            {
                drone.DisableMotors();

                return new
                {
                    message = "Motors disabled"
                };
            });

            webserver.AddAction("POST", BaseRoute + "/test", (context) =>
            {
                string input;
                using (StreamReader reader = new StreamReader(context.Request.InputStream))
                {
                    input = reader.ReadToEnd();
                }

                float value = JsonConvert.DeserializeObject<RunTestStruct>(input).Value;
                _ = Task.Run(() =>
                {
                    drone.RunTest(value);
                });

                return new
                {
                    message = "Motor test started"
                };
            });

            webserver.AddAction("POST", BaseRoute + "/setSpeed", (context) =>
            {
                string input;
                using (StreamReader reader = new StreamReader(context.Request.InputStream))
                {
                    input = reader.ReadToEnd();
                }

                float value = JsonConvert.DeserializeObject<RunTestStruct>(input).Value;
                _ = Task.Run(() =>
                {
                    drone.MotorThrottle = value;
                });

                return new
                {
                    message = "Motor throttle set"
                };
            });
        }
    }

    struct RunTestStruct
    {
        public float Value { get; set; }
    }
}
