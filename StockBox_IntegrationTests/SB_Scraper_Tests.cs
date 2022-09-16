using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;
using System;
using System.IO;


namespace StockBox_IntegrationTests
{
    [TestClass]
    public class SB_Scraper_Tests
    {

        [TestMethod]
        public void SB_Scraper_01_CurrentProviderReturnsExpectedPayloadAndParserPerformsExpectedMethod()
        {
            var cIn = new CurrentYahooFinanceProvider.CurrentProvider_InType() { Symbol = "MSFT" };
            var c = new CurrentYahooFinanceProvider(cIn);

            var parser = new CurrentYahooFinanceParser();
            var outpayload = parser.GetPayload(c.GetPayload());

            Assert.IsInstanceOfType(outpayload, typeof(CurrentYahooFinanceParser.CurrentProvider_OutType));
            var castPayload = outpayload as CurrentYahooFinanceParser.CurrentProvider_OutType;
            Assert.IsNotNull(castPayload.Date);
            Assert.IsNotNull(castPayload.High);
            Assert.IsNotNull(castPayload.Low);
            Assert.IsNotNull(castPayload.Open);
            Assert.IsNotNull(castPayload.Close);
            Assert.IsNotNull(castPayload.AdjClose);
            Assert.IsNotNull(castPayload.Volume);
        }

        [TestMethod]
        public void SB_Scraper_02_ScraperClassActsAsSuccessfulMediator()
        {
            var currentInParam = new CurrentYahooFinanceProvider.CurrentProvider_InType() { Symbol = "TSLA", };
            var scraper = new SbScraper(new CurrentYahooFinanceProvider(currentInParam), new CurrentYahooFinanceParser());
            var payload = scraper.Scrape() as CurrentYahooFinanceParser.CurrentProvider_OutType;
            Assert.IsNotNull(payload);
            Assert.IsNotNull(payload.Date);
            Assert.IsNotNull(payload.High);
            Assert.IsNotNull(payload.Low);
            Assert.IsNotNull(payload.Open);
            Assert.IsNotNull(payload.Close);
            Assert.IsNotNull(payload.AdjClose);
            Assert.IsNotNull(payload.Volume);
        }

        [TestMethod]
        public void SB_Scraper_03_HistoryInTypeAndUrlParserWorksAsExpected()
        {
            var startDate = new DateTime(2022, 8, 1);
            var endDate = new DateTime(2022, 8, 3);

            var historyIn = new HistoryYahooFinanceProvider.HistoryYahooFinanceProvider_InType()
            {
                Symbol = "MSFT",
                StartDate = startDate,
                EndDate = endDate,
                Interval = HistoryYahooFinanceProvider.EHistoryInterval.eDaily,
            };

            Assert.AreNotEqual(historyIn.EndDateInt, 0);
            Assert.AreNotEqual(historyIn.StartDateInt, 0);

            var history = new HistoryYahooFinanceProvider(historyIn);

            var historyPayload = history.GetPayload();
            Assert.IsTrue(historyPayload is MemoryStream);
        }

        [TestMethod]
        public void SB_Scraper_04_HistoryScraperIntegrationWorksAsExpected()
        {
            var startDate = new DateTime(2022, 8, 1);
            var endDate = new DateTime(2022, 8, 3);

            var historyIn = new HistoryYahooFinanceProvider.HistoryYahooFinanceProvider_InType()
            {
                Symbol = "MSFT",
                StartDate = startDate,
                EndDate = endDate,
                Interval = HistoryYahooFinanceProvider.EHistoryInterval.eDaily,
            };

            var scraper = new SbScraper(new HistoryYahooFinanceProvider(historyIn), new HistoryYahooFinanceParser());
            var payload = scraper.Scrape() as HistoryYahooFinanceParser.HistoryParser_OutType;

            Assert.IsNotNull(payload);
            Assert.IsNotNull(payload.Stream);
        }

        [TestMethod]
        public void SB_Scraper_05_HistoryScraperIntegrationWithDataFrame()
        {
            var startDate = new DateTime(2022, 8, 1);
            var endDate = new DateTime(2022, 8, 3);

            var historyIn = new HistoryYahooFinanceProvider.HistoryYahooFinanceProvider_InType()
            {
                Symbol = "MSFT",
                StartDate = startDate,
                EndDate = endDate,
                Interval = HistoryYahooFinanceProvider.EHistoryInterval.eDaily,
            };

            var scraper = new SbScraper(new HistoryYahooFinanceProvider(historyIn), new HistoryYahooFinanceParser());
            var payload = scraper.Scrape() as HistoryYahooFinanceParser.HistoryParser_OutType;
            Assert.IsNotNull(payload);
            Assert.IsNotNull(payload.Stream);

            var adapter = new DeedleAdapter(payload.Stream);

            Assert.IsNotNull(adapter);
            // Dataset is two days
            Assert.AreEqual(adapter.Length, 3);
        }
    }
}
