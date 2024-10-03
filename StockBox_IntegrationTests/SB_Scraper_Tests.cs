using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Associations.Enums;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames.Providers;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Helpers;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;
using StockBox.Models;
using System;
using System.Threading.Tasks;


namespace StockBox_IntegrationTests
{
    [TestClass]
    public class SB_Scraper_Tests
    {


        [TestMethod]
        public void SB_Scraper_03_HistoryInTypeAndUrlParserWorksAsExpected()
        {
            var freq = ScraperResources.I().AlphaVantage.FrequencyDaily;
            var apiKey = ScraperResources.I().AlphaVantage.ApiKey;
            var symbol = "MSFT";
            var expected = $"https://www.alphavantage.co/query?function={freq}&symbol={symbol}&apikey={apiKey}&outputsize=compact&datatype=json";
            var historyIn = new AlphaVantageSecurityHistoryProvider.AlphaVantageSecurityHistoryProvider_InType(
                symbol: symbol,
                frequency: EFrequency.Daily);

            var history = new AlphaVantageSecurityHistoryProvider(historyIn);

            Assert.AreEqual(expected, history.Url);
        }

        [TestMethod]
        public async Task SB_Scraper_04_HistoryScraperIntegrationWorksAsExpected()
        {
            var symbol = "MSFT";
            var historyIn = new AlphaVantageSecurityHistoryProvider.AlphaVantageSecurityHistoryProvider_InType(
                symbol: symbol,
                frequency: EFrequency.Daily);

            var scraper = new SbScraper(new AlphaVantageSecurityHistoryProvider(historyIn), new AlphaVantageSecurityHistoryParser());
            var payload = await scraper.Scrape() as AlphaVantageSecurityHistoryParser.HistoryParser_OutType;

            Assert.IsNotNull(payload);
            Assert.IsNotNull(payload.Data);
        }

        [TestMethod]
        public void SB_Scraper_06_HistoryScraperIntegrationWorksWithForex()
        {
            // AlphaVantage supports forex, but needs a different URL/InType
            Assert.Inconclusive();
        }
    }
}
