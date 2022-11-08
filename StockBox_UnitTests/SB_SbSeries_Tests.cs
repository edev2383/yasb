using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.SbFrames;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_SbSeries_Tests
    {

        [TestMethod]
        public void SB_01_SbSeries_Tests()
        {
            var series = new SbSeries();
            series.Add(DateTime.Now, 10);
            series.Add(DateTime.Now.AddDays(-1), 15);
            series.Add(DateTime.Now.AddDays(-2), 13);
            series.Add(DateTime.Now.AddDays(-3), 16);
            series.Add(DateTime.Now.AddDays(-4), 11);
            series.Add(DateTime.Now.AddDays(-5), 8);

            var result = series.Window(3, x => Average(x));
        }

        [TestMethod]
        public void SB_02_SbSeries_Tests()
        {
            var dtkey = DateTime.Now;
            var series = new SbSeries();
            series.Add(dtkey, 10);
            series.Add(dtkey.AddDays(-1), 15);
            var diff = series.Diff(1);
            Assert.AreEqual(1, diff.Count());
            Assert.AreEqual(5, diff.First().Value);
            Assert.AreEqual(dtkey.AddDays(-1), diff.First().Key);
        }

        [TestMethod]
        public void SB_03_SbSeries_Tests()
        {
            var dtkey = DateTime.Now;
            var series = new SbSeries();
            series.Add(dtkey, 21.5);
            series.Add(dtkey.AddDays(-1), 23.1);
            series.Add(dtkey.AddDays(-2), 14.8);
            var diff = series.Diff(2);
            Assert.AreEqual(1, diff.Count());
            Assert.AreEqual(-6.7, Math.Round(diff.First().Value, 1));
            Assert.AreEqual(dtkey.AddDays(-2), diff.First().Key);
        }

        public double Average(SbSeries x)
        {
            var sum = x.Values.Sum();
            var cnt = x.Count();
            return sum / cnt;
        }
    }
}
