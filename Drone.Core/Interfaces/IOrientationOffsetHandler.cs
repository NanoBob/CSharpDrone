using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.Interfaces
{
    interface IOrientationOffsetHandler
    {
        float HandleOffset(float offset);
    }
}
