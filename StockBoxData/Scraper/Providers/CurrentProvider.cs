using System;
using StockBox.Data.Scraper.Providers.Helpers;

namespace StockBox.Data.Scraper.Providers
{

    public class CurrentProvider : ScraperProviderBase
    {

        public CurrentProvider(CurrentProvider_InType inParams) : base("https://finance.yahoo.com/quote/{{Symbol}}/history", inParams, EProviderType.eDocument)
        {
        }

        public class CurrentProvider_InType : InType
        {
            public string Symbol { get; set; }
        }
    }
}
