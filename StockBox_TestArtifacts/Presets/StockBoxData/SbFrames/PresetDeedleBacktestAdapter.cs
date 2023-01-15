using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Providers;
using StockBox_TestArtifacts.Builders.StockBoxData.Adapters.DataFrame;

namespace StockBox_TestArtifacts.Presets.StockBoxData.SbFrames
{
    public class PresetBacktestingDataProviderCreator
    {

        /// <summary>
        /// Return a 3-day dataset
        /// </summary>
        /// <returns></returns>
        public static BackwardTestingDataProvider ThreeDaySmaCrossOver()
        {
            return new DataProviderBuilder()
                            .WithCloseAndIndicator(null, 100, new IndicatorDataPoint("SMA(2)", 110))
                            .WithCloseAndIndicator(null, 101, new IndicatorDataPoint("SMA(2)", 100))
                            .WithCloseAndIndicator(null, 101)
                            .Build<BackwardTestingDataProvider>();
        }
    }
}
