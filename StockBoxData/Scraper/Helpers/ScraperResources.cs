using System;


namespace StockBox.Data.Scraper.Helpers
{

    public class ScraperResources
    {
        public static readonly string Url_YahooFinance_History = "https://query1.finance.yahoo.com/v7/finance/download/{{Symbol}}?period1={{StartDateInt}}&period2={{EndDateInt}}&interval={{IntervalStr}}&events=history&includeAdjustedClose=true";
        public static readonly string Url_YahooFinance_Current = "https://finance.yahoo.com/quote/{{Symbol}}/history";

        public static readonly string Url_Nasdaq_History = "https://api.nasdaq.com/api/quote/{{Symbol}}/historical?assetclass=stocks&fromdate={{StartDate}}&limit=9999&todate={{EndDate}}&random=78";
        public static readonly string XPath_YahooFinance_Current = "//table[@data-test=\"historical-prices\"]//tbody//tr[1]//td//span//text()";

        public static readonly string Interval_YahooFinance_Daily = "1d";
        public static readonly string Interval_YahooFinance_Weekly = "1wk";
        public static readonly string Interval_YahooFinance_Monthly = "1mo";

        public static readonly string UserAgent = "Mozilla/5.0";

        public static readonly string UrlFormRegexPattern = "{{(.*?)}}";

        public AlphaVantageApiResources AlphaVantage = new AlphaVantageApiResources();

        public static ScraperResources I()
        {
           return new ScraperResources();
        }
    }
}
