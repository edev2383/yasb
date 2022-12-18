using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>Slope</c> is a calculation of the % of rate of change of a
    /// column over a given period of time (range). This can be used to provide
    /// context to other indicators, and will create the required indidcators
    /// if none already exist.
    /// </summary>
    public class Slope : BaseIndicator
    {
        public Slope(string column, int range) : base(column, EIndicatorType.eSlope, new int[1] { range })
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, double>();

            if (adapter.IndicatorExists(this) != true)
            {
                // create the indicator
                var dataColumn = DataColumn.ParseColumnDescriptor(ColumnKey);
                var targetInidicator = IndicatorFactory.Create(dataColumn.ColumnToken, dataColumn.Indices.ToArray());
                adapter.Parent.AddIndicator(targetInidicator);
            }

            var values = adapter.GetFullDataSource()
                .ToSeries(ColumnKey)
                .SortByKey()
                .Window(
                    Indices[0]
                    , win => CalculatePercentRateOfChange(win, Indices[0]));

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

