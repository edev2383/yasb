using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.SbFrames;


namespace StockBox_UnitTests
{
    [TestClass]
    public class SB_DataColumn_Tests
    {

        [TestMethod]
        public void SB_01_DataColumn_Tests()
        {
            var descriptor = "Close";
            var result = DataColumn.ParseColumnDescriptor(descriptor);

            Assert.AreEqual(result.EColumn, DataColumn.EColumns.eClose);
        }

        [TestMethod]
        public void SB_02_DataColumn_Tests()
        {
            var descriptor = "SlowSto(14,3)";
            var result = DataColumn.ParseColumnDescriptor(descriptor);

            Assert.AreEqual(result.EColumn, DataColumn.EColumns.eSloSto);
            Assert.AreEqual(result.Indices[0], 14);
            Assert.AreEqual(result.Indices[1], 3);
        }

        [TestMethod]
        public void SB_03_DataColumn_Tests()
        {
            var descriptor = "SlowSto( 14 , 3 )";
            var result = DataColumn.ParseColumnDescriptor(descriptor);

            Assert.AreEqual(result.EColumn, DataColumn.EColumns.eSloSto);
            Assert.AreEqual(result.Indices[0], 14);
            Assert.AreEqual(result.Indices[1], 3);
        }
    }
}
