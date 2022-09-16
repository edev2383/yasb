using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using StockBox.Data.Scraper.Providers.Helpers;


namespace StockBox.Data.Scraper.Providers
{

    public abstract class ScraperProviderBase : ISbScraperProvider
    {

        public InType In { get; set; }

        /// <summary>
        /// Url is an double-curly-bracketed string template, i.e., "x{{value}}"
        /// The FormatUrl method will replace any found matches with the InType
        /// property of the same name. 
        /// </summary>
        public string Url { get { return FormatUrl(); } }
        private readonly string _url;

        /// <summary>
        /// Explicity declare the type so we can return the proper Payload
        /// method
        /// </summary>
        private readonly EProviderType _type;

        public ScraperProviderBase(string url, InType inParams, EProviderType type) : this(url, type)
        {
            In = inParams;
        }

        public ScraperProviderBase(string url, EProviderType type)
        {
            _url = url;
            _type = type;
        }

        public ScraperProviderBase() { }

        public abstract class InType
        {
            public string this[string propertyName]
            {
                get
                {
                    Type myType = GetType();
                    PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                    return myPropInfo.GetValue(this, null).ToString();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetPayload()
        {
            switch (_type)
            {
                case EProviderType.eDocument:
                    return LoadDocument();
                case EProviderType.eMemoryStream:
                    return LoadStream();
                case EProviderType.eString:
                    return LoadText();
                default:
                    throw new Exception("Unknown EProviderType");
            }
        }

        /// <summary>
        /// Use this when doing straightforward webscraping and XPath parsing
        /// </summary>
        /// <returns></returns>
        public virtual HtmlDocument LoadDocument()
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(Url);
        }

        /// <summary>
        /// Use this when attemping to download a csv, or some other file
        /// </summary>
        /// <returns></returns>
        public virtual MemoryStream LoadStream()
        {
            return null;
        }

        /// <summary>
        /// Dont have a use-case yet for this, but seemed likely to be needed at
        /// some point
        /// </summary>
        /// <returns></returns>
        public virtual string LoadText()
        {
            return string.Empty;
        }

        /// <summary>
        /// Search the provided url for matches to the regex, and replace any
        /// found with the matching property from the InType. Common example is
        /// "https://someurl.com/{{Symbol}}". The InType will have a property
        /// "Symbol", so the Regex.Match will be Group[0] = "{{Symbol}}" and
        /// Group[1] = "Symbol". Replace {{Symbol}} with InType.Symbol
        /// </summary>
        /// <returns></returns>
        protected string FormatUrl()
        {
            var ret = _url;
            var re = "{{(.*?)}}";
            var allMatches = Regex.Matches(_url, re);
            foreach (Match m in allMatches)
            {
                var inFoundMatch = m.Groups[0].Value;
                var inPropertyName = m.Groups[1].Value;
                var propertyValue = In[inPropertyName];
                ret = ret.Replace(inFoundMatch, propertyValue);
            }
            return ret;
        }
    }
}
