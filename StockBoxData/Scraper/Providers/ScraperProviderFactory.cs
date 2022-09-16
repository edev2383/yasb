using System;
using StockBox.Data.Scraper.Helpers;

namespace StockBox.Data.Scraper.Providers
{
    public static class ScraperProviderFactory
    {
        public static ScraperProviderBase CreateProvider(EScraperType type)
        {
            switch (type)
            {
                case EScraperType.eHistoryYahooFinance:
                    return new HistoryYahooFinanceProvider();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
