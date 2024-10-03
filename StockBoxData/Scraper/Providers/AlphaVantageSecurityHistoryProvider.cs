using StockBox.Associations.Enums;
using StockBox.Data.Scraper.Helpers;
using StockBox.Data.Scraper.Providers.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Data.Scraper.Providers
{
    public class AlphaVantageSecurityHistoryProvider : ScraperProviderBase
    {

        public AlphaVantageSecurityHistoryProvider(AlphaVantageSecurityHistoryProvider_InType inParams)
            : base(
                  url: ScraperResources.I().AlphaVantage.UrlSecurityHistory,
                  inParams: inParams,
                  type: EProviderType.Json)
        { }

        public class AlphaVantageSecurityHistoryProvider_InType : InType
        {
            public string Symbol { get; set; }
            public string ApiKey { get { return ScraperResources.I().AlphaVantage.ApiKey; } }
            /// <summary>
            /// "compact" or "full" - compact returns only the latest 100 data points
            /// </summary>
            public string OutputSize { get; set; }
            
            /// <summary>
            /// "json" or "csv"
            /// </summary>
            public string DataType { get; set; }
            public string Frequency {  get { return _mapFrequency(); } }
            private EFrequency _frequency;

            private string _mapFrequency()
            {
                switch (_frequency)
                {
                    case EFrequency.Daily:
                        return ScraperResources.I().AlphaVantage.FrequencyDaily;
                    case EFrequency.Weekly:
                        return ScraperResources.I().AlphaVantage.FrequencyWeekly;
                    case EFrequency.Monthly:
                        return ScraperResources.I().AlphaVantage.FrequencyMonthly;
                    default:
                        throw new Exception("Invalid frequency");
                }
            }

            public AlphaVantageSecurityHistoryProvider_InType() { }
            public AlphaVantageSecurityHistoryProvider_InType(string symbol, EFrequency frequency, string outputSize = "compact", string dataType = "json")
            {
                Symbol = symbol;
                OutputSize = outputSize;
                DataType = dataType;
                _frequency = frequency;
            }
        }

        public async override Task<string> LoadTextOrJson()
        {
            var client = new HttpClient();
            return await client.GetStringAsync(Url);
        }
    }
}
