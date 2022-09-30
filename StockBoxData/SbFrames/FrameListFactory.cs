using System;
using StockBox.Associations;
using StockBox.Associations.Enums;
using StockBox.Associations.Tokens;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Context;
using StockBox.Data.Indicators;


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

        private DateTime _historicalStart = new DateTime(2000, 1, 1);

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

            var dailyCombos = combos.GetDailyDomainCombos();
            if (dailyCombos.Count > 0)
                ret.Add(CreateDailySbFrame(dailyCombos, symbol));

            var weeklyCombos = combos.GetWeeklyDomainCombos();
            if (weeklyCombos.Count > 0)
                ret.Add(CreateWeeklySbFrame(weeklyCombos, symbol));

            var monthlyCombos = combos.GetMonthyDomainCombos();
            if (monthlyCombos.Count > 0)
                ret.Add(CreateMonthlySbFrame(monthlyCombos, symbol));

            return ret;
        }

        public SbFrame CreateDailySbFrame(DomainCombinationList dailyCombos, ISymbolProvider symbol)
        {
            var ret = new DailyFrame(_adapter.Create(), symbol);

            var startDate = DateTimeFrameHelper.Get(dailyCombos, EFrequency.eDaily);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eDaily, startDate, endDate);

            ret.AddData(payload.Stream);
            MapIndicators(ret, dailyCombos);
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(DomainCombinationList weeklyCombos, ISymbolProvider symbol)
        {
            var ret = new WeeklyFrame(_adapter.Create(), symbol);

            var startDate = DateTimeFrameHelper.Get(weeklyCombos, EFrequency.eWeekly);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eWeekly, startDate, endDate);

            ret.AddData(payload.Stream);
            MapIndicators(ret, weeklyCombos);
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(DomainCombinationList monthlyCombos, ISymbolProvider symbol)
        {
            var ret = new MonthlyFrame(_adapter.Create(), symbol);

            var startDate = DateTimeFrameHelper.Get(monthlyCombos, EFrequency.eMonthly);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eMonthly, startDate, endDate);

            ret.AddData(payload.Stream);
            MapIndicators(ret, monthlyCombos);
            return ret;
        }

        public SbFrame CreateDailySbFrame(ISymbolProvider symbol)
        {
            var ret = new DailyFrame(_adapter.Create(), symbol);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eDaily, _historicalStart, endDate);
            ret.AddData(payload.Stream);
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(ISymbolProvider symbol)
        {
            var ret = new WeeklyFrame(_adapter.Create(), symbol);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eWeekly, _historicalStart, endDate);
            ret.AddData(payload.Stream);
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(ISymbolProvider symbol)
        {
            var ret = new MonthlyFrame(_adapter.Create(), symbol);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eMonthly, _historicalStart, endDate);
            ret.AddData(payload.Stream);
            return ret;
        }


        private void MapIndicators(SbFrame frame, DomainCombinationList combos)
        {
            foreach (var c in combos.GetIndicators())
            {
                frame.AddIndicator(IndicatorFactory.Create(c.DomainKeyword, c.Indices));
            }
        }

        public void AddIndicators(SbFrameList framelist, DomainCombinationList combos)
        {
            var dailyFrameList = framelist.FindByFrequency(EFrequency.eDaily);
            MapIndicators(dailyFrameList, combos.GetDailyDomainCombos());
            var weeklyFrameList = framelist.FindByFrequency(EFrequency.eWeekly);
            MapIndicators(weeklyFrameList, combos.GetWeeklyDomainCombos());
            var montlyFrameList = framelist.FindByFrequency(EFrequency.eMonthly);
            MapIndicators(montlyFrameList, combos.GetMonthyDomainCombos());
        }

        public SbFrameList CreateBacktestData(ISymbolProvider symbol)
        {
            var ret = new SbFrameList();
            ret.Add(CreateDailySbFrame(symbol));
            ret.Add(CreateWeeklySbFrame(symbol));
            ret.Add(CreateMonthlySbFrame(symbol));
            return ret;
        }
    }
}
