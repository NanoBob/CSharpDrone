using Drone.Core.Devices.Motors;
using Drone.Core.Devices.Pwm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Controllers
{
    public class MotorController
    {
        private readonly IPwmController pwmController;

        public Motor FrontLeft { get; private set; }
        public Motor FrontRight { get; private set; }
        public Motor RearLeft { get; private set; }
        public Motor RearRight { get; private set; }

        public bool IsEnabled { get; private set; }

        private double yaw;
        public double Yaw
        {
            get => yaw;
            set => yaw = LimitThrust(value, 0.25);
        }

        private double pitch;
        public double Pitch
        {
            get => pitch;
            set => pitch = LimitThrust(value, 0.25);
        }

        private double roll;
        public double Roll
        {
            get => roll;
            set => roll = LimitThrust(value, 0.25);
        }

        private double throttle;
        public double Throttle
        {
            get => throttle;
            set
            { 
                throttle = LimitThrust(value, 0.7);
                UpdateMotors();
            }
        }

        public MotorController(IPwmController pwmController)
        {
            this.pwmController = pwmController;
        }

        public async Task Init()
        {
            this.pwmController.Init();
            await Task.Delay(1000);
            
            FrontLeft = new Motor(this.pwmController, 0);
            FrontRight = new Motor(this.pwmController, 4);
            RearRight = new Motor(this.pwmController, 8);
            RearLeft = new Motor(this.pwmController, 12);

            await Arm();

            this.IsEnabled = true;

        }

        public async Task Arm(double min = 0.1, double max = 1)
        {
            await Task.WhenAll(new Task[] 
            {
                FrontLeft.Arm(min, max),
                FrontRight.Arm(min, max),
                RearLeft.Arm(min, max),
                RearRight.Arm(min, max)
            });
        }

        public void Shutdown()
        {
            StopAllMotors();
        }

        public void Enable() => IsEnabled = true;

        public void Disable()
        {
            IsEnabled = false;
            FrontLeft.Run(0);
            FrontRight.Run(0);
            RearLeft.Run(0);
            RearRight.Run(0);
            IsEnabled = false;

            InvokeThrottleChanged();
        }

        private void StopAllMotors()
        {
            FrontLeft.Stop();
            FrontRight.Stop();
            RearLeft.Stop();
            RearRight.Stop();
        }

        private double LimitThrust(double value, double limit) => Math.Clamp(value, -limit, limit);

        public void UpdateMotors()
        {
            if (!IsEnabled)
            {
                return;
            }

            FrontLeft.Run(throttle + pitch + roll + yaw);
            FrontRight.Run(throttle + pitch - roll - yaw);
            RearLeft.Run(throttle - pitch + roll - yaw);
            RearRight.Run(throttle - pitch - roll + yaw);

            InvokeThrottleChanged();
        }

        private void InvokeThrottleChanged()
        {
            this.ThrottleChanged?.Invoke(new Tuple<float, float, float, float>(
                (float)FrontLeft.Speed,
                (float)FrontRight.Speed,
                (float)RearLeft.Speed,
                (float)RearRight.Speed
            ));
        }

        public async Task RunTest(float target)
        {
            Console.WriteLine(target);
            if (target < 1)
            {
                await PlaySong(song);
                return;
            }
            if (target > 100)
            {
                return;
            }
            for (float i = 0; i < target; i += (target * .0004f))
            {
                this.Throttle = i;
                await Task.Delay(10);
            }
            for (float i = target; i >= 0; i -= (target * .0004f))
            {
                this.Throttle = i;
                await Task.Delay(10);
            }
        }

        private readonly Note[] song = new Note[]
        {
            new Note(0.07f, 750),
            new Note(0.00f, 50),
            new Note(0.07f, 300),
            new Note(0.11f, 500),
            new Note(0.08f, 500),
            new Note(0.06f, 500),
            new Note(0.04f, 750),
            new Note(0.03f, 500),
        };

        
        private async Task PlaySong(Note[] notes)
        {
            for (int i = 0; i < 4; i++)
            {
                foreach (var note in notes)
                {
                    this.Throttle = note.power * 3;
                    await Task.Delay(note.duration);
                    Console.WriteLine(note.power);
                }
                await Task.Delay(200);
            }
            this.Throttle = 0;
        }

        public event Action<Tuple<float, float, float, float>>? ThrottleChanged;
    }

    struct Note
    {
        public float power;
        public int duration;

        public Note(float power, int duration)
        {
            this.power = power;
            this.duration = duration;
        }
    }
}
