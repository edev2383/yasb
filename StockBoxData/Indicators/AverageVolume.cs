using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>AverageVolume</c> is a smoothing function for daily volume
    /// values. Allows for easy comparisons if volume is increasing or
    /// decreasing against the average
    /// </summary>
    public class AverageVolume : BaseIndicator<Dictionary<DateTime, double>>
    {

        public AverageVolume(string column, params int[] indices) : base(column, EIndicatorType.volume, indices)
        {
        }

        protected override Dictionary<DateTime, double> CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, double>();

            // apply the Mean method over the window of length = Indices[0]
            var values = provider.GetFullDataSource().ToSeries("Volume").SortByKey().Window(Indices[0], win => win.Mean());

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
