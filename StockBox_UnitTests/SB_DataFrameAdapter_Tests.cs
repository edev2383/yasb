using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using static StockBox_UnitTests.Helpers.EFile;
using StockBox_UnitTests.Helpers;


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

            Assert.AreEqual(adapter.FirstKey.Date, expected);
        }

        [TestMethod, Description("Ensure the Adapter can return the first key in the DataPointList property")]
        public void SB_04_DeedleBacktestAdapter_FirstKeyPropReturnsExpectedValue()
        {
            DateTime expected = new DateTime(2021, 6, 21, 0, 0, 0).Date;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var adapter = new DeedleBacktestAdapter(stream);

            Assert.AreEqual(adapter.FirstKey.Date, expected);
        }
    }
}
