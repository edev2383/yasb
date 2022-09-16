using System;
using StockBox.Associations;
using StockBox.Associations.Tokens;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;
using StockBox.Data.Scraper;
using StockBox.Data.Scraper.Parsers;
using StockBox.Data.Scraper.Providers;

namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// The beginning of a system to automate the creation of valid ranged data
    /// based on an analysis of the provided expression. Each rule statement
    /// and resulting expression needs to be analyzed to get a full Domain
    /// Combination List
    /// </summary>
    public class FrameListFactory
    {

        /// <summary>
        /// ICallContextProviders are intended to provide MemoryStream results
        /// that can be applied to the adapter through the AddData method
        /// </summary>
        private ICallContextProvider _ctx;
        private IDataFrameAdapter _adapter;

        public FrameListFactory(ICallContextProvider ctx, IDataFrameAdapter adapter)
        {
            _ctx = ctx;
            _adapter = adapter;
        }

        public SbFrameList Create(DomainCombinationList combos, ISymbolProvider symbol)
        {
            SbFrameList ret = new SbFrameList();

            var daily = CreateDailySbFrame(combos.GetDailyDomainCombos(), symbol);
            if (daily != null)
                ret.Add(daily);

            var weekly = CreateWeeklySbFrame(combos.GetWeeklyDomainCombos(), symbol);
            if (weekly != null)
                ret.Add(weekly);

            var monthly = CreateMonthlySbFrame(combos.GetMonthyDomainCombos(), symbol);
            if (monthly != null)
                ret.Add(monthly);

            return ret;
        }

        public SbFrame CreateDailySbFrame(DomainCombinationList dailyCombos, ISymbolProvider symbol)
        {
            var ret = new DailyFrame(_adapter.Create());

            var interval = HistoryYahooFinanceProvider.EHistoryInterval.eDaily;

            var inType = new HistoryYahooFinanceProvider.HistoryYahooFinanceProvider_InType()
            {
                Symbol = symbol.Name,
                Interval = interval,
                EndDate = DateTime.Now,
                StartDate = DateTimeFrameHelper.Get(dailyCombos, interval),
            };

            var scraperProvider = new HistoryYahooFinanceProvider(inType);
            var scraperParser = new HistoryYahooFinanceParser();
            var scraper = new SbScraper(scraperProvider, scraperParser);
            var payload = scraper.Scrape() as HistoryYahooFinanceParser.HistoryParser_OutType;

            ret.AddData(payload.Stream);
            MapIndicators(ret, dailyCombos);
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(DomainCombinationList weeklyCombos, ISymbolProvider symbol)
        {
            var ret = new WeeklyFrame(_adapter.Create());

            var interval = HistoryYahooFinanceProvider.EHistoryInterval.eDaily;

            var inType = new HistoryYahooFinanceProvider.HistoryYahooFinanceProvider_InType()
            {
                Symbol = symbol.Name,
                Interval = interval,
                EndDate = DateTime.Now,
                StartDate = DateTimeFrameHelper.Get(weeklyCombos, interval),
            };

            var scraperProvider = new HistoryYahooFinanceProvider(inType);
            var scraperParser = new HistoryYahooFinanceParser();
            var scraper = new SbScraper(scraperProvider, scraperParser);
            var payload = scraper.Scrape() as HistoryYahooFinanceParser.HistoryParser_OutType;

            ret.AddData(payload.Stream);
            MapIndicators(ret, weeklyCombos);
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(DomainCombinationList monthlyCombos, ISymbolProvider symbol)
        {
            var ret = new MonthlyFrame(_adapter.Create());

            var interval = HistoryYahooFinanceProvider.EHistoryInterval.eDaily;

            var inType = new HistoryYahooFinanceProvider.HistoryYahooFinanceProvider_InType()
            {
                Symbol = symbol.Name,
                Interval = interval,
                EndDate = DateTime.Now,
                StartDate = DateTimeFrameHelper.Get(monthlyCombos, interval),
            };

            var scraperProvider = new HistoryYahooFinanceProvider(inType);
            var scraperParser = new HistoryYahooFinanceParser();
            var scraper = new SbScraper(scraperProvider, scraperParser);
            var payload = scraper.Scrape() as HistoryYahooFinanceParser.HistoryParser_OutType;

            ret.AddData(payload.Stream);
            MapIndicators(ret, monthlyCombos);
            return ret;
        }

        private void MapIndicators(SbFrame frame, DomainCombinationList combos)
        {
            foreach (var c in combos.GetIndicators())
            {
                frame.AddIndicator(IndicatorFactory.Create(c.DomainKeyword, c.Indices));
            }
        }
    }
}
