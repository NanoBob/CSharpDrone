using Drone.Core.Sensors.Gps;
using Drone.Core.Structs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drone.Core.Controllers
{
    public class GpsController
    {
        private IGpsSensor gpsSensor;

        public Position Position { get; private set; } = new Position() { latitude = 0, longitude = 0 };

        private bool enabled;
        public bool IsEnabled
        {
            get => this.enabled;
            set
            {
                if (this.enabled != value)
                {
                    if (value)
                    {
                        this.gpsSensor.OnPositionDetected += UpdatePosition;
                    } else
                    {
                        this.gpsSensor.OnPositionDetected -= UpdatePosition;
                    }
                    this.enabled = value;
                }
            }
        }

        public GpsController()
        {
            this.gpsSensor = new GpsSensor();
        }

        public async Task Init()
        {
            await this.gpsSensor.Init();
        }

        private void UpdatePosition(Position position)
        {
            this.Position = position;
            this.PositionChanged?.Invoke(position);
        }

        public event Action<Position>? PositionChanged;
    }
}
