using System;
using StockBox.Data.Scraper.Helpers;
using StockBox.Data.Scraper.Providers.Helpers;

namespace StockBox.Data.Scraper.Providers
{

    public class CurrentYahooFinanceProvider : ScraperProviderBase
    {

        public CurrentYahooFinanceProvider(CurrentProvider_InType inParams)
            : base(
                  ScraperResources.Url_YahooFinance_Current,
                  inParams,
                  EProviderType.eDocument)
        {
        }

        public class CurrentProvider_InType : InType
        {
            public string Symbol { get; set; }
        }
    }
}
