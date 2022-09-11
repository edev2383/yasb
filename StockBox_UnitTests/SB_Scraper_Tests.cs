using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;
using System;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Scraper_Tests
    {
        [TestMethod]
        public void SB_Scraper_01_ScraperCanBeCreated()
        {
            var scraper = new Scraper(new CurrentProvider(new CurrentProvider.CurrentProvider_InType()), new HistoryParser());
            Assert.IsNotNull(scraper);
        }

        [TestMethod]
        public void SB_Scraper_02_ProviderUrlInterpolationWorksAsExpected()
        {
            var cIn = new CurrentProvider.CurrentProvider_InType() { Symbol = "AMD" };
            var c = new CurrentProvider(cIn);

            Assert.AreEqual(c.Url, "https://finance.yahoo.com/quote/AMD/history");
            //HtmlWeb web = new HtmlWeb();
            //HtmlDocument doc = web.Load(url);
            //var nodes = doc.DocumentNode.SelectNodes("//table[@data-test=\"historical-prices\"]//tbody//tr[1]//td//span//text()");
        }

        [TestMethod]
        public void SB_Scraper_03_CurrentProviderReturnsExpectedPayloadAndParserPerformsExpectedMethod()
        {
            var cIn = new CurrentProvider.CurrentProvider_InType() { Symbol = "MSFT" };
            var c = new CurrentProvider(cIn);

            var parser = new CurrentParser();
            var outpayload = parser.GetPayload(c.GetPayload());

            Assert.IsInstanceOfType(outpayload, typeof(CurrentParser.CurrentProvider_OutType));
            var castPayload = outpayload as CurrentParser.CurrentProvider_OutType;
            Assert.IsNotNull(castPayload.Date);
            Assert.IsNotNull(castPayload.High);
            Assert.IsNotNull(castPayload.Low);
            Assert.IsNotNull(castPayload.Open);
            Assert.IsNotNull(castPayload.Close);
            Assert.IsNotNull(castPayload.AdjClose);
            Assert.IsNotNull(castPayload.Volume);
        }

        [TestMethod]
        public void SB_Scraper_04_HistoryInTypeAndUrlParserWorksAsExpected()
        {
            var startDate = new DateTime(2022, 8, 1);
            var endDate = new DateTime(2022, 8, 3);
            var startDateInt = 1659326400;
            var endDateInt = 1659499200;

            var historyIn = new HistoryProvider.HistoryProvider_InType()
            {
                Symbol = "MSFT",
                StartDate = startDate,
                EndDate = endDate,
                Interval = "1d",
            };

            Assert.AreNotEqual(historyIn.EndDateInt, 0);
            Assert.AreNotEqual(historyIn.StartDateInt, 0);

            var history = new HistoryProvider(historyIn);

            var historyPayload = history.GetPayload();
            Assert.AreEqual(history.Url, $"https://query1.finance.yahoo.com/v7/finance/download/{historyIn.Symbol}?period1={startDateInt}&period2={endDateInt}&interval={historyIn.Interval}&events=history&includeAdjustedClose=true");
            Assert.IsTrue(historyPayload is string);
        }
    }
}
