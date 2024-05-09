using System;
using System.Collections.Generic;
using StockBox.Associations;
using StockBox.Associations.Enums;
using StockBox.Base.Tokens;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Context;
using StockBox.Data.Indicators;
using StockBox.Data.SbFrames.Helpers;

namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// The beginning of a system to automate the creation of valid ranged data
    /// based on an analysis of the provided expression. Each rule statement
    /// and resulting expression needs to be analyzed to get a full Domain
    /// Combination List
    /// </summary>
    public class FrameListFactory : ISbFrameListProvider
    {

        /// <summary>
        /// By default get data back to 01/01/2000
        /// </summary>
        private DateTime _historicalStart = new DateTime(2000, 1, 1);

        /// <summary>
        /// ICallContextProviders are intended to provide MemoryStream results
        /// that can be applied to the adapter through the AddData method
        /// </summary>
        private ICallContextProvider _ctx;
        private IDataPointListProvider _provider;

        public FrameListFactory(ICallContextProvider ctx, IDataPointListProvider provider)
        {
            _ctx = ctx;
            _provider = provider;
        }

        public SbFrame CreateDailySbFrame(IDomainCombinationsProvider dailyCombos, ISymbolProvider symbol)
        {
            var ret = new DailyFrame(_provider.Create(), symbol);

            var startDate = DateTimeFrameHelper.Get(dailyCombos, EFrequency.eDaily);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eDaily, startDate, endDate);

            var toDataPointListAdapter = new DeedleToDataPointListAdapter(payload.Stream);

            ret.AddData(toDataPointListAdapter.Convert());
            MapIndicators(ret, dailyCombos);
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(IDomainCombinationsProvider weeklyCombos, ISymbolProvider symbol)
        {
            var ret = new WeeklyFrame(_provider.Create(), symbol);

            var startDate = DateTimeFrameHelper.Get(weeklyCombos, EFrequency.eWeekly);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eWeekly, startDate, endDate);

            var toDataPointListAdapter = new DeedleToDataPointListAdapter(payload.Stream);

            ret.AddData(toDataPointListAdapter.Convert());
            MapIndicators(ret, weeklyCombos);
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(IDomainCombinationsProvider monthlyCombos, ISymbolProvider symbol)
        {
            var ret = new MonthlyFrame(_provider.Create(), symbol);

            var startDate = DateTimeFrameHelper.Get(monthlyCombos, EFrequency.eMonthly);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eMonthly, startDate, endDate);
            var toDataPointListAdapter = new DeedleToDataPointListAdapter(payload.Stream);

            ret.AddData(toDataPointListAdapter.Convert());
            MapIndicators(ret, monthlyCombos);
            return ret;
        }

        public SbFrame CreateDailySbFrame(ISymbolProvider symbol)
        {
            var ret = new DailyFrame(_provider.Create(), symbol);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eDaily, _historicalStart, endDate);
            var toDataPointListAdapter = new DeedleToDataPointListAdapter(payload.Stream);

            ret.AddData(toDataPointListAdapter.Convert());
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(ISymbolProvider symbol)
        {
            var ret = new WeeklyFrame(_provider.Create(), symbol);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eWeekly, _historicalStart, endDate);
            var toDataPointListAdapter = new DeedleToDataPointListAdapter(payload.Stream);

            ret.AddData(toDataPointListAdapter.Convert());
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(ISymbolProvider symbol)
        {
            var ret = new MonthlyFrame(_provider.Create(), symbol);
            var endDate = DateTimeFrameHelper.GetOrigin();
            var payload = StreamFactory.Create(symbol.Name, EFrequency.eMonthly, _historicalStart, endDate);
            var toDataPointListAdapter = new DeedleToDataPointListAdapter(payload.Stream);

            ret.AddData(toDataPointListAdapter.Convert());
            return ret;
        }

        private void MapIndicators(SbFrame frame, IDomainCombinationsProvider combos)
        {
            foreach (var c in (combos as DomainCombinationList).GetIndicators())
            {
                frame.AddIndicator(IndicatorFactory.Create(c.DomainKeyword, c.Indices));
            }
        }

        public List<ISbFrame> Create(IDomainCombinationsProvider combos, ISymbolProvider symbol)
        {
            SbFrameList ret = new SbFrameList();

            var dailyCombos = combos.GetDailyDomainCombos() as DomainCombinationList;
            if (dailyCombos.Count > 0)
                ret.Add(CreateDailySbFrame(dailyCombos, symbol));

            var weeklyCombos = combos.GetWeeklyDomainCombos() as DomainCombinationList;
            if (weeklyCombos.Count > 0)
                ret.Add(CreateWeeklySbFrame(weeklyCombos, symbol));

            var domainTokens = combos.GetDomainTokens() as DomainCombinationList;
            if (domainTokens.Count == 0)
            {
                var monthlyCombos = combos.GetMonthyDomainCombos() as DomainCombinationList;
                if (monthlyCombos.Count > 0)
                    ret.Add(CreateMonthlySbFrame(monthlyCombos, symbol));
            }
            else
            {
                ret.Add(CreateMonthlySbFrame(domainTokens, symbol));
            }

            return ret;
        }

        public List<ISbFrame> CreateBacktestData(ISymbolProvider symbol)
        {
            var ret = new SbFrameList();
            ret.Add(CreateDailySbFrame(symbol));
            ret.Add(CreateWeeklySbFrame(symbol));
            ret.Add(CreateMonthlySbFrame(symbol));
            return ret;
        }

        public void AddIndicators(List<ISbFrame> framelist, IDomainCombinationsProvider domainCombinations)
        {
            var fl = framelist as SbFrameList;
            var dailyFrameList = fl.FindByFrequency(EFrequency.eDaily);
            MapIndicators(dailyFrameList, domainCombinations.GetDailyDomainCombos());
            var weeklyFrameList = fl.FindByFrequency(EFrequency.eWeekly);
            MapIndicators(weeklyFrameList, domainCombinations.GetWeeklyDomainCombos());
            var montlyFrameList = fl.FindByFrequency(EFrequency.eMonthly);
            MapIndicators(montlyFrameList, domainCombinations.GetMonthyDomainCombos());
        }

        public void HydrateFrameList(List<ISbFrame> frameList, IDomainCombinationsProvider domainCombinationsProvider)
        {
            // query for historical data when seeing
            AddIndicators(frameList, domainCombinationsProvider);
        }


    }
}
