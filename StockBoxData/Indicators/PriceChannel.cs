using StockBox.Data.SbFrames.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockBox.Data.Indicators
{
    public class PriceChannel : BaseIndicator<Dictionary<DateTime, (double high, double center, double low)>>
    {
        public PriceChannel(string column, params int[] indices) : base(column, EIndicatorType.priceChannel, indices)
        {
        }

        protected override Dictionary<DateTime, (double high, double center, double low)> CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, (double high, double center, double low)>();

            var highs = provider.GetFullDataSource().Reversed.ToSeries("High").Window(Indices[0] + 1, x => x.SkipLast(1).Max(x => x.Value));
            var lows = provider.GetFullDataSource().Reversed.ToSeries("Low").Window(Indices[0] + 1, x => x.SkipLast(1).Min(x => x.Value));

            // loop through the result set
            for (var idx = 0; idx < highs.Count; idx++)
            {
                var h = highs.ElementAt(idx);
                var l = lows.ElementAt(idx);
                // acquire the element at a given index
                // add the DateTime Key and double value to the return object
                ret.Add(h.Key, (h.Value, (h.Value + l.Value) / 2, l.Value));
            }

            return ret;
        }
    }
}
