using Drone.Core.Sensors.Orientation;
using System;
using System.Threading;

namespace Drone.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            OrientationSensor sensor = new OrientationSensor();
            var initialized = sensor.Init().Result;

            while (true)
            {
                Console.WriteLine($"Orientation: {sensor.GetOrientation()}");
                Thread.Sleep(500);
            }
        }
    }
}
