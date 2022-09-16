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

        public HistoryYahooFinanceProvider() : base(ScraperResources.Url_YahooFinance_History, EProviderType.eMemoryStream) { }

        public class HistoryYahooFinanceProvider_InType : InType
        {
            /// <summary>
            /// Stock symbol, ex.: "MSFT"
            /// </summary>
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

            /// <summary>
            /// Convert the provided datetime into an integer epoch timestamp
            /// </summary>
            /// <param name="date"></param>
            /// <returns></returns>
            private int ConvertDateTimeToUnixEpoch(DateTime date)
            {
                DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                TimeSpan diff = date.ToUniversalTime() - origin;
                return (int)Math.Floor(diff.TotalSeconds);
            }

            /// <summary>
            /// Take an enum and map to a string from the ScraperResources
            /// </summary>
            /// <param name="interval"></param>
            /// <returns></returns>
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

        /// <summary>
        /// The provider target is a CSV download. We read it from the
        /// StreamReader and copy it over to a MemoryStream because that's
        /// what the Adapters are designed to take
        /// </summary>
        /// <returns></returns>
        public override MemoryStream LoadStream()
        {
            // make the request
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
            // set the user agent, otherwise we get an error response (503?)
            req.UserAgent = ScraperResources.UserAgent;
            var res = req.GetResponse();
            // TODO - do some error response handling here
            StreamReader sr = new StreamReader(res.GetResponseStream());

            // init the memory stream
            MemoryStream memstream = new MemoryStream();
            var buffer = new byte[512];
            var bytesRead = default(int);
            // read the response and copy the bytes to the memstream
            while ((bytesRead = sr.BaseStream.Read(buffer, 0, buffer.Length)) > 0)
                memstream.Write(buffer, 0, bytesRead);

            // reset the memstream's position
            memstream.Position = 0;
            return memstream;
        }
    }
}
