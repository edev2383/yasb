using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using static StockBox_TestArtifacts.Helpers.EFile;
using StockBox_TestArtifacts.Helpers;
using StockBox_UnitTests.Accessors;
using StockBox_TestArtifacts.Mocks.StockBoxData.SbFrames;
using StockBox_TestArtifacts.Builders.StockBoxData.Adapters.DataFrame;
using System.Collections.Generic;
using StockBox.Data.SbFrames;
using StockBox.Models;
using StockBox.Data.Indicators;
using StockBox_TestArtifacts.Presets.StockBoxData.SbFrames;
using StockBox.Interpreter;
using StockBox.Data.SbFrames.Providers;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_DataFrameAdapter_Tests
    {
        [TestMethod, Description("Test the creation of the object")]
        public void SB_01_DeedleAdapterCanBeCreated()
        {
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());

            Assert.IsNotNull(provider);
        }

        [TestMethod, Description("Test the creation of the object")]
        public void SB_02_DeedleBacktestAdapterCanBeCreated()
        {
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var provider = new BackwardTestingDataProvider(toDplAdapter.Convert());

            Assert.IsNotNull(provider);
        }

        [TestMethod, Description("Ensure the Adapter can return the first key in the DataPointList property")]
        public void SB_03_DeedleAdapter_FirstKeyPropReturnsExpectedValue()
        {
            DateTime expected = new DateTime(2022, 6, 17, 0, 0, 0).Date;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);

            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());
            Assert.AreEqual(expected, provider.FirstKey.Date);
        }

        [TestMethod, Description("Ensure the Adapter can return the first key in the DataPointList property")]
        public void SB_04_DeedleBacktestAdapter_FirstKeyPropReturnsExpectedValue()
        {
            DateTime expected = new DateTime(2022, 6, 17, 0, 0, 0).Date;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);

            var provider = new BackwardTestingDataProvider(toDplAdapter.Convert());
            Assert.AreEqual(expected, provider.FirstKey.Date);
        }

        [TestMethod, Description("Ensure the backtest provider IterateWindow method works as intended")]
        public void SB_05_DeedleBacktestAdapter_WindowIterationWorksAsExpected()
        {

            var stream = new Reader().GetFileStream(eAmdDailySmallDataset);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);

            var provider = new BackwardTestingDataProvider_Accessor(toDplAdapter.Convert());
            provider.IterateWindow();
            var data = provider.Access_GetData();
            Assert.AreEqual(data.Count, 2);
            var first = data[0];
            Assert.AreEqual(first.Date, new DateTime(2022, 9, 20));
            var second = data[1];
            Assert.AreEqual(second.Date, new DateTime(2022, 9, 19));
        }

        [TestMethod, Description("Ensure the backtest provider IterateWindow method works as intended")]
        public void SB_06_DeedleBacktestAdapter_WindowIterationWorksAsExpected()
        {
            var stream = new Reader().GetFileStream(eAmdDailySmallDataset);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);

            var provider = new BackwardTestingDataProvider_Accessor(toDplAdapter.Convert());
            provider.IterateWindow();
            Assert.AreEqual(provider.Access_GetData().Count, 2);
            provider.IterateWindow();
            Assert.AreEqual(provider.Access_GetData().Count, 3);
            provider.IterateWindow();
            Assert.AreEqual(provider.Access_GetData().Count, 4);
        }

        [TestMethod, Description("Ensure the backtest provider IterateWindow method works as intended")]
        public void SB_06_DeedleBacktestAdapter_IsAtEndCheckWorksAsIntended()
        {
            var stream = new Reader().GetFileStream(eAmdDailySmallDataset);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);

            var provider = new BackwardTestingDataProvider_Accessor(toDplAdapter.Convert());

            // our first iteration gives us a primary dataset with a length of 2
            var expectedIndex = 2;

            // when the provider runs out of data, we'll return true and break
            // the while loop
            while (provider.IsAtEnd() != true)
            {
                provider.IterateWindow();
                var data = provider.Access_GetData();

                // test that our data length is as expected, growing by one on
                // each iteration
                Assert.AreEqual(data.Count, expectedIndex);
                // iterate the expected index before the next loop
                expectedIndex++;
            }
        }

        [TestMethod]
        public void SB_07_DeedleBackTestAdapter_IndicatorDataGetsCopiedToWindow()
        {
            var df = PresetBacktestingDataProviderCreator.ThreeDaySmaCrossOver();

            var sbFrame = new DailyFrame(df, new Symbol("AMD"));
            var sbFrameList = new SbFrameList() { sbFrame, };
            var interpreter = new SbInterpreter(sbFrameList);

            var provider = sbFrame.GetProvider();
            df.IterateWindow();
        }
    }
}
