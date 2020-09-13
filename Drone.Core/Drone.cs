using Drone.Core.Controllers;
using Drone.Core.Devices.Pwm;
using Drone.Core.Enums;
using Drone.Core.Interfaces;
using Drone.Core.Structs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core
{
    public class Drone
    {
        public Orientation Orientation => new Orientation(this.orientationController.Orientation);
        public Vector3 TargetOrientation
        {
            get => this.orientationController.Target;
            set => this.orientationController.Target = value;
        }

        public Position Position => this.gpsController.Position;
        public bool IsOrientationSensorEnabled => this.orientationController.IsSensorEnabled;
        public bool IsOrientationAssistEnabled => this.orientationController.IsAssistEnabled;
        public bool IsGpsEnabled => this.gpsController.IsEnabled;
        public bool AreMotorsEnabled => this.motorController.IsEnabled;

        public int OrientationAssistRate
        {
            get => this.orientationController.FramesPerAssist;
            set => this.orientationController.FramesPerAssist = value;
        }

        public float MotorThrottle
        {
            set => this.motorController.Throttle = value;
        }

        public float OrientationAssistAgression
        {
            set => this.orientationController.OverallAggression = value;
        }

        private readonly PwmController pwmController;
        private readonly MotorController motorController;
        private readonly OrientationController orientationController;
        private readonly GpsController gpsController;

        public Drone()
        {
            this.pwmController = new PwmController();

            this.motorController = new MotorController(pwmController);
            this.motorController.ThrottleChanged += (value) 
                => ThrottleChanged?.Invoke(value);

            this.orientationController = new OrientationController(this.motorController);
            this.orientationController.OrientationChanged += (value) 
                => OrientationChanged?.Invoke(value);

            this.gpsController = new GpsController();
            this.gpsController.PositionChanged += (value) 
                => PositionChanged?.Invoke(value);
        }

        public async Task Init()
        {
            await Task.WhenAll(new Task[]
            {
                this.motorController.Init(),
                this.orientationController.Init(),
                this.gpsController.Init()
            });

            this.orientationController.StartOrientationLoop();
        }

        public Tuple<float, float, float, float> GetMotorThrottles()
        {
            return new Tuple<float, float, float, float>(
                (float)this.motorController.FrontLeft.Speed,
                (float)this.motorController.FrontRight.Speed,
                (float)this.motorController.RearLeft.Speed,
                (float)this.motorController.RearRight.Speed
            );
        }

        public void StartOrientationThread() => this.orientationController.StartOrientationLoop();

        public void StopOrientationThread() => this.orientationController.StopOrientationLoop();

        public void StartOrientationAssist() => this.orientationController.IsAssistEnabled = true;

        public void StopOrientationAssist() => this.orientationController.IsAssistEnabled = false;

        public void StartGps() => this.gpsController.IsEnabled = true;

        public void StopGps() => this.gpsController.IsEnabled = false;

        public Task RunTest(float value) => this.motorController.RunTest(value);

        public void EnableMotors() => this.motorController.Enable();

        public void DisableMotors() => this.motorController.Disable();

        public void SetOffsetHandler(Axis axis, IOrientationOffsetHandler offsetHandler) 
            => this.orientationController.SetOffsetHandler(axis, offsetHandler);

        public IOrientationOffsetHandler? GetOffsetHandler(Axis axis)
            => this.orientationController.GetOffsetHandler(axis);

        public event Action<Tuple<float, float, float, float>>? ThrottleChanged;
        public event Action<Orientation>? OrientationChanged;
        public event Action<Position>? PositionChanged;
    }
}
