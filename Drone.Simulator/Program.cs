using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Drone.Simulator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var simulator = new DroneSimulator();
                await simulator.Init();
                var result = simulator.Start(Vector3.Zero, new Vector3(0, 10, 0), (int)Math.Pow(2, 31));
                Console.WriteLine($"Result: {result}");
            }).Wait();
        }
    }
}
