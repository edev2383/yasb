using static StockBox.Data.Scraper.Parsers.ScraperParserBase;

namespace StockBox.Data.Scraper.Parsers
{

    public interface ISbScraperParser
    {
        public string XPath { get; }

        OutType GetPayload(object obj);
    }
}
