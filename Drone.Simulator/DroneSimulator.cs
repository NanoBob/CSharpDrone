using Drone.Core.Controllers;
using Drone.Core.Enums;
using Drone.Core.Interfaces;
using Drone.Core.Sensors.Orientation;
using Drone.Simulator.Devices;
using System;
using System.Collections.Generic;
using System.IO;
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
            this.orientationController = new OrientationController(this.motorController, this.orientationSensor, new Dictionary<Axis, IOrientationOffsetHandler>()
            {
                [Axis.Yaw] = new QLearningOrientationOffsetHandler(),
                [Axis.Pitch] = new QLearningOrientationOffsetHandler(),
                [Axis.Roll] = new QLearningOrientationOffsetHandler(),
            });
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
                //Console.WriteLine(this.orientationSensor.Orientation);
            }

            foreach(var offsetHandler in this.orientationController.orientationOffsetHandlers)
            {
                if (offsetHandler.Value is QLearningOrientationOffsetHandler qLearningOrientationOffsetHandler)
                {
                    File.WriteAllText($"{offsetHandler.Key.ToString()}.json", qLearningOrientationOffsetHandler.SaveToJson());
                }
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
            this.orientationSensor.Orientation = SanitizeOriention(newOrientation);
        }

        private Vector3 SanitizeOriention(Vector3 orientation)
        {
            return new Vector3(
                orientation.X > 180 ? orientation.X - 360 : orientation.X < -180 ? orientation.X + 360 : orientation.X,
                orientation.Y > 180 ? orientation.Y - 360 : orientation.Y < -180 ? orientation.Y + 360 : orientation.Y,
                orientation.Z > 180 ? orientation.Z - 360 : orientation.Z < -180 ? orientation.Z + 360 : orientation.Z
            );
        }
    }
}
