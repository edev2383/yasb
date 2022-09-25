using System;
using HtmlAgilityPack;
using StockBox.Data.Scraper.Helpers;

namespace StockBox.Data.Scraper.Parsers
{

    /// <summary>
    /// Scrape the current data from the provided dataset. The "Current" url
    /// points to histoy stock data, and the Parser XPath points to the first
    /// row of the found table 
    /// </summary>
    public class CurrentYahooFinanceParser : ScraperParserBase
    {

        /// <summary>
        /// An enum declaring expected order of indexed information from the
        /// CurrentProvider payload
        /// </summary>
        protected enum ECurrentKeyOrder
        {
            eDate = 0,
            eOpen,
            eHigh,
            eLow,
            eClose,
            eAdjClose,
            eVolume
        };

        public CurrentYahooFinanceParser() : base(ScraperResources.XPath_YahooFinance_Current)
        {
        }

        public class CurrentProvider_OutType : OutType
        {
            public DateTime Date;
            public double High;
            public double Low;
            public double Open;
            public double Close;
            public double AdjClose;
            public double Volume;
        }

        protected override OutType GetPayload(HtmlDocument document)
        {
            var ret = new CurrentProvider_OutType();
            var nodes = document.DocumentNode.SelectNodes(XPath);
            if (nodes.Count > 0)
            {
                ret.Date = DateTime.Parse(nodes[(int)ECurrentKeyOrder.eDate].InnerText);
                ret.High = double.Parse(nodes[(int)ECurrentKeyOrder.eHigh].InnerText);
                ret.Low = double.Parse(nodes[(int)ECurrentKeyOrder.eLow].InnerText);
                ret.Open = double.Parse(nodes[(int)ECurrentKeyOrder.eOpen].InnerText); ;
                ret.Close = double.Parse(nodes[(int)ECurrentKeyOrder.eClose].InnerText);
                ret.AdjClose = double.Parse(nodes[(int)ECurrentKeyOrder.eAdjClose].InnerText);
                ret.Volume = double.Parse(nodes[(int)ECurrentKeyOrder.eVolume].InnerText);
            }
            return ret;
        }
    }
}
