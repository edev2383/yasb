using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using StockBox.Data.Scraper.Helpers;
using StockBox.Data.Scraper.Providers.Helpers;


namespace StockBox.Data.Scraper.Providers
{

    public abstract class ScraperProviderBase : ISbScraperProvider
    {

        public SbScraper Parent { get; set; }
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
        public async Task<object> GetPayload()
        {
            switch (_type)
            {
                case EProviderType.String:
                case EProviderType.Json:
                    return await LoadTextOrJson();
                default:
                    throw new Exception("Unknown EProviderType");
            }
        }


        /// <summary>
        /// Dont have a use-case yet for this, but seemed likely to be needed at
        /// some point
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> LoadTextOrJson();


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
            var allMatches = Regex.Matches(_url, ScraperResources.UrlFormRegexPattern);
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
