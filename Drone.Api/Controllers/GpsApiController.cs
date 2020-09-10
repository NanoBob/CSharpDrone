using Drone.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.Controllers
{
    public class GpsApiController : IController
    {
        public string BaseRoute => "/gps";

        public GpsApiController()
        {
        }

        public void AddRoutes(WebServer webserver, Drone drone)
        {
            webserver.AddAction("GET", BaseRoute, (context) =>
            {
                return new
                {
                    value = drone.IsGpsEnabled
                };
            });

            webserver.AddAction("POST", BaseRoute, (context) =>
            {
                drone.StartGps();
                return new
                {
                    message = "Gps enabled"
                };
            });

            webserver.AddAction("DELETE", BaseRoute, (context) =>
            {
                drone.StopGps();
                return new
                {
                    message = "Gps disabled"
                };
            });

            webserver.AddAction("GET", $"{BaseRoute}/position", (context) =>
            {
                return drone.Position;
            });
        }
    }
}
