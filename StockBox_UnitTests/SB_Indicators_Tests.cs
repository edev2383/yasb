using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.Indicators;
using StockBox_TestArtifacts.Helpers;
using static StockBox_TestArtifacts.Helpers.EFile;
using System.Collections.Generic;
using System.Linq;
using StockBox.Associations.Enums;
using StockBox.Models;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Indicators_Tests
    {
        [TestMethod, Description("Test the indicator is created and the values are as expected based on the test dataset")]
        public void SB_Indicators_01_SimpleMovingAverage()
        {
            var expectedLastValue = 96.38;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var frame = new SbFrame(adapter, EFrequency.eDaily, new Symbol(string.Empty));
            var sma = IndicatorFactory.Create("SMA", 25);
            frame.AddIndicator(sma);

            // Asserting the Indicator initializes correctly and performs the
            // proper calculations
            Assert.AreEqual("SMA(25)", sma.Name);
            Assert.IsNotNull(sma.Payload);
            Assert.IsInstanceOfType(sma.Payload, typeof(Dictionary<DateTime, double>));

            // cast the indicator to the appropriate payload obj
            var payload = (Dictionary<DateTime, double>)sma.Payload;

            // for readability, round the last value of the payload to 2 decimal
            // places and compare against the expected
            var roundedLastValue = Math.Round(payload.Values.Last(), 2, MidpointRounding.AwayFromZero);
            Assert.AreEqual(roundedLastValue, expectedLastValue);

            // assert the indicator was correctly mapped to the SbFrame's inner
            // DataPointList object
            var firstDataPoint = frame.FirstDataPoint();
            var indicatorValue = firstDataPoint.GetByColumn(new DataColumn("SMA", 25));
            Assert.IsNotNull(indicatorValue);

            // DeedleAdapter is forward testing, i.e., DESC order - most recent
            // DateTime first, so the last value of the calculated indicator
            // payload is going to be the first value in the SbFrame's dataset
            // which is mostly irrelevant because the payload is mapped to the
            // DataPoint.Indicators via DateTime key. This is just for the test
            Assert.AreEqual(payload.Values.Last(), indicatorValue);
        }

        [TestMethod, Description("Test the indicator is created and the values are as expected based on the test dataset")]
        public void SB_Indicators_02_VolumeAverage()
        {
            var expectedLastValue = 115648348;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var frame = new SbFrame(adapter, EFrequency.eDaily, new Symbol(string.Empty));
            var sma = IndicatorFactory.Create("VOLUME", 25);
            frame.AddIndicator(sma);

            // Asserting the Indicator initializes correctly and performs the
            // proper calculations
            Assert.AreEqual("VOLUME(25)", sma.Name);
            Assert.IsNotNull(sma.Payload);
            Assert.IsInstanceOfType(sma.Payload, typeof(Dictionary<DateTime, double>));

            // cast the indicator to the appropriate payload obj
            var payload = (Dictionary<DateTime, double>)sma.Payload;

            // for readability, round the last value of the payload to 2 decimal
            // places and compare against the expected
            var roundedLastValue = Math.Round(payload.Values.Last(), 2, MidpointRounding.AwayFromZero);
            Assert.AreEqual(roundedLastValue, expectedLastValue);

            // assert the indicator was correctly mapped to the SbFrame's inner
            // DataPointList object
            var firstDataPoint = frame.FirstDataPoint();
            var indicatorValue = firstDataPoint.GetByColumn(new DataColumn("volume", 25));
            Assert.IsNotNull(indicatorValue);

            // DeedleAdapter is forward testing, i.e., DESC order - most recent
            // DateTime first, so the last value of the calculated indicator
            // payload is going to be the first value in the SbFrame's dataset
            // which is mostly irrelevant because the payload is mapped to the
            // DataPoint.Indicators via DateTime key. This is just for the test
            Assert.AreEqual(payload.Values.Last(), indicatorValue);
        }
    }
}
