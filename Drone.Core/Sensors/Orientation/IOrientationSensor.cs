using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Sensors.Orientation
{
    public interface IOrientationSensor
    {
        Vector3 GetOrientation();

        Task Init();
    }
}
