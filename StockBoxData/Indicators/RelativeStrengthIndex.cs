using System;
using System.Collections.Generic;
using StockBox.Data.Adapters.DataFrame;
using Deedle;
using System.Data;
using System.Linq;

namespace StockBox.Data.Indicators
{
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

            // rsi is a collection of previous values
            List<(double gain, double loss)> averages = new List<(double gain, double loss)>();

            //var test = adapter.SourceData.Window(Indices[0], win => CalculateRsi(win, ref rsi));
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
            double currentGainLoss = 0;
            (double gain, double loss) currentAverages = (0, 0);
            double avgGain = 0;
            double avgLoss = 0;
            double rs = 0;
            double rsi = 0;
            if (averages.Count < window)
            {
                avgGain = values.Diff(1).Select(kvp => kvp.Value > 0 ? kvp.Value : 0).Sum() / window;
                avgLoss = Math.Abs(values.Diff(1).Select(kvp => kvp.Value < 0 ? kvp.Value : 0).Sum()) / window;
            }
            else
            {
                currentGainLoss = values.Diff(1).LastValue();
                currentAverages = averages.Last();
                avgGain = ((currentAverages.gain * 13) + (currentGainLoss > 0 ? currentGainLoss : 0)) / window;
                avgLoss = ((currentAverages.loss * 13) + (currentGainLoss < 0 ? Math.Abs(currentGainLoss) : 0)) / window;
            }

            averages.Add((avgGain, avgLoss));
            rs = avgGain / avgLoss;
            rsi = 100 - (100 / (1 + rs));
            return rsi;
        }

        public static DataTable data;
        public static double[] positiveChanges;
        public static double[] negativeChanges;
        public static double[] averageGain;
        public static double[] averageLoss;
        public static double[] rsi;

        public static double CalculateDifference(double current_price, double previous_price)
        {
            return current_price - previous_price;
        }

        public static double CalculatePositiveChange(double difference)
        {
            return difference > 0 ? difference : 0;
        }

        public static double CalculateNegativeChange(double difference)
        {
            return difference < 0 ? difference * -1 : 0;
        }

        public static void CalculateRSI(int rsi_period, int price_index = 5)
        {
            for (int i = 0; i < data.Rows.Count; i++)
            {
                double current_difference = 0.0;
                if (i > 0)
                {
                    double previous_close = Convert.ToDouble(data.Rows[i - 1].Field<string>(price_index));
                    double current_close = Convert.ToDouble(data.Rows[i].Field<string>(price_index));
                    current_difference = CalculateDifference(current_close, previous_close);
                }
                positiveChanges[i] = CalculatePositiveChange(current_difference);
                negativeChanges[i] = CalculateNegativeChange(current_difference);

                if (i == Math.Max(1, rsi_period))
                {
                    double gain_sum = 0.0;
                    double loss_sum = 0.0;
                    for (int x = Math.Max(1, rsi_period); x > 0; x--)
                    {
                        gain_sum += positiveChanges[x];
                        loss_sum += negativeChanges[x];
                    }

                    averageGain[i] = gain_sum / Math.Max(1, rsi_period);
                    averageLoss[i] = loss_sum / Math.Max(1, rsi_period);

                }
                else if (i > Math.Max(1, rsi_period))
                {
                    averageGain[i] = (averageGain[i - 1] * (rsi_period - 1) + positiveChanges[i]) / Math.Max(1, rsi_period);
                    averageLoss[i] = (averageLoss[i - 1] * (rsi_period - 1) + negativeChanges[i]) / Math.Max(1, rsi_period);
                    rsi[i] = averageLoss[i] == 0 ? 100 : averageGain[i] == 0 ? 0 : Math.Round(100 - (100 / (1 + averageGain[i] / averageLoss[i])), 5);
                }
            }
        }

        public static void Launch()
        {
            data = new DataTable();
            //load {date, time, open, high, low, close} values in data (6th column (index #5) = close price) here

            positiveChanges = new double[data.Rows.Count];
            negativeChanges = new double[data.Rows.Count];
            averageGain = new double[data.Rows.Count];
            averageLoss = new double[data.Rows.Count];
            rsi = new double[data.Rows.Count];

            CalculateRSI(14);
        }

    }
}
