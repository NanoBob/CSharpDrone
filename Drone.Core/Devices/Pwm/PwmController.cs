using Drone.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;

namespace Drone.Core.Devices.Pwm
{
    public class PwmController: IPwmController
    {
        private readonly I2cDevice i2cDevice;

        public PwmController(int busId = 1, int address = 0x40)
        {
            this.i2cDevice = I2cDevice.Create(new I2cConnectionSettings(busId, address));
        }

        public void Init()
        {
            Reset();
        }

        public void Reset()
        {
            this.i2cDevice.WriteToAddress((byte)PwmRegister.MODE1, 0x0);
        }

        public void SetPwm(byte channel, ushort on, ushort off)
        {
            this.i2cDevice.WriteToAddress((byte)(((int)PwmRegister.LED0_ON_L) + 4 * channel), (byte)(on & 0xff));
            this.i2cDevice.WriteToAddress((byte)(((int)PwmRegister.LED0_ON_H) + 4 * channel), (byte)(on >> 8));
            this.i2cDevice.WriteToAddress((byte)(((int)PwmRegister.LED0_OFF_L) + 4 * channel), (byte)(off & 0xff));
            this.i2cDevice.WriteToAddress((byte)(((int)PwmRegister.LED0_OFF_H) + 4 * channel), (byte)(off >> 8));
        }

        public void SetAllPwm(ushort on, ushort off)
        {
            this.i2cDevice.WriteToAddress((byte)PwmRegister.ALL_LED_ON_L, (byte)(on & 0xff));
            this.i2cDevice.WriteToAddress((byte)PwmRegister.ALL_LED_ON_H, (byte)(on >> 8));
            this.i2cDevice.WriteToAddress((byte)PwmRegister.ALL_LED_OFF_L, (byte)(off & 0xff));
            this.i2cDevice.WriteToAddress((byte)PwmRegister.ALL_LED_OFF_H, (byte)(off >> 8));
        }

        public void SetPulseParameters(byte pin, double dutyCycle, bool invertPolarity = false)
        {
            ushort value = (ushort)Math.Min((float)dutyCycle, 4095.0f);

            if (pin > 15)
            {
                throw new ArgumentException("Pin number must be between 0 and 15");
            }

            if (invertPolarity)
            {
                switch (value)
                {
                    case 0:
                        SetPwm(pin, 4095, 0);
                        break;
                    case 4095:
                        SetPwm(pin, 0, 4095);
                        break;
                    case 4096:
                        SetPwm(pin, 0, 4096);
                        break;
                    default:
                        SetPwm(pin, 0, (ushort)(4095 - value));
                        break;
                }
            }
            else
            {
                switch (value)
                {
                    case 4095:
                        SetPwm(pin, 4095, 0);
                        break;
                    case 4096:
                        SetPwm(pin, 4096, 0);
                        break;
                    case 0:
                        SetPwm(pin, 0, 4095);
                        break;
                    default:
                        SetPwm(pin, 0, value);
                        break;
                }
            }
        }

        public void SetDesiredFrequency(double frequency)
        {
            frequency *= 0.9f;  // Correct for overshoot in the frequency setting.
            double prescaleval = 25000000;
            prescaleval /= 4096;
            prescaleval /= frequency;
            prescaleval -= 1;

            byte prescale = (byte)Math.Floor(prescaleval + 0.5f);

            int oldMode = i2cDevice.ReadFromAddress((byte)PwmRegister.MODE1);
            int newMode = (oldMode & 0x7f) | 0x10;

            this.i2cDevice.WriteToAddress((byte)PwmRegister.MODE1, (byte)newMode);
            this.i2cDevice.WriteToAddress((byte)PwmRegister.PRESCALE, (byte)prescale);
            this.i2cDevice.WriteToAddress((byte)PwmRegister.MODE1, (byte)oldMode);

            this.i2cDevice.WriteToAddress((byte)PwmRegister.MODE1, (byte)(oldMode | 0xa1));
        }
    }
}
