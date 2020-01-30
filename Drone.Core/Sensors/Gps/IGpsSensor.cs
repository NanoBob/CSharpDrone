using Drone.Core.Structs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Sensors.Gps
{
    public interface IGpsSensor
    {
        Task Init();

        event Action<Position> OnPositionDetected;
    }
}
