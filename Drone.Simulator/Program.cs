using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Drone.Simulator
{
    public class Program
    {
        static void Main()
        {
            Task.Run(async () =>
            {
                var simulator = new DroneSimulator();
                await simulator.Init();
                var result = simulator.Start(Vector3.Zero, new Vector3(10, 20, 30), (uint)Math.Pow(2, 16));
                Console.WriteLine($"Result: {result}");
            }).Wait();
        }
    }
}
