using Drone.Core.Enums;
using Drone.Core.Interfaces;
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
        private IOrientationSensor orientationSensor;
        private bool running;
        private readonly MotorController motorController;

        public Dictionary<Axis, IOrientationOffsetHandler> orientationOffsetHandlers;

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

        public OrientationController(MotorController motorController, IOrientationSensor orientationSensor, Dictionary<Axis, IOrientationOffsetHandler> offsetHandlers)
        {
            this.orientationSensor = orientationSensor;
            this.motorController = motorController;

            this.orientationOffsetHandlers = offsetHandlers;
        }

        public OrientationController(MotorController motorController, IOrientationSensor orientationSensor): 
            this(
                motorController, 
                orientationSensor, 
                new Dictionary<Axis, IOrientationOffsetHandler>()
                {
                    [Axis.Yaw] = new OrientationOffsetHandler(0.0f, 0.1f),
                    [Axis.Pitch] = new OrientationOffsetHandler(1.0f / 45.0f, 0.3f),
                    [Axis.Roll] = new OrientationOffsetHandler(1.0f / 45.0f, 0.3f),
                })
        {

        }

        public OrientationController(MotorController motorController): this(motorController, new OrientationSensor())
        {

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
                RunOrientationAssist();
                await Task.Delay(10);
            }
        }

        public void RunOrientationAssist()
        {
            this.Orientation = this.orientationSensor.GetOrientation();
            if (Enabled)
            {
                Console.WriteLine($"Orient: {Orientation}");
                Console.WriteLine($"Target: {Target}");
                HandleOrientationOffset(this.Orientation - this.Target);
            }
        }

        private void HandleOrientationOffset(Vector3 offset)
        {
            Console.WriteLine($"Offset: {offset}");
            this.motorController.Yaw = GetThrottleForOffset(Axis.Yaw, offset.X);
            this.motorController.Pitch = GetThrottleForOffset(Axis.Pitch, offset.Y);
            this.motorController.Roll = GetThrottleForOffset(Axis.Roll, offset.Z);
            this.motorController.UpdateMotors();
        }

        private float GetThrottleForOffset(Axis axis, float offset)
        {
            if (this.orientationOffsetHandlers.TryGetValue(axis, out IOrientationOffsetHandler orientationOffsetHandler))
            {
                return orientationOffsetHandler.HandleOffset(offset);
            }
            return 0;
        }
    }
}
