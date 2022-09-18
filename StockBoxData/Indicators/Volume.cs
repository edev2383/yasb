using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using Deedle;


namespace StockBox.Data.Indicators
{

    public class Volume : BaseIndicator
    {
        public Volume(string column, params int[] indices) : base(column, EIndicatorType.eVolume, indices)
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, double>();

            // guard out if the adapter hasn't been sourced. 
            if (adapter.SourceData == null) return ret;

            var orderedSeries = adapter.SourceData.GetColumn<double>("Volume").SortByKey();
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
