using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>AverageVolume</c> is a smoothing function for daily volume
    /// values. Allows for easy comparisons if volume is increasing or
    /// decreasing against the average
    /// </summary>
    public class AverageVolume : BaseIndicator
    {

        public AverageVolume(string column, params int[] indices) : base(column, EIndicatorType.eVolume, indices)
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, double>();

            // guard out if the adapter hasn't been sourced. 
            if (adapter.SourceData == null) return ret;

            // apply the Mean method over the window of length = Indices[0]
            var values = adapter.GetSeries("Volume").SortByKey().Window(Indices[0], win => win.Mean());

            // loop through the result set
            for (var idx = 0; idx < values.Count; idx++)
            {
                // acquire the element at a given index
                var e = values.ElementAt(idx);
                // add the DateTime Key and double value to the return object
                ret.Add(e.Key, e.Value);
            }

            return ret;
        }
    }
}
