using System;
using System.Collections.Generic;
using System.Text;

namespace Drone.Core.Devices.Pwm
{
    public interface IPwmController
    {
        void Init();
        void SetPulseParameters(byte pin, double dutyCycle, bool invertPolarity = false);
        void SetDesiredFrequency(double frequency);
    }
}
