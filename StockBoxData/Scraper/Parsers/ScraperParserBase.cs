using System;
using System.IO;
using HtmlAgilityPack;


namespace StockBox.Data.Scraper.Parsers
{

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

        protected virtual OutType GetPayload(HtmlDocument document) { return null; }
        protected virtual OutType GetPayload(MemoryStream stream) { return null; }
        protected virtual OutType GetPayload(string text) { return null; }
    }
}
