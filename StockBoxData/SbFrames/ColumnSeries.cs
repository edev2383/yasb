using System;
using System.Collections.Generic;

namespace StockBox.Data.SbFrames
{
    public class ColumnSeries: Dictionary<DateTime, object>
    {
        public string ColumnKey { get; set; }

        public ColumnSeries(string key)
        {
            ColumnKey = key;
        }
    }
}
