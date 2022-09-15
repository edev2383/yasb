using System;
using System.IO;
using HtmlAgilityPack;


namespace StockBox.Data.Scraper.Parsers
{

    /// <summary>
    /// A parser will receive a payload from a related Provider and will turn
    /// the payload into a class-specific OutType. In the case of general web-
    /// scraping, this will be completed via XPath, ex. CurrentParser.
    /// </summary>
    public abstract class ScraperParserBase : ISbScraperParser
    {
        public string XPath { get { return _xpath; } }
        private readonly string _xpath;

        public ScraperParserBase(string xpath)
        {
            _xpath = xpath;
        }

        public abstract class OutType { }

        public ScraperParserBase()
        {
        }

        /// <summary>
        /// Return the proper payload method, based on the object received from
        /// the Provider. This could allow one parser to break down data from
        /// different sources as needed
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public OutType GetPayload(object obj)
        {
            if (obj is HtmlDocument)
                return GetPayload(obj as HtmlDocument);
            if (obj is MemoryStream)
                return GetPayload(obj as MemoryStream);
            if (obj is string)
                return GetPayload(obj as string);
            throw new Exception("Provided object is of unsupported type");
        }

        /// <summary>
        /// Create the appropriate outtype based on a provided HtmlDocument
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        protected virtual OutType GetPayload(HtmlDocument document) { return null; }

        /// <summary>
        /// Create the appropriate outtype based on a provided MemoryStream obj
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        protected virtual OutType GetPayload(MemoryStream stream) { return null; }

        /// <summary>
        /// Create the appropriate outtype based on a provided string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual OutType GetPayload(string text) { return null; }
    }
}
