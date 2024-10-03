using NHibernate.Mapping;
using StockBox.Associations.Enums;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Context;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Data.SbFrames
{
    /// <summary>
    /// <c>DataPointListFactory</c> acts as the boundary to get a DataPointList from
    /// the data source. The consumer will only need to provide the symbol, frequency,
    /// and date range, while all implementation details are hidden.
    /// </summary>
    public class DataPointListFactory
    {

        /// <summary>
        /// Create a DataPointList from which data source can be queried.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="frequency"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async static Task<DataPointList> Create(string symbol, EFrequency frequency, DateTime startDate, DateTime? endDate = null)
        {
            if (symbol.Length <= 4)
            {
                /// create stock scraper
                return await ScrapeAlphaVantage(symbol, frequency, startDate, endDate);
            } else if (symbol.Length == 6)
            {
                /// create forex scraper
                throw new NotImplementedException();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [Obsolete("Yahoo Finance hid the target behind a paywall.")]
        private static DataPointList CreateDataPointListFromYahooFinance(string symbol, EFrequency frequency, DateTime startDate, DateTime? endDate = null)
        {
            var payload = StreamFactory.Create(symbol, EFrequency.Daily, startDate, endDate);

            var toDataPointListAdapter = new DeedleToDataPointListYahooFinanceAdapter(payload.Stream);

            return toDataPointListAdapter.Convert();
        }

        private async static Task<DataPointList> ScrapeAlphaVantage(string symbol, EFrequency frequency, DateTime startDate, DateTime? endDate = null)
        {
            var alphaVantageInParams = new AlphaVantageSecurityHistoryProvider.AlphaVantageSecurityHistoryProvider_InType(
                    symbol: symbol,
                    frequency: frequency);
            var scraper = new SbScraper(new AlphaVantageSecurityHistoryProvider(alphaVantageInParams), new AlphaVantageSecurityHistoryParser());
            var payload = await scraper.Scrape() as AlphaVantageSecurityHistoryParser.HistoryParser_OutType;

            if (payload == null) throw new Exception("Payload is null");

            return payload.Data;
        }
    }
}
