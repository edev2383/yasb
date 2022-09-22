using System;
using StockBox.Associations;
using StockBox.Associations.Enums;
using StockBox.Data.SbFrames;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;
using static StockBox.Data.Scraper.Providers.HistoryYahooFinanceProvider;


namespace StockBox.Data.Context
{

    /// <summary>
    /// Create and return an IStreamProvider object. Currently, it's just the
    /// one parser, but this will expand as we add some redundancies.
    /// </summary>
    public class StreamFactory
    {
        public static IStreamProvider Create(string symbol, EFrequency frequency, DateTime startDate, DateTime? endDate = null)
        {
            var inType = new HistoryYahooFinanceProvider_InType()
            {
                Symbol = symbol,
                Interval = frequency,
                EndDate = endDate != null ? (DateTime)endDate : DateTimeFrameHelper.GetOrigin(),
                StartDate = startDate,
            };

            var scraperProvider = new HistoryYahooFinanceProvider(inType);
            var scraperParser = new HistoryYahooFinanceParser();
            var scraper = new SbScraper(scraperProvider, scraperParser);
            return scraper.Scrape() as HistoryYahooFinanceParser.HistoryParser_OutType;
        }
    }
}
