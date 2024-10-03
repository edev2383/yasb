using System;
using System.Threading.Tasks;
namespace StockBox.Data.Scraper.Providers
{
    public interface ISbScraperProvider
    {
        SbScraper Parent { get; set; }
        string Url { get; }
        Task<object> GetPayload();
    }
}
