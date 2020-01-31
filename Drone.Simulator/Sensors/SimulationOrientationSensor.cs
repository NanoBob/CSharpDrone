using Drone.Core.Extensions;
using Drone.Core.Sensors.Orientation;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Drone.Simulator.Sensors.Orientation
{
    public class SimulationOrientationSensor : IOrientationSensor
	{
        public Vector3 Orientation { get; set; }

        public Task Init()
        {
            return Task.CompletedTask;
        }

		public Vector3 GetOrientation()
		{
            return this.Orientation;
		}
	}
}
