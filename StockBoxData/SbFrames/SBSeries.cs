using System;
using System.Collections.Generic;

namespace StockBox.Data.SbFrames
{
    public class SBSeries : List<Dictionary<DateTime, double>>
    {
        public SBSeries(IEnumerable<Dictionary<DateTime, double>> source)
        {
            AddRange(source);
        }
    }
}
