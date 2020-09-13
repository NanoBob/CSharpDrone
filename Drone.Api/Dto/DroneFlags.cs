using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Api.Dto
{
    [Flags]
    public enum DroneFlags
    {
        MotorsEnabled = 0x01,
        GpsEnabled = 0x02,
        OrientationSensorEnabled = 0x04,
        OrientationAssistEnabled = 0x08,
    }
}
