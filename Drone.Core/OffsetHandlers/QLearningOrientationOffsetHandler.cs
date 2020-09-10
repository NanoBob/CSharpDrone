using Drone.Core.Interfaces;
using Drone.Core.QLearning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Drone.Core.OffsetHandlers
{
    public class QLearningOrientationOffsetHandler : IOrientationOffsetHandler
    {
        private readonly QTable qTable;

        private bool isFirst;
        private float previousOffset;
        private float previousAction;

        public float MinThrottle { get; }
        public float MaxThrottle { get; }
        public float ThrottleIncrement { get; }

        public QLearningOrientationOffsetHandler(
            float minOffset = -180, float maxOffset = 180, float offsetIncrements = 1, 
            float minThrottle = -0.3f, float maxThrottle = 0.3f, float throttleIncrement = 0.01f
        )
        {
            MinThrottle = minThrottle;
            MaxThrottle = maxThrottle;
            ThrottleIncrement = throttleIncrement;

            this.qTable = new QTable(minOffset, maxOffset, offsetIncrements, minThrottle, maxThrottle, throttleIncrement);
            this.isFirst = true;
        }

        public float HandleOffset(float offset)
        {
            if (!this.isFirst)
            {
                var change = offset - this.previousOffset;
                var overshoot = (this.previousOffset > 0 && offset < 0) || (this.previousOffset < 0 && offset > 0) ? MathF.Abs(offset) : 0;
                var wrongWay = MathF.Abs(offset) > MathF.Abs(this.previousOffset);
                var result = 180 + ((MathF.Abs(change) * (wrongWay ? -1 : 1)) - (overshoot * 1.5f)) * 10000;
                this.qTable.StoreActionResult(this.previousOffset, this.previousAction, result);
            }

            var action = this.qTable.GetAction(offset);
            this.previousOffset = offset;
            this.previousAction = action;

            this.isFirst = false;

            return action;
        }

        public void LoadJson(string json) => this.qTable.LoadJson(json);

        public string SaveToJson() => this.qTable.SaveToJson();
    }
}
