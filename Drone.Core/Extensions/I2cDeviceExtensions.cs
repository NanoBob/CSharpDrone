using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Text;

namespace Drone.Core.Extensions
{
    public static class I2cDeviceExtensions
    {
		public static void WriteToAddress(this I2cDevice i2cDevice, byte address, byte data)
		{
			i2cDevice.Write(new byte[] {
				address,
				data
			});
		}

		public static byte[] ReadFromAddress(this I2cDevice i2cDevice, byte address, int length)
		{
			i2cDevice.Write(new byte[] {
				address
			});
			byte[] buffer = new byte[length];
			i2cDevice.Read(buffer);
			return buffer;
		}

		public static byte ReadFromAddress(this I2cDevice i2cDevice, byte address)
		{
			i2cDevice.Write(new byte[] {
				address
			});
			byte[] buffer = new byte[1];
			i2cDevice.Read(buffer);
			return buffer[0];
		}
	}
}
