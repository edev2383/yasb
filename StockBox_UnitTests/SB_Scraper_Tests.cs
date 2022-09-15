using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;
using System;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Scraper_Tests
    {
        [TestMethod]
        public void SB_Scraper_01_ScraperCanBeCreated()
        {
            var scraper = new Scraper(new CurrentYahooFinanceProvider(new CurrentYahooFinanceProvider.CurrentProvider_InType()), new HistoryYahooFinanceParser());
            Assert.IsNotNull(scraper);
        }

        [TestMethod]
        public void SB_Scraper_02_ProviderUrlInterpolationWorksAsExpected()
        {
            var cIn = new CurrentYahooFinanceProvider.CurrentProvider_InType() { Symbol = "AMD" };
            var c = new CurrentYahooFinanceProvider(cIn);
            Assert.AreEqual(c.Url, "https://finance.yahoo.com/quote/AMD/history");
        }

        [TestMethod]
        public void SB_Scraper_03_HistoryInTypeAndUrlParserWorksAsExpected()
        {
            var startDate = new DateTime(2022, 8, 1);
            var endDate = new DateTime(2022, 8, 3);
            // Monday, August 1, 2022 12:00:00 AM GMT-04:00 DST as integer
            var startDateInt = 1659326400;
            // Wednesday, August 3, 2022 11:59:59 PM GMT-04:00 DST as integer
            var endDateInt = 1659585599;

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
            Assert.AreEqual(history.Url, $"https://query1.finance.yahoo.com/v7/finance/download/{historyIn.Symbol}?period1={startDateInt}&period2={endDateInt}&interval={historyIn.Interval}&events=history&includeAdjustedClose=true");
        }

    }
}
