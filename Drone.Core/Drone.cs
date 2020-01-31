using Drone.Core.Controllers;
using Drone.Core.Devices.Pwm;
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

        public Tuple<float, float, float, float> GetMotorThrottles()
        {
            return new Tuple<float, float, float, float>(
                (float)this.motorController.FrontLeft.Speed,
                (float)this.motorController.FrontRight.Speed,
                (float)this.motorController.RearLeft.Speed,
                (float)this.motorController.RearRight.Speed
            );
        }

        public float MotorThrottle
        {
            set => this.motorController.Throttle = value;
        }

        public float OrientationAssistAgression
        {
            set => this.orientationController.OverallAggression = value;
        }

        public bool IsSelfLearning
        {
            set => throw new NotImplementedException();
        }

        private readonly PwmController pwmController;
        private readonly MotorController motorController;
        private readonly OrientationController orientationController;
        private readonly GpsController gpsController;

        public Drone()
        {
            this.pwmController = new PwmController();
            this.motorController = new MotorController(pwmController);
            this.orientationController = new OrientationController(this.motorController);
            this.gpsController = new GpsController();
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

        public void StartOrientationThread() => this.orientationController.StartOrientationLoop();

        public void StopOrientationThread() => this.orientationController.StopOrientationLoop();

        public void StartOrientationAssist() => this.orientationController.Enabled = true;

        public void StopOrientationAssist() => this.orientationController.Enabled = false;

        public void StartGps() => this.gpsController.Enabled = true;

        public void StopGps() => this.gpsController.Enabled = false;

        public Task RunTest(float value) => this.motorController.RunTest(value);

        public void EnableMotors() => this.motorController.Enable();

        public void DisableMotors() => this.motorController.Disable();
    }
}
