using System;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;


namespace StockBox.Data.Scraper
{

    public class Scraper
    {
        private ISbScraperParser _parser;
        private ISbScraperProvider _provider;

        public Scraper(ISbScraperProvider provider, ISbScraperParser parser)
        {
            _provider = provider;
            _parser = parser;
        }
    }
}
