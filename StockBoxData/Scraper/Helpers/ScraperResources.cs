using System;


namespace StockBox.Data.Scraper.Helpers
{

    public class ScraperResources
    {
        public static string Url_YahooFinance_History = "https://query1.finance.yahoo.com/v7/finance/download/{{Symbol}}?period1={{StartDateInt}}&period2={{EndDateInt}}&interval={{IntervalStr}}&events=history&includeAdjustedClose=true";
        public static string Url_YahooFinance_Current = "https://finance.yahoo.com/quote/{{Symbol}}/history";

        public static string XPath_YahooFinance_Current = "//table[@data-test=\"historical-prices\"]//tbody//tr[1]//td//span//text()";

        public static string Interval_YahooFinance_Daily = "1d";
        public static string Interval_YahooFinance_Weekly = "1wk";
        public static string Interval_YahooFinance_Monthly = "1mo";

        public static string UserAgent = "Mozilla/5.0";

        public static string UrlFormRegexPattern = "{{(.*?)}}";
    }
}
