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
using StockBox.Data.SbFrames.Providers;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Indicators_Tests
    {
        [TestMethod, Description("Test the indicator `SimpleMovingAverage` is created and the values are as expected based on the test dataset")]
        public void SB_Indicators_01_SimpleMovingAverage()
        {
            var expectedLastValue = 96.38;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());
            var frame = new SbFrame(provider, EFrequency.eDaily, new Symbol(string.Empty));
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

        [TestMethod, Description("Test the indicator `Volume` is created and the values are as expected based on the test dataset")]
        public void SB_Indicators_02_VolumeAverage()
        {
            var expectedLastValue = 115648348;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());
            var frame = new SbFrame(provider, EFrequency.eDaily, new Symbol(string.Empty));
            var sma = IndicatorFactory.Create("AVGVolume", 25);
            frame.AddIndicator(sma);

            // Asserting the Indicator initializes correctly and performs the
            // proper calculations
            Assert.AreEqual("AVGVolume(25)", sma.Name);
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
            var indicatorValue = firstDataPoint.GetByColumn(new DataColumn("AVGVolume", 25));
            Assert.IsNotNull(indicatorValue);

            // DeedleAdapter is forward testing, i.e., DESC order - most recent
            // DateTime first, so the last value of the calculated indicator
            // payload is going to be the first value in the SbFrame's dataset
            // which is mostly irrelevant because the payload is mapped to the
            // DataPoint.Indicators via DateTime key. This is just for the test
            Assert.AreEqual(payload.Values.Last(), indicatorValue);
        }

        [TestMethod, Description("Test the indicator `RelativeStrengthIndex` is created and the values are as expected based on the test dataset")]
        public void SB_Indicators_03_RelativeStrengthIndex()
        {
            var expectedLastValue = 34.47;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());
            var frame = new SbFrame(provider, EFrequency.eDaily, new Symbol(string.Empty));
            var sma = IndicatorFactory.Create("RSI", 14);
            frame.AddIndicator(sma);

            // Asserting the Indicator initializes correctly and performs the
            // proper calculations
            Assert.AreEqual("RSI(14)", sma.Name);
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
            var indicatorValue = firstDataPoint.GetByColumn(new DataColumn("rsi", 14));
            Assert.IsNotNull(indicatorValue);

            // DeedleAdapter is forward testing, i.e., DESC order - most recent
            // DateTime first, so the last value of the calculated indicator
            // payload is going to be the first value in the SbFrame's dataset
            // which is mostly irrelevant because the payload is mapped to the
            // DataPoint.Indicators via DateTime key. This is just for the test
            Assert.AreEqual(payload.Values.Last(), indicatorValue);

        }

        [TestMethod, Description("Test the indicator `FastStochastics` is created and the values are as expected based on the test dataset")]
        public void SB_Indicators_04_FastStochastics()
        {
            var expectedFirstKValue = 57.82;
            var expectedFirstDValue = 62.59;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());
            var frame = new SbFrame(provider, EFrequency.eDaily, new Symbol(string.Empty));
            var fastSto = IndicatorFactory.Create("FastSto", 14, 3);
            frame.AddIndicator(fastSto);

            // Asserting the Indicator initializes correctly and performs the
            // proper calculations
            Assert.AreEqual("FastSto(14,3)", fastSto.Name);

            var firstPayloadValue = ((Dictionary<DateTime, (double k, double d)>)fastSto.Payload).First().Value;
            Assert.AreEqual(expectedFirstKValue, Math.Round(firstPayloadValue.k, 2, MidpointRounding.AwayFromZero));
            Assert.AreEqual(expectedFirstDValue, Math.Round(firstPayloadValue.d, 2, MidpointRounding.AwayFromZero));
            Assert.IsNotNull(fastSto.Payload);
            Assert.IsInstanceOfType(fastSto.Payload, typeof(Dictionary<DateTime, (double k, double d)>));

            // cast the indicator to the appropriate payload obj
            var payload = (Dictionary<DateTime, (double k, double d)>)fastSto.Payload;

            // assert the indicator was correctly mapped to the SbFrame's inner
            // DataPointList object
            var firstDataPoint = frame.FirstDataPoint();
            var indicatorValue = firstDataPoint.GetByColumn(new DataColumn("FastSto", 14, 3));
            Assert.IsNotNull(indicatorValue);

            // DeedleAdapter is forward testing, i.e., DESC order - most recent
            // DateTime first, so the last value of the calculated indicator
            // payload is going to be the first value in the SbFrame's dataset
            // which is mostly irrelevant because the payload is mapped to the
            // DataPoint.Indicators via DateTime key. This is just for the test
            Assert.AreEqual(payload.Values.Last().k, indicatorValue);
        }

        [TestMethod, Description("Test the indicator `SlowStochastic` is created and the values are as expected based on the test dataset")]
        public void SB_Indicators_05_SlowStochastics()
        {
            var expectedFirstKValue = 39.26;
            var expectedFirstDValue = 52.22;
            var stream = new Reader().GetFileStream(eAmdDaily);
            var toDplAdapter = new DeedleToDataPointListAdapter(stream);
            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());
            var frame = new SbFrame(provider, EFrequency.eDaily, new Symbol(string.Empty));
            var slowSto = IndicatorFactory.Create("SlowSto", 14, 3);
            frame.AddIndicator(slowSto);

            // Asserting the Indicator initializes correctly and performs the
            // proper calculations
            Assert.AreEqual("SlowSto(14,3)", slowSto.Name);

            var firstPayloadValue = ((Dictionary<DateTime, (double k, double d)>)slowSto.Payload).First().Value;
            Assert.AreEqual(expectedFirstKValue, Math.Round(firstPayloadValue.k, 2, MidpointRounding.AwayFromZero));
            Assert.AreEqual(expectedFirstDValue, Math.Round(firstPayloadValue.d, 2, MidpointRounding.AwayFromZero));
            Assert.IsNotNull(slowSto.Payload);
            Assert.IsInstanceOfType(slowSto.Payload, typeof(Dictionary<DateTime, (double k, double d)>));

            // cast the indicator to the appropriate payload obj
            var payload = (Dictionary<DateTime, (double k, double d)>)slowSto.Payload;

            // assert the indicator was correctly mapped to the SbFrame's inner
            // DataPointList object
            var firstDataPoint = frame.FirstDataPoint();
            var indicatorValue = firstDataPoint.GetByColumn(new DataColumn("SlowSto", 14, 3));
            Assert.IsNotNull(indicatorValue);

            // DeedleAdapter is forward testing, i.e., DESC order - most recent
            // DateTime first, so the last value of the calculated indicator
            // payload is going to be the first value in the SbFrame's dataset
            // which is mostly irrelevant because the payload is mapped to the
            // DataPoint.Indicators via DateTime key. This is just for the test
            Assert.AreEqual(payload.Values.Last().k, indicatorValue);
        }
    }
}