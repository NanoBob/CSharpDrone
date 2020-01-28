using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Drone.Core.Sensors.Orientation
{
    public class OrientationSensor
	{
		private readonly I2cDevice i2cDevice;

		private const int BNO055_ADDRESS_A = 0x28;
		private const int BNO055_ADDRESS_B = 0x29;
		private const int BNO055_ID = 0xA0;

		public OrientationSensor(int busId = 1, int address = 0x28)
        {
            this.i2cDevice = I2cDevice.Create(new I2cConnectionSettings(busId, address));
        }

        public async Task<bool> Init()
		{
			await Task.Delay(1000);
			this.WriteToAddress((byte)OrientationSensorRegister.BNO055_OPR_MODE_ADDR, (int)OrientationSensorOperationMode.OPERATION_MODE_CONFIG);
			int value = this.ReadFromAddress((byte)OrientationSensorRegister.BNO055_OPR_MODE_ADDR);
			Console.WriteLine("Operation mode set to ");
			Console.WriteLine(value);
			await Task.Delay(1000);

			this.WriteToAddress((byte)OrientationSensorRegister.BNO055_SYS_TRIGGER_ADDR, 0x20);
			await Task.Delay(5000);


			while (this.ReadFromAddress((byte)OrientationSensorRegister.BNO055_CHIP_ID_ADDR) != BNO055_ID)
			{
				await Task.Delay(1);
			}
			await Task.Delay(50);

			value = this.ReadFromAddress((int)OrientationSensorRegister.BNO055_SYS_TRIGGER_ADDR);
			// Console.WriteLine("Set sys trigger to ");
			// Console.WriteLine(value);
			await Task.Delay(1000);

			this.WriteToAddress((int)OrientationSensorRegister.BNO055_PWR_MODE_ADDR, (int)OrientationSensorPowerMode.POWER_MODE_NORMAL);
			value = this.ReadFromAddress((int)OrientationSensorRegister.BNO055_PWR_MODE_ADDR);
			Console.WriteLine("Set normal power mode ");
			Console.WriteLine(value);
			await Task.Delay(1000);

			this.WriteToAddress((int)OrientationSensorRegister.BNO055_PAGE_ID_ADDR, 0);
			value = this.ReadFromAddress((byte)OrientationSensorRegister.BNO055_PAGE_ID_ADDR);
			Console.WriteLine("Set page address ");
			Console.WriteLine(value);
			await Task.Delay(1000);

			this.WriteToAddress((int)OrientationSensorRegister.BNO055_SYS_TRIGGER_ADDR, 0x0);
			value = this.ReadFromAddress((byte)OrientationSensorRegister.BNO055_SYS_TRIGGER_ADDR);
			Console.WriteLine("Set sys trigger to ");
			Console.WriteLine(value);
			await Task.Delay(1000);

			this.WriteToAddress((int)OrientationSensorRegister.BNO055_OPR_MODE_ADDR, (int)OrientationSensorOperationMode.OPERATION_MODE_NDOF);
			value = this.ReadFromAddress((int)OrientationSensorRegister.BNO055_OPR_MODE_ADDR);
			Console.WriteLine("Operation mode set to ");
			Console.WriteLine(value);
			await Task.Delay(1000);
			return true;
		}

		private void Sleep(float time)
		{
			Thread.Sleep((int)(time * 1000));
		}

        private void WriteToAddress(byte address, byte data)
        {
            this.i2cDevice.Write(new byte[] { 
                address, 
                data 
            });
		}

		private byte[] ReadFromAddress(byte address, int length)
		{
			this.i2cDevice.Write(new byte[] {
				address
			});
			byte[] buffer = new byte[length];
			this.i2cDevice.Read(buffer);
			return buffer;
		}

		private byte ReadFromAddress(byte address)
		{
			this.i2cDevice.Write(new byte[] {
				address
			});
			byte[] buffer = new byte[1];
			this.i2cDevice.Read(buffer);
			return buffer[0];
		}

		public Vector3 GetOrientation()
		{
			var bytes = ReadFromAddress((byte)OrientationSensorRegister.BNO055_EULER_H_LSB_ADDR, 6);

			short yaw = BitConverter.ToInt16(bytes, 0);
			short pitch = BitConverter.ToInt16(bytes, 2);
			short roll = BitConverter.ToInt16(bytes, 4);

			return new Vector3(yaw / 16.0f, pitch / 16.0f, roll / 16.0f);
		}
	}
}
