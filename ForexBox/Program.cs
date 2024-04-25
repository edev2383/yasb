
namespace ForexBox;

class Program
{
    static void Main(string[] args)
    {
        // import 
        var symbols = new List<string> { "EURUSD=X", "USDJPY=X", "AUDUSD=X", "EURJPY=X", "GBPUSD=X", "USDCAD=X", "USDCHF=X", "EURGBP=X", };
        var today = DateTime.Now;
        var startDate = today.AddDays(-200);

        var scraper = new SbScraper();
    }
}
