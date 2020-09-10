using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Drone.Api.Dto;
using Drone.Core.Enums;
using Drone.Core.Interfaces;
using Drone.Core.OffsetHandlers;
using Drone.Core.Structs;
using Newtonsoft.Json;

namespace Drone.Core.Controllers
{
    public class OrientationApiController : IController
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

            webserver.AddAction("GET", "/orientation/sensor", (context) =>
            {
                return new
                {
                    value = drone.IsOrientationSensorEnabled
                };
            });

            webserver.AddAction("POST", "/orientation/sensor", (context) =>
            {
                drone.StartOrientationThread();
                return new
                {
                    message = "Orientation sensor enabled"
                };
            });

            webserver.AddAction("DELETE", "/orientation/sensor", (context) =>
            {
                drone.StopOrientationThread();
                return new
                {
                    message = "Orientation sensor disabled"
                };
            });

            webserver.AddAction("GET", "/orientation/assist", (context) =>
            {
                return new
                {
                    value = drone.IsOrientationAssistEnabled
                };
            });

            webserver.AddAction("POST", "/orientation/assist", (context) =>
            {
                drone.StartOrientationAssist();
                return new
                {
                    message = "Orientation assist enabled"
                };
            });

            webserver.AddAction("DELETE", "/orientation/assist", (context) =>
            {
                drone.StopOrientationAssist();
                return new
                {
                    message = "Orientation assist disabled"
                };
            });

            foreach (Axis axis in Enum.GetValues(typeof(Axis)))
            {
                webserver.AddAction("POST", $"/orientation/assist/handler/{axis}", (context) =>
                {
                    string input;
                    using (StreamReader reader = new StreamReader(context.Request.InputStream))
                    {
                        input = reader.ReadToEnd();
                    }

                    OrientationAssistHandlerDto dto = JsonConvert.DeserializeObject<OrientationAssistHandlerDto>(input);

                    var handler = dto.isQLearning ?
                        new QLearningOrientationOffsetHandler(-180, 180, 1, dto.minThrottle.Value, dto.maxThrottle, dto.throttleIncrement.Value) :
                        (IOrientationOffsetHandler)new OrientationOffsetHandler(dto.agression.Value, dto.maxThrottle);
                    drone.SetOffsetHandler(dto.axis, handler);

                    return new
                    {
                        message = "Orientation assist handler updated"
                    };
                });
            }

            foreach (Axis axis in Enum.GetValues(typeof(Axis))) {
                webserver.AddAction("GET", $"/orientation/assist/handler/{axis}", (context) =>
                {
                    var handler = drone.GetOffsetHandler(axis);

                    var qLearningHandler = handler as QLearningOrientationOffsetHandler;
                    var regularHandler = handler as OrientationOffsetHandler;

                    return new OrientationAssistHandlerDto
                    {
                        axis = axis,
                        agression = regularHandler?.Aggression,

                        isQLearning = qLearningHandler != null,
                        minThrottle = qLearningHandler?.MinThrottle,
                        maxThrottle = qLearningHandler?.MaxThrottle ?? regularHandler.MaxThrottle,
                        throttleIncrement = qLearningHandler?.ThrottleIncrement
                    };
                });
            }
        }
    };
}