using Drone.Core.Enums;
using Drone.Core.Interfaces;
using Drone.Core.OffsetHandlers;
using Drone.Core.Sensors.Orientation;
using Drone.Core.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Controllers
{
    public class OrientationController
    {
        private readonly IOrientationSensor orientationSensor;
        private readonly MotorController motorController;
        private readonly Dictionary<Axis, IOrientationOffsetHandler> orientationOffsetHandlers;


        public Vector3 Orientation { get; private set; }
        public Vector3 Target { get; set; } = Vector3.Zero;
        public bool IsSensorEnabled { get; private set; }
        public bool IsAssistEnabled { get; set; }
        public int FramesPerAssist { get; set; }

        private int assistCount;

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

            this.assistCount = 0;
            this.FramesPerAssist = 1;
        }

        public OrientationController(MotorController motorController, IOrientationSensor orientationSensor): 
            this(
                motorController, 
                orientationSensor, 
                new Dictionary<Axis, IOrientationOffsetHandler>()
                {
                    [Axis.Yaw] = new OrientationOffsetHandler(0.0f, 0.1f),
                    [Axis.Pitch] = new OrientationOffsetHandler(5.0f, 0.2f),
                    [Axis.Roll] = new OrientationOffsetHandler(5.0f, 0.2f),
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
            if (this.IsSensorEnabled)
            {
                return;
            }
            this.IsSensorEnabled = true;
            _ = Task.Run(OrientationLoop);
        }

        public void StopOrientationLoop()
        {
            this.IsSensorEnabled = false;
        }

        private async void OrientationLoop()
        {
            while (this.IsSensorEnabled)
            {
                RunOrientationAssist();
                await Task.Delay(10);
            }
        }

        public void RunOrientationAssist()
        {
            this.Orientation = SanitizeOriention(this.orientationSensor.GetOrientation());
            OrientationChanged?.Invoke(new Orientation(this.Orientation));
            if (IsAssistEnabled && (++assistCount % FramesPerAssist == 0))
            {
                Debug.WriteLine($"Orient: {Orientation}");
                Debug.WriteLine($"Target: {Target}");
                HandleOrientationOffset(this.Orientation - this.Target);
            }
        }

        public IOrientationOffsetHandler? GetOffsetHandler(Axis axis)
        {
            this.orientationOffsetHandlers.TryGetValue(axis, out var value);
            return value;
        }

        public void SetOffsetHandler(Axis axis, IOrientationOffsetHandler handler)
        {
            this.orientationOffsetHandlers[axis] = handler;
        }

        private void HandleOrientationOffset(Vector3 offset)
        {
            Debug.WriteLine($"Offset: {offset}");
            this.motorController.Yaw = GetThrottleForOffset(Axis.Yaw, offset.X);
            this.motorController.Pitch = GetThrottleForOffset(Axis.Pitch, offset.Y);
            this.motorController.Roll = GetThrottleForOffset(Axis.Roll, offset.Z);
            this.motorController.UpdateMotors();
        }

        private float GetThrottleForOffset(Axis axis, float offset)
        {
            if (this.orientationOffsetHandlers.TryGetValue(axis, out IOrientationOffsetHandler? orientationOffsetHandler))
            {
                return orientationOffsetHandler.HandleOffset(offset);
            }
            return 0;
        }

        private Vector3 SanitizeOriention(Vector3 orientation)
        {
            return new Vector3(
                orientation.X > 180 ? orientation.X - 360 : orientation.X < -180 ? orientation.X + 360 : orientation.X,
                orientation.Y > 180 ? orientation.Y - 360 : orientation.Y < -180 ? orientation.Y + 360 : orientation.Y,
                orientation.Z > 180 ? orientation.Z - 360 : orientation.Z < -180 ? orientation.Z + 360 : orientation.Z
            );
        }

        public event Action<Orientation>? OrientationChanged;
    }
}
