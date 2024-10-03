using System.Threading.Tasks;
using static StockBox.Data.Scraper.Parsers.ScraperParserBase;

namespace StockBox.Data.Scraper.Parsers
{

    public interface ISbScraperParser
    {

        SbScraper Parent { get; set; }

        OutType GetPayload(object obj);
    }
}
