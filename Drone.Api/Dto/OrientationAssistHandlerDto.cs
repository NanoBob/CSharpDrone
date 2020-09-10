using Drone.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Api.Dto
{
    public struct OrientationAssistHandlerDto
    {
        public Axis axis;

        public float? agression;

        public bool isQLearning;
        public float? minThrottle;
        public float maxThrottle;
        public float? throttleIncrement;
    }
}
