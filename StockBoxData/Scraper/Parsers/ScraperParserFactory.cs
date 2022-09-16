using System;
using StockBox.Data.Scraper.Helpers;

namespace StockBox.Data.Scraper.Parsers
{
    public static class ScraperParserFactory
    {
        public static ScraperParserBase CreateParser(EScraperType type)
        {
            switch (type)
            {
                case EScraperType.eHistoryYahooFinance:
                    return new HistoryYahooFinanceParser();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
