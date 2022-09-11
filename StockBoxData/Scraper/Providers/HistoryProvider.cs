using System;
using System.IO;
using System.Net;
using StockBox.Data.Scraper.Providers.Helpers;

namespace StockBox.Data.Scraper.Providers
{

    public class HistoryProvider : ScraperProviderBase
    {

        //https://query1.finance.yahoo.com/v7/finance/download/AMD?period1=1631356149&period2=1662892149&interval=1d&events=history&includeAdjustedClose=true
        //https://query1.finance.yahoo.com/v7/finance/download/AMD?period1=1631374718&period2=1662910718&interval=1wk&events=history&includeAdjustedClose=true
        //https://query1.finance.yahoo.com/v7/finance/download/AMD?period1=1631374718&period2=1662910718&interval=1mo&events=history&includeAdjustedClose=true
        // {{Symbol}}
        // {{StartDateInt}}
        // {{EndDateInt}}
        // {{Interval}} == 1d, 1wk, 1mo
        // 
        public HistoryProvider(HistoryProvider_InType inParams) : base(
            "https://query1.finance.yahoo.com/v7/finance/download/{{Symbol}}?period1={{StartDateInt}}&period2={{EndDateInt}}&interval={{Interval}}&events=history&includeAdjustedClose=true",
            inParams,
            EProviderType.eString)
        {
        }

        public class HistoryProvider_InType : InType
        {
            public string Symbol { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Interval { get; set; }
            public int StartDateInt { get { return ConvertDateTimeToUnixEpoch(StartDate); } }
            public int EndDateInt { get { return ConvertDateTimeToUnixEpoch(EndDate); } }

            private int ConvertDateTimeToUnixEpoch(DateTime? date)
            {
                if (date == null) return 0;
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan diff = ((DateTime)date).ToUniversalTime() - origin;
                return (int)Math.Floor(diff.TotalSeconds);
            }
        }


        public override string LoadText()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.UserAgent = "Mozilla/5.0";
            var res = req.GetResponse();
            MemoryStream memStream;
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string results = sr.ReadToEnd();
            sr.Close();

            return results;
            //return base.LoadStream();
        }
    }
}
