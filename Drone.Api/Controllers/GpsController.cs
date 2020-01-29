using Drone.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.Controllers
{
    public class GpsController : IController
    {
        public string BaseRoute => "/gps";

        public GpsController()
        {
        }

        public void AddRoutes(WebServer webserver, Drone drone)
        {
            webserver.AddAction("POST", BaseRoute, (context) =>
            {
                drone.StartGps();
                return "Gps started";
            });

            webserver.AddAction("DELETE", BaseRoute, (context) =>
            {
                drone.StopGps();
                return "Gps stopped";
            });

            webserver.AddAction("GET", $"{BaseRoute}/position", (context) =>
            {
                var position = drone.Position;
                return new
                {
                    longitude = position.longitude,
                    latitude = position.latitude
                };
            });
        }
    }
}
