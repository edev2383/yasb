using NHibernate.Hql.Ast;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Data.Indicators
{
    public class AverageTrueRange : BaseIndicator
    {
        public AverageTrueRange(string column,  params int[] indices) : base(column, EIndicatorType.eAverageTrueRange, indices)
        {
        }

        protected override object CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, double>();

            var atrs = new List<double>();
            var values = provider.GetFullDataSource().Reversed.Window(Indices[0], x => CalculateAverageTrueRange(x, ref atrs));

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


        /// <summary>
        /// Calculate the True Range for a given values
        /// </summary>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <returns></returns>
        private double CalculateTrueRange(double high, double low, double close)
        {
            var h = Math.Round(high, 2);
            var l = Math.Round(low, 2);
            var c = Math.Round(close, 2);
            var val = Math.Max(h - l, Math.Max(Math.Abs(h - c), Math.Abs(l - c)));
            return val;
        }

        /// <summary>
        /// Calculate the Average True Range for a given values
        /// </summary>
        /// <param name="values"></param>
        /// <param name="atrs"></param>
        /// <returns></returns>
        private double CalculateAverageTrueRange(DataPointList values, ref List<double> atrs)
        {
            double atr = 0;
            if (atrs.Count == 0)
            {
                
                atr = values.Average(x => CalculateTrueRange(x.High, x.Low, x.Close));
            }
            else
            {
                // Current ATR = [(Prior ATR x 13) + Current TR] / 14
                var prev = values.ElementAt(Indices[0] - 2);
                
                atr = ((atrs.Last() * (Indices[0] - 1)) + CalculateTrueRange(values.Last().High, values.Last().Low, prev.Close)) / Indices[0];
            }
            atrs.Add(atr);
            return atr;
        }


    }
}
