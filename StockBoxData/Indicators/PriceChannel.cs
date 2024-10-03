using StockBox.Data.SbFrames.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Indicator <c>PriceChannel</c> calculates the high, center, and low values for a 
    /// given period. The period is for the (n) number of days prior to the current date, 
    /// i.e., the current date is not included in the calculation (otherwise a "breakout" 
    /// for the n+1 day would be impossible).
    /// </summary>
    public class PriceChannel : BaseIndicator<Dictionary<DateTime, (double high, double center, double low)>>
    {
        public PriceChannel(string column, params int[] indices) : base(column, EIndicatorType.priceChannel, indices)
        {
        }

        protected override Dictionary<DateTime, (double high, double center, double low)> CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, (double high, double center, double low)>();

            var highs = provider.GetFullDataSource()
                            .ToSeries("High").SortByKey()
                                .Window(Indices[0] + 1, x => x.SkipLast(1).Max(x => x.Value));
            
            var lows = provider.GetFullDataSource().Reversed
                            .ToSeries("Low").Window(
                                Indices[0] + 1, x => x.SkipLast(1).Min(x => x.Value));

            // loop through the result set
            for (var idx = 0; idx < highs.Count; idx++)
            {
                var h = highs.ElementAt(idx);
                var l = lows.ElementAt(idx);

                // acquire the element at a given index
                // add the DateTime Key and double valuesa to the return object
                ret.Add(h.Key, (h.Value, (h.Value + l.Value) / 2, l.Value));
            }

            return ret;
        }
    }
}
