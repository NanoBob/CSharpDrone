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
                simulator.Start(Vector3.Zero, new Vector3(0, 10, 0), 256);
            }).Wait();
        }
    }
}
