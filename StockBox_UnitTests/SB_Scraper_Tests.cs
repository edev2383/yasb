using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Associations.Enums;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;
using StockBox_TestArtifacts.Helpers;
using System;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Scraper_Tests
    {
        [TestMethod]
        public void SB_Scraper_01_ScraperCanBeCreated()
        {
            var scraper = new SbScraper(new AlphaVantageSecurityHistoryProvider(new AlphaVantageSecurityHistoryProvider.AlphaVantageSecurityHistoryProvider_InType()), new AlphaVantageSecurityHistoryParser());
            Assert.IsNotNull(scraper);
        }

        [TestMethod]
        public void SB_Scraper_02_HistoryParserCreatesProperPayloadObjectFromStaticFilePayload_Daily()
        {
            var fileContents = new Reader().GetFileContents(EFile.AlphaVantageDailyTxt);
            var parser = new AlphaVantageSecurityHistoryParser();
            var payload = parser.GetPayload(fileContents) as AlphaVantageSecurityHistoryParser.HistoryParser_OutType;
            Assert.IsNotNull(payload);
            Assert.IsNotNull(payload.Data);
            Assert.IsTrue(payload.Data.Count > 0);
        }

        [TestMethod]
        public void SB_Scraper_02_HistoryParserCreatesProperPayloadObjectFromStaticFilePayload_Weekly()
        {
            var fileContents = new Reader().GetFileContents(EFile.AlphaVantageWeeklyTxt);
            var parser = new AlphaVantageSecurityHistoryParser();
            var payload = parser.GetPayload(fileContents) as AlphaVantageSecurityHistoryParser.HistoryParser_OutType;
            Assert.IsNotNull(payload);
            Assert.IsNotNull(payload.Data);
            Assert.IsTrue(payload.Data.Count > 0);
        }

        [TestMethod]
        public void SB_Scraper_02_HistoryParserCreatesProperPayloadObjectFromStaticFilePayload_Monthly()
        {
            var fileContents = new Reader().GetFileContents(EFile.AlphaVantageMonthlyTxt);
            var parser = new AlphaVantageSecurityHistoryParser();
            var payload = parser.GetPayload(fileContents) as AlphaVantageSecurityHistoryParser.HistoryParser_OutType;
            Assert.IsNotNull(payload);
            Assert.IsNotNull(payload.Data);
            Assert.IsTrue(payload.Data.Count > 0);
        }

    }
}
