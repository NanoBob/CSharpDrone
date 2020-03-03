using Drone.Core.Interfaces;
using Drone.Core.QLearning;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Drone.Core.Controllers
{
    public class QLearningOrientationOffsetHandler : IOrientationOffsetHandler
    {
        private QTable qTable;

        private bool isFirst;
        private float previousOffset;
        private float previousAction;

        public QLearningOrientationOffsetHandler()
        {
            this.qTable = new QTable(-180, 180, 1, -0.3f, 0.3f, 0.01f);

            this.isFirst = true;
        }

        public float HandleOffset(float offset)
        {
            if (!this.isFirst)
            {
                var change = this.previousOffset - offset;
                var overshoot = (this.previousOffset > 0 && offset < 0) || (this.previousOffset < 0 && offset > 0) ? MathF.Abs(offset) : 0;
                var result = 180 + (MathF.Abs(change) - (overshoot * 1.5f)) * 1000;
                this.qTable.StoreActionResult(this.previousOffset, this.previousAction, result);
            }

            var action = this.qTable.GetAction(offset);
            Debug.WriteLine($"Performing {action}");
            this.previousOffset = offset;
            this.previousAction = action;

            this.isFirst = false;

            return action;
        }

        public void LoadJson(string json) => this.qTable.LoadJson(json);

        public string SaveToJson() => this.qTable.SaveToJson();
    }
}
