﻿using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using System.Linq;
using StockBox.Data.SbFrames;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>SlowStochastic</c> is a momentum osillator indicator, related
    /// to the FastStochastic indicator, but smoothed out, typically with a
    /// SMA(3) applied to the FastStochastics %D value
    ///
    /// <see cref="https://www.fidelity.com/learning-center/trading-investing/technical-analysis/technical-indicator-guide/slow-stochastic"/>
    /// </summary>
    public class SlowStochastic : BaseIndicator
    {

        public SlowStochastic(string column, params int[] indices) : base(column, EIndicatorType.eSlowStochastics, indices)
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, (double k, double d)>();

            var fastSto = IndicatorFactory.Create("FastSto", Indices);
            fastSto.Calculate(adapter);
            adapter.MapIndicator(fastSto);

            var data = adapter.GetFullDataSource().Reversed;

            var kseries = new SbSeries("SlowSto.k");
            var dataColumn = SbFrames.DataColumn.ParseColumnDescriptor(fastSto.Name);
            foreach (var dp in data)
            {
                var indicator = dp.Indicators.FindByKey(fastSto.Name);
                if (indicator != null)
                    kseries.Add(dp.Date, (double)indicator.Values[1]);
            }

            // pull a series (column) from the data in the Close column
            //var kseries = data.ToSeries(fastSto.Name).Window(Indices[0], win => win.Mean());
            var dseries = kseries.Window(Indices[1], win => win.Mean());
            //var orderedSeries = adapter.SourceData.GetColumn<double>("Close").SortByKey();

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

    }
}