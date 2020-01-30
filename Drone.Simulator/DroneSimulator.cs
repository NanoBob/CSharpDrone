using Drone.Core.Controllers;
using Drone.Core.Sensors.Orientation;
using System;

namespace Drone.Simulator
{
    public class DroneSimulator
    {
        private MotorController motorController;
        private OrientationController orientationController;
        private SimulationOrientationSensor orientationSensor;

        public DroneSimulator()
        {
            this.orientationSensor = new SimulationOrientationSensor();
            this.motorController = new MotorController();
            this.orientationController = new OrientationController(this.motorController, this.orientationSensor);
        }


    }
}
