using Drone.Core.Devices.Pwm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Devices.Motors
{
    public class Motor
    {
        private const int servoMax = (int)(4000 * 0.4);
        private const int servoMin = (int)(4000 * 0.4 * 0.35);

        private readonly PwmController pwmController;
        private readonly byte id;

        private double throttleMin;
        private double throttleMax;

        public double Speed { get; private set; }
        public bool IsRunning { get; private set; }

        public Motor(PwmController pwmController, byte id)
        {
            this.pwmController = pwmController;
            this.id = id;
            Console.WriteLine($"Min: {servoMin}");
            Console.WriteLine($"Max: {servoMax}");
        }

        public async Task Arm(double min, double max)
        {
            throttleMin = servoMin;
            throttleMax = servoMax;

            this.Stop();
            await Task.Delay(4000);

            this.Run(max);
            await Task.Delay(1000);

            this.Run(min);
            await Task.Delay(1000);

            throttleMin = servoMin + min * (servoMax - servoMin);
            throttleMax = servoMin + max * (servoMax - servoMin);
        }

        public void Run(double speed)
        {
            this.Speed = speed;
            this.IsRunning = speed > 0;
            if (speed <= 0)
            {
                this.pwmController.SetPulseParameters(this.id, throttleMin);
            } else
            {
                this.pwmController.SetPulseParameters(this.id, throttleMin + speed * (throttleMax - throttleMin));
            }
        }

        public void Stop()
        {
            this.pwmController.SetPulseParameters(this.id, 0);
        }
    }
}
