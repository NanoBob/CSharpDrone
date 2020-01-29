using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Drone.Core.Structs
{
    public struct Orientation
    {
        public float Yaw { get; set; }
        public float Pitch { get; set; }
        public float Roll { get; set; }

        public Orientation(float yaw, float pitch, float roll)
        {
            this.Yaw = yaw;
            this.Pitch = pitch;
            this.Roll = roll;
        }

        public Orientation(Vector3 vector)
        {
            this.Yaw = vector.X;
            this.Pitch = vector.Y;
            this.Roll = vector.Z;
        }
    }
}
