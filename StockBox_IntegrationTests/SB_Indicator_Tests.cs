using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Associations.Enums;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;
using StockBox.Data.SbFrames.Providers;
using StockBox.Data.SbFrames;
using StockBox.Data.Scraper.Providers;
using StockBox.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox_IntegrationTests
{
    [TestClass]
    public class SB_Indicator_Tests
    {

        [TestMethod]
        public void SB_Indicator_01_Tests()
        {
            /// Using Stockcharts.com's data as a model, this test is to make sure 
            /// that we're calculating the ATR properly on scraped data. To get this 
            /// value, go to stockcharts.com and load the chart for "AMD". Select the
            /// date range and add the ATR indicator to the chart. The far right will
            /// give you the value of the ATR for the last date in the range. 
            var expectedLastValue = 5.668;

            /// SC.com says they use 250 days to make sure their ATR values are accurate.
            /// We'll go ahead and grab two years (500+ datapoints) to be *extra* accurate.
            var startDate = new DateTime(2020, 6, 17);
            var endDate = new DateTime(2022, 6, 17);

            var historyIn = new HistoryYahooFinanceProvider.HistoryYahooFinanceProvider_InType()
            {
                Symbol = "AMD",
                StartDate = startDate,
                EndDate = endDate,
                Interval = EFrequency.eDaily,
            };

            Assert.AreNotEqual(historyIn.EndDateInt, 0);
            Assert.AreNotEqual(historyIn.StartDateInt, 0);

            var history = new HistoryYahooFinanceProvider(historyIn);

            var historyPayload = history.GetPayload();

            var toDplAdapter = new DeedleToDataPointListAdapter(historyPayload as MemoryStream);
            var provider = new ForwardTestingDataProvider(toDplAdapter.Convert());
            var frame = new SbFrame(provider, EFrequency.eDaily, new Symbol(string.Empty));
            var atr = IndicatorFactory.Create("ATR", 14) as AverageTrueRange;
            frame.AddIndicator(atr);

            // Asserting the Indicator initializes correctly and performs the
            // proper calculations
            Assert.AreEqual("ATR(14)", atr.Name);
            Assert.IsNotNull(atr.Payload);
            Assert.IsInstanceOfType(atr.Payload, typeof(Dictionary<DateTime, double>));

            // cast the indicator to the appropriate payload obj
            var payload = (Dictionary<DateTime, double>)atr.Payload;

            // for readability, round the last value of the payload to 3 decimal
            // places and compare against the expected
            var roundedLastValue = Math.Round(payload.Values.Last(), 3, MidpointRounding.AwayFromZero);
            Assert.AreEqual(roundedLastValue, expectedLastValue);
        }
    }
}
