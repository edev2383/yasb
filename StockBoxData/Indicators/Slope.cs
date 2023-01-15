using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>Slope</c> is a calculation of the % of rate of change of a
    /// column over a given period of time (range, default=3). This can be used
    /// to provide context to other indicators, such as if SMA/EMAs are
    /// converging or diverging. If the related indicator does not exist, it
    /// will be created at run-time.
    ///
    /// "Slope[SMA(5)] >< Slope[SMA(20)]"
    /// </summary>
    public class Slope : BaseIndicator
    {
        public Slope(string column, int range = 3) : base(column, EIndicatorType.eSlope, new int[1] { range })
        {
        }

        protected override object CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, double>();

            // Parse the column and create an indicator proto-object.
            var dataColumn = DataColumn.ParseColumnDescriptor(ColumnKey);
            var targetInidicator = IndicatorFactory.Create(dataColumn.ColumnToken, dataColumn.Indices.ToArray());

            // check that the target indicator object already exists. If not,
            // add it to the adapter parent SbFrame
            if (provider.IndicatorExists(targetInidicator) != true)
                provider.Parent.AddIndicator(targetInidicator);


            var values = provider.GetFullDataSource()
                .ToSeries(ColumnKey)
                .SortByKey()
                .Window(
                    Indices[0],
                    win => CalculatePercentRateOfChange(win, Indices[0]));

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

        private double CalculatePercentRateOfChange(SbSeries values, int range)
        {
            var firstValue = values.FirstValue();
            var priceChange = values.LastValue() - firstValue;
            var percentChange = (priceChange / firstValue) * 100;
            return percentChange / range;
        }
    }
}

