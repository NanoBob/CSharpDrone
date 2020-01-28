using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;

namespace Drone.Core.Devices.Pwm
{
    public class PwmController
    {
        private readonly I2cDevice i2cDevice;

        public PwmController(int busId = 1, int address = 0x28)
        {
            this.i2cDevice = I2cDevice.Create(new I2cConnectionSettings(busId, address));
        }

        public void Init()
        {

        }

        public void Reset()
        {

        }
    }
}
