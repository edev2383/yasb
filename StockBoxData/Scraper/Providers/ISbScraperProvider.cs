using System;
namespace StockBox.Data.Scraper.Providers
{
    public interface ISbScraperProvider
    {
        public string Url { get; }
        object GetPayload();
    }
}
