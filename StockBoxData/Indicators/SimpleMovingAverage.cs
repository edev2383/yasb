using System;
using System.Collections.Generic;
using Deedle;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Indicators
{
    public class SimpleMovingAverage : BaseIndicator
    {
        public SimpleMovingAverage(string column, params int[] indices) : base(column, EIndicatorType.eSma, indices)
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, double>();
            // so far all series calcs will take place chronologically and
            // return an equally ordered result set, however, the DataPointList
            // will be able to take that ordered resultset and apply those
            // values to the appropriate DataPoint object, by shared DateTime
            // key
            var orderedSeries = adapter.SourceData.GetColumn<double>("Close").SortByKey();
            var values = orderedSeries.WindowInto(Indices[0], win => win.Mean());
            for (var idx = 0; idx < values.KeyCount; idx++)
            {
                var key = values.Keys.ToOrdinalSeries<DateTime>()[idx];
                var innerValue = values[key];
                ret.Add(key, innerValue);
            }

            return ret;
        }
    }
}
