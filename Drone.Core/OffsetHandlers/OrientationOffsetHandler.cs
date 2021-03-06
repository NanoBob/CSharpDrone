﻿using Drone.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.OffsetHandlers
{
    public class OrientationOffsetHandler : IOrientationOffsetHandler
    {
        public float Aggression { get; set; }
        public float MaxThrottle { get; }

        public OrientationOffsetHandler(float aggression, float maxThrust)
        {
            this.Aggression = aggression;
            this.MaxThrottle = maxThrust;
        }

        public float HandleOffset(float offset)
        {
            float targetPower = MaxThrottle * Math.Clamp(this.Aggression * (offset / 180.0f), -1.0f, 1.0f);
            return targetPower;
        }
    }
}
