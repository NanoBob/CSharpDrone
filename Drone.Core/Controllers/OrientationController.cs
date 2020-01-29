using Drone.Core.Enums;
using Drone.Core.Sensors.Orientation;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Controllers
{
    public class OrientationController
    {
        private OrientationSensor orientationSensor;
        private bool running;
        private readonly MotorController motorController;

        public Dictionary<Axis, OrientationOffsetHandler> orientationOffsetHandlers;

        public Vector3 Orientation { get; private set; }
        public Vector3 Target { get; set; } = Vector3.Zero;

        public bool Enabled { get; set; }

        public float OverallAggression
        {
            set
            {
                foreach(var kvPair in this.orientationOffsetHandlers)
                {
                    if (kvPair.Value is OrientationOffsetHandler orientationOffsetHandler)
                    {
                        orientationOffsetHandler.Aggression = value;
                    }
                }
            }
        }

        public OrientationController(MotorController motorController)
        {
            this.orientationSensor = new OrientationSensor();
            this.motorController = motorController;

            this.orientationOffsetHandlers = new Dictionary<Axis, OrientationOffsetHandler>()
            {
                //[Axis.Yaw] = new OrientationOffsetHandler(1.0f / 90.0f, 0.1f),
                [Axis.Pitch] = new OrientationOffsetHandler(1.0f / 45.0f, 0.3f),
                [Axis.Roll] = new OrientationOffsetHandler(1.0f / 45.0f, 0.3f),
            };
        }

        public async Task Init()
        {
            await this.orientationSensor.Init();
        }

        public void StartOrientationLoop()
        {
            if (this.running)
            {
                return;
            }
            this.running = true;
            _ = Task.Run(OrientationLoop);
        }

        public void StopOrientationLoop()
        {
            this.running = false;
        }

        private async void OrientationLoop()
        {
            while (true)
            {
                this.Orientation = this.orientationSensor.GetOrientation();
                if (Enabled)
                {
                    HandleOrientationOffset(this.Orientation - this.Target);
                }
                await Task.Delay(10);
            }
        }

        private void HandleOrientationOffset(Vector3 orientation)
        {
            this.motorController.Yaw = GetThrottleForOffset(Axis.Yaw, orientation.X);
            this.motorController.Pitch = GetThrottleForOffset(Axis.Pitch, orientation.Y);
            this.motorController.Roll = GetThrottleForOffset(Axis.Roll, orientation.Z);
            this.motorController.UpdateMotors();
        }

        private float GetThrottleForOffset(Axis axis, float offset)
        {
            if (this.orientationOffsetHandlers.TryGetValue(axis, out OrientationOffsetHandler orientationOffsetHandler))
            {
                return orientationOffsetHandler.HandleOffset(offset);
            }
            return 0;
        }
    }
}
