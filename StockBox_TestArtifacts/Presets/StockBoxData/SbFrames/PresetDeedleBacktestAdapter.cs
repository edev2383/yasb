using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox_TestArtifacts.Builders.StockBoxData.Adapters.DataFrame;

namespace StockBox_TestArtifacts.Presets.StockBoxData.SbFrames
{
    public class PresetDeedleBacktestAdapter
    {

        /// <summary>
        /// Return a 3-day dataset
        /// </summary>
        /// <returns></returns>
        public static DeedleBacktestAdapter ThreeDaySmaCrossOver()
        {
            return new DataFrameBuilder()
                            .WithCloseAndIndicator(null, 100, new IndicatorDataPoint("SMA(2)", 110))
                            .WithCloseAndIndicator(null, 101, new IndicatorDataPoint("SMA(2)", 100))
                            .WithCloseAndIndicator(null, 101)
                            .Build<DeedleBacktestAdapter>();
        }
    }
}
