using System;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;


namespace StockBox.Data.Scraper
{
    /// <summary>
    /// A mediator class that takes a Provider and Parser as arguments. Returns
    /// the payload Parser.OutType
    /// </summary>
    public class Scraper
    {
        private readonly ISbScraperParser _parser;
        private readonly ISbScraperProvider _provider;

        public Scraper(ISbScraperProvider provider, ISbScraperParser parser)
        {
            _provider = provider;
            _parser = parser;
        }

        public ScraperParserBase.OutType Scrape()
        {
            return _parser.GetPayload(_provider.GetPayload());
        }
    }
}
