using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using Deedle;
using System.Data;
using System.Linq;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>SlowStochastic</c>
    /// </summary>
    public class SlowStochastic : BaseIndicator
    {

        public SlowStochastic(string column, params int[] indices) : base(column, EIndicatorType.eSlowStochastics, indices)
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, double>();

            // guard out if the adapter hasn't been sourced. 
            if (adapter.SourceData == null) return ret;

            // averages is a collection of previous values cached and passed
            // to the aggregation function. The last value is needed for the
            // next calculated averages
            List<(double gain, double loss)> averages = new List<(double gain, double loss)>();

            // pull a series (column) from the data in the Close column
            var orderedSeries = adapter.SourceData.GetColumn<double>("Close").SortByKey();

            var values = orderedSeries.WindowInto(Indices[0], win => CalculateSlowStochastic(win));
            for (var idx = 0; idx < values.KeyCount; idx++)
            {
                var key = values.Keys.ToOrdinalSeries<DateTime>()[idx];
                var innerValue = values[key];
                ret.Add(key, innerValue);
            }

            return ret;

        }


        protected double CalculateSlowStochastic(Series<DateTime, double> values)
        {
            return 0;
        }
    }
}