﻿using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using System.Linq;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>RelativeStrengthIndex</c> is a price-trend indicator
    ///
    /// <see cref="https://www.omnicalculator.com/finance/rsi"/>
    /// </summary>
    public class RelativeStrengthIndex : BaseIndicator<Dictionary<DateTime, double>>
    {
        public RelativeStrengthIndex(string column, params int[] indices) : base(column, EIndicatorType.rsi, indices)
        {
        }

        protected override Dictionary<DateTime, double> CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, double>();

            // we need to cache the averages created by the calculation without
            // mutating any original data, so we create this list to hold onto
            // our averages as we go. We'll need to reference the Last() tuple
            // on each iteration of the window
            List<(double gain, double loss)> averages = new List<(double gain, double loss)>();

            // apply the Mean method over the window of length = Indices[0]
            // the SortByKey() call may be unnecessary, however, it's probably
            // better to be safe
            var values = provider.GetFullDataSource()
                                    .ToSeries("Close")
                                    .SortByKey()
                                    .Window(
                                    Indices[0],
                                    win => CalculateRsi(win, ref averages, Indices[0]));

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
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="averages"></param>
        /// <param name="window"></param>
        /// <returns></returns>
        protected double CalculateRsi(SbSeries values, ref List<(double gain, double loss)> averages, int window)
        {
            double avgGain = 0;
            double avgLoss = 0;
            double rs = 0;
            double rsi = 0;

            if (averages.Count == 0)
            {
                // this path is seed data. Create the first average values,
                // after which, we can compare against series Close data as we
                // move along the window
                avgGain = values.Diff(1).Select(kvp => kvp.Value > 0 ? kvp.Value : 0).Sum() / window;
                avgLoss = Math.Abs(values.Diff(1).Select(kvp => kvp.Value < 0 ? kvp.Value : 0).Sum()) / window;
            }
            else
            {
                // now that we have an extended set of values, we can target
                // only the current diff as a single gain-loss value which will
                // act to smooth the values
                double currentGainLoss = values.Diff(1).Last().Value;
                (double gain, double loss) currentAverages = averages.Last();
                // apply the gainloss value only to the proper average, making
                // sure to use the absolute value in the case of the a loss
                avgGain = ((currentAverages.gain * 13) + (currentGainLoss > 0 ? currentGainLoss : 0)) / window;
                avgLoss = ((currentAverages.loss * 13) + (currentGainLoss < 0 ? Math.Abs(currentGainLoss) : 0)) / window;
            }

            // store the averages in the cache list
            averages.Add((avgGain, avgLoss));

            // rsi calculation. see url in class def for more
            rs = avgGain / avgLoss;
            rsi = 100 - (100 / (1 + rs));
            return rsi;
        }
    }
}
