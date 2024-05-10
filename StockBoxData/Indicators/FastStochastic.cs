using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using System.Linq;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>FastStochastic</c> is a momentum indicator that oscillates
    /// between 0 and 100 depending on over-bought/over-sold conditions
    ///
    /// <see cref="https://www.fidelity.com/learning-center/trading-investing/technical-analysis/technical-indicator-guide/fast-stochastic"/>
    /// </summary>
    public class FastStochastic : BaseIndicator<Dictionary<DateTime, (double k, double d)>>
    {

        public FastStochastic(string column, params int[] indices) : base(column, EIndicatorType.fastStochastics, indices)
        {
        }


        protected override Dictionary<DateTime, (double k, double d)> CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, (double k, double d)>();

            // averages is a collection of previous values cached and passed
            // to the aggregation function. The last value is needed for the
            // next calculated averages
            List<(double gain, double loss)> averages = new List<(double gain, double loss)>();

            // pull a series (column) from the data in the Close column
            var data = provider.GetFullDataSource().Reversed;

            var kseries = data.Window(Indices[0], x => CalculateFastStochastic(x));
            var dseries = kseries.Window(Indices[1], win => win.Mean());

            for (var idx = 0; idx < kseries.KeyCount; idx++)
            {
                var k = kseries.ElementAt(idx);
                if (dseries.ContainsKey(k.Key))
                {
                    var d = dseries[k.Key];
                    ret.Add(k.Key, (k.Value, d));
                }
            }

            return ret;
        }

        protected double CalculateFastStochastic(DataPointList values)
        {
            double mostRecentClosePrice = values.Last().Close;
            double lowestLowInWindow = values.Min("low");
            double highestHighInWindow = values.Max("high");
            double k = 100 * ((mostRecentClosePrice - lowestLowInWindow) / (highestHighInWindow - lowestLowInWindow));
            return k;
        }

    }
}
