using Drone.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.Controllers
{
    public class OrientationOffsetHandler : IOrientationOffsetHandler
    {
        public float Aggression { get; set; }
        private readonly float maxThrust;

        public OrientationOffsetHandler(float aggression, float maxThrust)
        {
            this.Aggression = aggression;
            this.maxThrust = maxThrust;
        }

        public float HandleOffset(float offset)
        {
            float targetPower = maxThrust * Math.Min(this.Aggression * offset, 1.0f);
            return targetPower;
        }
    }
}
