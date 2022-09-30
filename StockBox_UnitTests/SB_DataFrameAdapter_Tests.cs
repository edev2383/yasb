using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using static StockBox_UnitTests.Helpers.EFile;
using StockBox_UnitTests.Helpers;
using StockBox_UnitTests.Accessors;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_DataFrameAdapter_Tests
    {
        [TestMethod, Description("Test the creation of the object")]
        public void SB_01_DeedleAdapterCanBeCreated()
        {
            var stream = new Reader().GetFileStream(eAmdDaily);
            var adapter = new DeedleAdapter(stream);

            Assert.IsNotNull(adapter);
        }

        [TestMethod, Description("Test the creation of the object")]
        public void SB_02_DeedleBacktestAdapterCanBeCreated()
        {
            var stream = new Reader().GetFileStream(eAmdDaily);
            var adapter = new DeedleBacktestAdapter(stream);

            Assert.IsNotNull(adapter);
        }

        [TestMethod, Description("Ensure the Adapter can return the first key in the DataPointList property")]
        public void SB_03_DeedleAdapter_FirstKeyPropReturnsExpectedValue()
        {
            DateTime expected = new DateTime(2022, 6, 17, 0, 0, 0).Date;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var adapter = new DeedleAdapter(stream);

            Assert.AreEqual(expected, adapter.FirstKey.Date);
        }

        [TestMethod, Description("Ensure the Adapter can return the first key in the DataPointList property")]
        public void SB_04_DeedleBacktestAdapter_FirstKeyPropReturnsExpectedValue()
        {
            DateTime expected = new DateTime(2022, 6, 17, 0, 0, 0).Date;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var adapter = new DeedleBacktestAdapter(stream);
            Assert.AreEqual(expected, adapter.FirstKey.Date);
        }

        [TestMethod, Description("Ensure the backtest adapter IterateWindow method works as intended")]
        public void SB_05_DeedleBacktestAdapter_WindowIterationWorksAsExpected()
        {

            var stream = new Reader().GetFileStream(eAmdDailySmallDataset);
            var adapter = new DeedleBacktestAdapter_Accessor(stream);
            adapter.IterateWindow();
            var data = adapter.Access_GetData();
            Assert.AreEqual(data.Count, 2);
            var first = data[0];
            Assert.AreEqual(first.Date, new DateTime(2022, 9, 20));
            var second = data[1];
            Assert.AreEqual(second.Date, new DateTime(2022, 9, 19));
        }

        [TestMethod, Description("Ensure the backtest adapter IterateWindow method works as intended")]
        public void SB_06_DeedleBacktestAdapter_WindowIterationWorksAsExpected()
        {
            var stream = new Reader().GetFileStream(eAmdDailySmallDataset);
            var adapter = new DeedleBacktestAdapter_Accessor(stream);
            adapter.IterateWindow();
            Assert.AreEqual(adapter.Access_GetData().Count, 2);
            adapter.IterateWindow();
            Assert.AreEqual(adapter.Access_GetData().Count, 3);
            adapter.IterateWindow();
            Assert.AreEqual(adapter.Access_GetData().Count, 4);
        }

        [TestMethod, Description("Ensure the backtest adapter IterateWindow method works as intended")]
        public void SB_06_DeedleBacktestAdapter_IsAtEndCheckWorksAsIntended()
        {
            var stream = new Reader().GetFileStream(eAmdDailySmallDataset);
            var adapter = new DeedleBacktestAdapter_Accessor(stream);

            // our first iteration gives us a primary dataset with a length of 2
            var expectedIndex = 2;

            // when the adapter runs out of data, we'll return true and break
            // the while loop
            while (adapter.IsAtEnd() != true)
            {
                adapter.IterateWindow();
                var data = adapter.Access_GetData();

                // test that our data length is as expected, growing by one on
                // each iteration
                Assert.AreEqual(data.Count, expectedIndex);
                // iterate the expected index before the next loop
                expectedIndex++;
            }
        }
    }
}
