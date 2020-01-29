using Drone.Core.Devices.Motors;
using Drone.Core.Devices.Pwm;
using Drone.Core.Sensors.Gps;
using Drone.Core.Sensors.Orientation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Drone.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");


            Task.Run(async () =>
            {
                using (GpsSensor sensor = new GpsSensor())
                {
                    await sensor.Init();
                    Console.WriteLine("GPS Sensor opened");
                    for (int i = 0; i < 10; i++)
                    {
                        Console.WriteLine($"Longitude: {sensor.Longitude}, latitude: {sensor.Latitude}");
                        await Task.Delay(1000);
                    }
                }
                Console.WriteLine("GPS Sensor closed");
            });

            Task.Run(async () =>
            {
                OrientationSensor sensor = new OrientationSensor();
                await sensor.Init();

                while (true)
                {
                    Console.WriteLine($"Orientation: {sensor.GetOrientation()}");
                    await Task.Delay(500);
                }
            });

            Task.Run(async () =>
            {
                PwmController pwmController = new PwmController();
                pwmController.Init();
                await Task.Delay(1000);

                Motor[] motors = new Motor[4];
                for (byte i = 0; i < 16; i += 4)
                {
                    motors[i / 4] = new Motor(pwmController, i);
                }

                Task[] tasks = new Task[motors.Length];
                for (int i = 0; i < motors.Length; i++)
                {
                    tasks[i] = motors[i].Arm(0, 1);
                }
                await Task.WhenAll(tasks);

                foreach(var motor in motors)
                {
                    motor.Run(0.05);
                }
                await Task.Delay(1000);
                foreach (var motor in motors)
                {
                    motor.Run(0.0);
                }

            });

            Thread.Sleep(-1);
        }
    }
}
