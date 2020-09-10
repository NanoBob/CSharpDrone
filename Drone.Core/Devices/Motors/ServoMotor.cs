using Drone.Core.Devices.Pwm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.Devices.Motors
{
    public class ServoMotor
    {
        private const int servoMax = 4096;
        private const int servoMin = 0;

        private readonly IPwmController pwmController;
        private readonly byte id;

        public ServoMotor(IPwmController pwmController, byte id)
        {
            this.pwmController = pwmController;
            this.id = id;
            Console.WriteLine($"Min: {servoMin}");
            Console.WriteLine($"Max: {servoMax}");
        }

        public void SetDesiredAngle(float factor)
        {
            this.pwmController.SetPulseParameters(this.id, servoMin + factor * (servoMax - servoMin));
            Console.WriteLine($"{id}: {factor}: {servoMin + factor * (servoMax - servoMin)}");
        }
    }
}
