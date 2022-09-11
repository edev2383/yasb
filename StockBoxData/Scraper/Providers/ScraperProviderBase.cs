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

        public string Url { get { return FormatUrl(); } }
        private readonly string _url;

        private readonly EProviderType _type;

        public ScraperProviderBase(string url, InType inParams, EProviderType type)
        {
            _url = url;
            In = inParams;
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

        public virtual HtmlDocument LoadDocument()
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(Url);
        }

        public virtual MemoryStream LoadStream()
        {
            return null;
        }

        public virtual string LoadText()
        {
            return string.Empty;
        }


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
