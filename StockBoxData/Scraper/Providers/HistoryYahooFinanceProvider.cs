using System;
using System.IO;
using System.Net;
using StockBox.Data.Scraper.Helpers;
using StockBox.Data.Scraper.Providers.Helpers;

namespace StockBox.Data.Scraper.Providers
{

    /// <summary>
    /// Download the the requested history range/frequency and return as a
    /// MemoryStream. The Adapters will convert the MemoryStream into an SbFrame
    /// </summary>
    public class HistoryYahooFinanceProvider : ScraperProviderBase
    {

        public enum EHistoryInterval
        {
            eDaily = 0,
            eWeekly,
            eMonthly
        }

        public HistoryYahooFinanceProvider(HistoryYahooFinanceProvider_InType inParams)
            : base(
                ScraperResources.Url_YahooFinance_History,
                inParams,
                EProviderType.eMemoryStream)
        {
        }

        public class HistoryYahooFinanceProvider_InType : InType
        {
            public string Symbol { get; set; }
            public DateTime StartDate { get; set; }
            /// <summary>
            /// The end date of the history request. This date WILL BE included
            /// in the result set. The additional 23:59:59 is automatically
            /// added to the provided value
            /// </summary>
            public DateTime EndDate { get; set; }
            public EHistoryInterval Interval { get; set; }
            public string IntervalStr { get { return MapInterval(Interval); } }

            public int StartDateInt { get { return ConvertDateTimeToUnixEpoch(StartDate); } }
            public int EndDateInt { get { return ConvertDateTimeToUnixEpoch(EndDate.AddHours(23).AddMinutes(59).AddSeconds(59)); } }

            private int ConvertDateTimeToUnixEpoch(DateTime? date)
            {
                if (date == null) return 0;
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan diff = ((DateTime)date).ToUniversalTime() - origin;
                return (int)Math.Floor(diff.TotalSeconds);
            }

            private string MapInterval(EHistoryInterval interval)
            {
                switch (interval)
                {
                    case EHistoryInterval.eWeekly:
                        return ScraperResources.Interval_YahooFinance_Weekly;
                    case EHistoryInterval.eMonthly:
                        return ScraperResources.Interval_YahooFinance_Monthly;
                    default:
                        return ScraperResources.Interval_YahooFinance_Daily;
                }
            }
        }

        public override MemoryStream LoadStream()
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            req.UserAgent = ScraperResources.UserAgent;
            var res = req.GetResponse();
            StreamReader sr = new StreamReader(res.GetResponseStream());
            MemoryStream memstream = new MemoryStream();
            var buffer = new byte[512];
            var bytesRead = default(int);
            while ((bytesRead = sr.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                memstream.Write(buffer, 0, bytesRead);
            memstream.Position = 0;
            return memstream;
        }
    }
}
