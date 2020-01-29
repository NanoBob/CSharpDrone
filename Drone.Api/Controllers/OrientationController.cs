using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Drone.Core.Interfaces;
using Drone.Core.Structs;
using Newtonsoft.Json;

namespace Drone.Core.Controllers
{
    public class OrientationController : IController
    {
        public void AddRoutes(WebServer webserver, Drone drone)
        {
            webserver.AddAction("GET", "/orientation", (context) =>
            {
                Orientation orientation = drone.Orientation;
                return new
                {
                    yaw = orientation.Yaw,
                    pitch = orientation.Pitch,
                    roll = orientation.Roll,
                };
            });

            webserver.AddAction("POST", "/orientation", (context) =>
            {
                drone.StartOrientationThread();
                return new
                {
                    message = "Orientation sensor enabled"
                };
            });

            webserver.AddAction("DELETE", "/orientation", (context) =>
            {
                drone.StartOrientationThread();
                return new
                {
                    message = "Orientation sensor disabled"
                };
            });

            webserver.AddAction("POST", "/orientationAssist", (context) =>
            {
                drone.StartOrientationAssist();
                return new
                {
                    message = "Orientation assist enabled"
                };
            });

            webserver.AddAction("DELETE", "/orientationAssist", (context) =>
            {
                drone.StopOrientationAssist();
                return new
                {
                    message = "Orientation assist disabled"
                };
            });

            webserver.AddAction("POST", "/selfLearning", (context) =>
            {
                drone.IsSelfLearning = true;
                return new
                {
                    message = "Self learning enabled"
                };
            });

            webserver.AddAction("DELETE", "/selfLearning", (context) =>
            {
                drone.IsSelfLearning = false;
                return new
                {
                    message = "Self learning disabled"
                };
            });

            webserver.AddAction("POST", "/orientationAssistAgression", (context) =>
            {

                string input;
                using (StreamReader reader = new StreamReader(context.Request.InputStream))
                {
                    input = reader.ReadToEnd();
                }

                float value = JsonConvert.DeserializeObject<RunTestStruct>(input).Value;

                drone.OrientationAssistAgression = value;
                return new
                {
                    message = "Orientation assist aggression set"
                };
            });
        }
    };
}