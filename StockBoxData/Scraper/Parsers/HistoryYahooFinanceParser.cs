using System;
using System.IO;
using StockBox.Associations;

namespace StockBox.Data.Scraper.Parsers
{

    /// <summary>
    /// No parsing is currently needed in this particular parser, just passing
    /// the provided MemoryStream object back out as an OutType property
    /// </summary>
    public class HistoryYahooFinanceParser : ScraperParserBase
    {

        public HistoryYahooFinanceParser()
        {
        }

        public class HistoryParser_OutType : OutType, IStreamProvider
        {
            public MemoryStream Stream { get; set; }
        }

        protected override OutType GetPayload(MemoryStream stream)
        {
            return new HistoryParser_OutType()
            {
                Stream = stream,
            };
        }
    }
}
