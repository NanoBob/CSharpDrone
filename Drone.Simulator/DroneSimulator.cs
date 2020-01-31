using Drone.Core.Controllers;
using Drone.Core.Sensors.Orientation;
using Drone.Simulator.Devices;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Drone.Simulator
{
    public class DroneSimulator
    {
        private const float angleOffsetMultiplier = 1.37f;

        private readonly SimulationPwmController pwmController;
        private readonly MotorController motorController;
        private readonly OrientationController orientationController;
        private readonly SimulationOrientationSensor orientationSensor;

        public DroneSimulator()
        {
            this.pwmController = new SimulationPwmController();
            this.orientationSensor = new SimulationOrientationSensor();
            this.motorController = new MotorController(this.pwmController);
            this.orientationController = new OrientationController(this.motorController, this.orientationSensor);
        }

        public async Task Init()
        {
            await this.orientationController.Init();
            await this.motorController.Init();
        }

        public Vector3 Start(Vector3 startOrientation, Vector3 targetOrientation, int iterations = 256)
        {
            this.orientationSensor.Orientation = startOrientation;
            this.orientationController.Target = targetOrientation;

            this.orientationController.Enabled = true;

            this.orientationController.OverallAggression = 0.13f;

            for (int i = 0; i < iterations; i++)
            {
                SimulateThrustApplication();
                this.orientationController.RunOrientationAssist();
                Console.WriteLine(this.orientationSensor.Orientation);
            }
            return this.orientationSensor.Orientation;
        }

        private void SimulateThrustApplication()
        {
            double yaw =
                + this.motorController.FrontLeft.Speed
                - this.motorController.FrontRight.Speed
                - this.motorController.RearLeft.Speed
                + this.motorController.RearRight.Speed;

            double pitch =
                + this.motorController.FrontLeft.Speed
                + this.motorController.FrontRight.Speed
                - this.motorController.RearLeft.Speed
                - this.motorController.RearRight.Speed;

            double roll =
                + this.motorController.FrontLeft.Speed
                - this.motorController.FrontRight.Speed
                + this.motorController.RearLeft.Speed
                - this.motorController.RearRight.Speed;

            Vector3 currentOrientation = this.orientationSensor.Orientation;
            Vector3 offset = new Vector3(- (float)yaw, - (float)pitch, - (float)roll);
            Vector3 newOrientation = currentOrientation + (angleOffsetMultiplier * offset);
            this.orientationSensor.Orientation = newOrientation;
        }
    }
}
