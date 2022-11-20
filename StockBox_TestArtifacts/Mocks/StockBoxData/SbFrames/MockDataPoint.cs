using System;
using System.Collections.Generic;
using StockBox.Data.SbFrames;

namespace StockBox_TestArtifacts.Mocks.StockBoxData.SbFrames
{
    public class MockDataPoint
    {
        public DateTime? Date { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double AdjClose { get; set; }
        public double Volume { get; set; }
        public List<IndicatorDataPoint> Indicators { get; set; }

        public MockDataPoint(DateTime? date = null, double? high = null, double? low = null, double? open = null, double? close = null, double? adjClose = null, double? volume = null, List<IndicatorDataPoint> indicators = null)
        {
            Date = date;
            High = high ?? 0;
            Low = low ?? 0;
            Open = open ?? 0;
            Close = close ?? 0;
            AdjClose = adjClose ?? 0;
            Volume = volume ?? 0;
            Indicators = indicators ?? new List<IndicatorDataPoint>();
        }
    }
}
