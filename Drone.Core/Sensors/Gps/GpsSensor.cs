using NmeaParser;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Sensors.Gps
{
    public class GpsSensor
    {
        private readonly SerialPort serialPort;
        private readonly SerialPortDevice SerialPortDevice;

        public double Longitude { get; private set; }
        public double Latitude { get; private set; }

        public GpsSensor()
        {
            this.serialPort = new SerialPort("/dev/serial0");
            this.SerialPortDevice = new SerialPortDevice(this.serialPort);
        }

        public async Task Init()
        {
            this.SerialPortDevice.MessageReceived += HandleNmeaMessage;

            await this.SerialPortDevice.OpenAsync();
        }

        private void HandleNmeaMessage(object sender, NmeaMessageReceivedEventArgs eventArgs)
        {
            if (eventArgs.Message is NmeaParser.Messages.Rmc rmc)
            {
                this.Longitude = rmc.Longitude;
                this.Latitude = rmc.Latitude;
                Console.WriteLine($"Your current location is: {rmc.Longitude}, {rmc.Latitude}");
            }
        }
    }
}
