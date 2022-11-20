using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>SimpleMovingAverage</c> is a smoothing curve indicator against
    /// the Close value of time-series data
    ///
    /// <see cref="https://www.investopedia.com/terms/s/sma.asp"/>
    /// </summary>
    public class SimpleMovingAverage : BaseIndicator
    {

        public SimpleMovingAverage(string column, params int[] indices) : base(column, EIndicatorType.eSma, indices)
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, double>();

            // apply the Mean method over the window of length = Indices[0]
            // the SortByKey() call may be unnecessary, however, it's probably
            // better to be safe
            var values = adapter.GetFullDataSource().ToSeries("Close").SortByKey()
                                 .Window(Indices[0], win => win.Mean());

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
