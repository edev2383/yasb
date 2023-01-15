using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using static StockBox_TestArtifacts.Helpers.EFile;
using StockBox_UnitTests.Accessors;
using StockBox.Data.SbFrames;
using StockBox_TestArtifacts.Helpers;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_DataPointList_Tests
    {

        [TestMethod]
        public void SB_01_DataPointList_Tests()
        {
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var adapter = new DataProvider_Accessor(toDplAdapter.Convert());

            var data = adapter.Access_GetData();

            var series = data.ToSeries("Close");

            Assert.IsNotNull(series);
            Assert.IsTrue(series.Count > 0);
        }
    }
}
