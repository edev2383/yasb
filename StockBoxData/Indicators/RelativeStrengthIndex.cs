using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using Deedle;
using System.Data;
using System.Linq;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>RelativeStrengthIndex</c> 
    /// </summary>
    public class RelativeStrengthIndex : BaseIndicator
    {
        public RelativeStrengthIndex(string column, params int[] indices) : base(column, EIndicatorType.eRSI, indices)
        {
        }

        protected override object CalculateIndicator(IDataFrameAdapter adapter)
        {
            var ret = new Dictionary<DateTime, double>();

            // guard out if the adapter hasn't been sourced. 
            if (adapter.SourceData == null) return ret;

            // averages is a collection of previous values cached and passed
            // to the aggregation function. The last value is needed for the
            // next calculated averages
            List<(double gain, double loss)> averages = new List<(double gain, double loss)>();

            // pull a series (column) from the data in the Close column
            var orderedSeries = adapter.SourceData.GetColumn<double>("Close").SortByKey();

            var values = orderedSeries.WindowInto(Indices[0], win => CalculateRsi(win, ref averages, Indices[0]));
            for (var idx = 0; idx < values.KeyCount; idx++)
            {
                var key = values.Keys.ToOrdinalSeries<DateTime>()[idx];
                var innerValue = values[key];
                ret.Add(key, innerValue);
            }

            return ret;
        }

        protected double CalculateRsi(Series<DateTime, double> values, ref List<(double gain, double loss)> averages, int window)
        {
            double avgGain = 0;
            double avgLoss = 0;
            double rs = 0;
            double rsi = 0;

            // for the first winow, create the seed data by taking a simple avg
            if (averages.Count == 0)
            {
                avgGain = values.Diff(1).Select(kvp => kvp.Value > 0 ? kvp.Value : 0).Sum() / window;
                avgLoss = Math.Abs(values.Diff(1).Select(kvp => kvp.Value < 0 ? kvp.Value : 0).Sum()) / window;
            }
            else
            {
                // now that we have an extended set of values, we can target
                // only the current diff as a single gain-loss value which will
                // act to smooth the values
                double currentGainLoss = values.Diff(1).LastValue();
                (double gain, double loss) currentAverages = averages.Last();
                // apply the gainloss value only to the proper average, making
                // sure to use the absolute value in the case of the a loss
                avgGain = ((currentAverages.gain * 13) + (currentGainLoss > 0 ? currentGainLoss : 0)) / window;
                avgLoss = ((currentAverages.loss * 13) + (currentGainLoss < 0 ? Math.Abs(currentGainLoss) : 0)) / window;
            }

            // store the averages in the cache list
            averages.Add((avgGain, avgLoss));

            rs = avgGain / avgLoss;
            rsi = 100 - (100 / (1 + rs));
            return rsi;
        }
    }
}
