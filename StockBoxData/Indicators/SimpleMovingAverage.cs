using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>SimpleMovingAverage</c> is a smoothing curve indicator against
    /// the Close value of time-series data
    ///
    /// <see cref="https://www.investopedia.com/terms/s/sma.asp"/>
    /// </summary>
    public class SimpleMovingAverage : BaseIndicator<Dictionary<DateTime, double>>
    {
        private readonly string _targetColumn;
        public SimpleMovingAverage(string column, string target = "Close", params int[] indices) : base(column, EIndicatorType.sma, indices)
        {
            _targetColumn = target;
        }

        public SimpleMovingAverage(string column, params int[] indices) : base(column, EIndicatorType.sma, indices)
        {
            _targetColumn = "Close";
        }


        protected override Dictionary<DateTime, double> CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, double>();

            /// To get the SimpleMovingAverage, we need to narrow the data source down
            /// to a singular series and apply a Mean() function to the windowed data.
            /// 1.) GetFullDataSource returns the entire DataPointList
            /// 2.) ToSeries converts the DataPointList to an SbSeries object of a singular
            ///     column
            /// 3.) Then SortByKey() to ensure the data is in chronological order
            /// 4.) Window() applies a Function expression to the data in the window
            var values = provider.GetFullDataSource()
                            .ToSeries(_targetColumn).SortByKey()
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
