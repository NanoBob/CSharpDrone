using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Drone.Core.QLearning
{
    public class QTable
    {
        private readonly float minOffset;
        private readonly float maxOffset;
        private readonly float offsetIncrements;
        private readonly float minThrottle;
        private readonly float maxThrottle;
        private readonly float throttleIncrement;
        private Dictionary<int, QTableRow> tableRows;

        public QTable(float minOffset, float maxOffset, float offsetIncrements, float minThrottle, float maxThrottle, float throttleIncrement)
        {
            this.minOffset = minOffset;
            this.maxOffset = maxOffset;
            this.offsetIncrements = offsetIncrements;
            this.minThrottle = minThrottle;
            this.maxThrottle = maxThrottle;
            this.throttleIncrement = throttleIncrement;

            this.tableRows = new Dictionary<int, QTableRow>();

            InitializeTable();
        }

        public void LoadJson(string json)
        {
            this.tableRows = JsonConvert.DeserializeObject<Dictionary<int, QTableRow>>(json);
        }

        public string SaveToJson()
        {
            return JsonConvert.SerializeObject(this.tableRows, Formatting.Indented);
        }

        public float GetAction(float offset)
        {
            int index = GetIndexForOffset(offset, offsetIncrements);
            if (!this.tableRows.ContainsKey(index))
            {
                CreateQTableRow(offset);
            }
            return this.tableRows[GetIndexForOffset(index, offsetIncrements)].GetAction();
        }

        public void StoreActionResult(float offset, float action, float result)
        {
            var roundedOffset = GetIndexForOffset(offset, offsetIncrements);
            var roundedAction = GetIndexForOffset(action, throttleIncrement);
            Debug.WriteLine($"Storing {result} as result for action {roundedAction} on offset {roundedOffset}");
            if (this.tableRows.ContainsKey(roundedOffset))
            {
                this.tableRows[roundedOffset].StoreActionResult(roundedAction, result);
            } else
            {
                Debug.WriteLine($"No entry found for offset {roundedOffset}");
            }
        }

        private void InitializeTable()
        {
            for (float i = minOffset; i < maxOffset; i += offsetIncrements)
            {
                CreateQTableRow(i);
            }
        }

        private QTableRow CreateQTableRow(float offset)
        {
            QTableRow row = new QTableRow(minThrottle, maxThrottle, throttleIncrement);
            this.tableRows[GetIndexForOffset(offset, offsetIncrements)] = row;
            return row;
        }

        private int GetIndexForOffset(float value, float increment)
        {
            return (int)MathF.Round(value / increment);
        }
    }
}
