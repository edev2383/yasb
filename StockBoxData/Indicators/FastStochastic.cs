﻿using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using Deedle;
using System.Linq;
using StockBox.Data.SbFrames;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>FastStochastic</c>
    /// </summary>
    public class FastStochastic : BaseIndicator
    {

        public FastStochastic(string column, params int[] indices) : base(column, EIndicatorType.eFastStochastics, indices)
        {
        }


        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, (double k, double d)>();

            // guard out if the adapter hasn't been sourced. 
            if (adapter.SourceData == null) return ret;

            // averages is a collection of previous values cached and passed
            // to the aggregation function. The last value is needed for the
            // next calculated averages
            List<(double gain, double loss)> averages = new List<(double gain, double loss)>();

            // pull a series (column) from the data in the Close column
            var data = adapter.GetData().Reversed;

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
