using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Drone.Core.QLearning
{
    public class QTableRow
    {
        private readonly float minThrottle;
        private readonly float maxThrottle;
        private readonly float throttleIncrement;
        private readonly Func<float, float> defaultValueConstructor;
        public Dictionary<int, float> tableColumns;
        private readonly Random randomNumberGenerator;


        public QTableRow(float minThrottle, float maxThrottle, float throttleIncrement, Func<float, float> defaultValueConstructor)
        {
            this.minThrottle = minThrottle;
            this.maxThrottle = maxThrottle;
            this.throttleIncrement = throttleIncrement;
            this.defaultValueConstructor = defaultValueConstructor;

            this.tableColumns = new Dictionary<int, float>();
            this.randomNumberGenerator = new Random();

            InitializeRow();
        }

        public QTableRow(float minThrottle, float maxThrottle, float throttleIncrement, float defaultValue = 1000)
            : this(minThrottle, maxThrottle, throttleIncrement, (action) => defaultValue)
        { }

        public float GetAction()
        {
            float total = this.tableColumns.Sum(column => column.Value);
            float selected = (float)this.randomNumberGenerator.NextDouble() * total;
            Debug.WriteLine($"Selected {selected} for {total}");

            float current = 0;
            foreach(var keyValuePair in this.tableColumns)
            {
                if (current >= selected)
                {
                    return keyValuePair.Key * throttleIncrement;
                }
                current += keyValuePair.Value;
            }
            return 0;
        }

        public void StoreActionResult(int action, float result)
        {
            Debug.WriteLine($"Storing action result {result} for action {action}");
            this.tableColumns[action] = result;
        }

        private void InitializeRow()
        {
            for (float i = minThrottle; i < maxThrottle; i += throttleIncrement)
            {
                this.tableColumns[GetIndexForOffset(i, throttleIncrement)] = defaultValueConstructor(i);
            }
        }

        private int GetIndexForOffset(float value, float increment)
        {
            return (int)MathF.Round(value / increment);
        }
    }
}
