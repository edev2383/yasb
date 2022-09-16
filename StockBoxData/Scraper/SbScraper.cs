using System;
using System.IO;
using StockBox.Associations;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;


namespace StockBox.Data.Scraper
{
    /// <summary>
    /// A mediator class that takes a Provider and Parser as arguments. Returns
    /// the payload Parser.OutType.
    ///
    /// CallContextProvider is a placeholder, still building out some ideas
    /// </summary>
    public class SbScraper : ICallContextProvider
    {
        private ISbScraperParser _parser;
        private ISbScraperProvider _provider;

        public SbScraper() { }

        public SbScraper(ISbScraperProvider provider, ISbScraperParser parser)
        {
            _provider = provider;
            _parser = parser;
        }

        public void Load(ISbScraperProvider provider, ISbScraperParser parser)
        {
            _provider = provider;
            _parser = parser;
        }

        public SbScraper Clone()
        {
            return new SbScraper(_provider, _parser);
        }

        public ScraperParserBase.OutType Scrape()
        {
            return _parser.GetPayload(_provider.GetPayload());
        }

        MemoryStream ICallContextProvider.GetDaily()
        {
            throw new NotImplementedException();
        }

        MemoryStream ICallContextProvider.GetWeekly()
        {
            throw new NotImplementedException();
        }

        MemoryStream ICallContextProvider.GetMontly()
        {
            throw new NotImplementedException();
        }
    }
}
