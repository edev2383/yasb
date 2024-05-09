using System;
using System.Collections.Generic;
using StockBox.Associations;
using StockBox.Associations.Enums;
using StockBox.Base.Tokens;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;
using StockBox_TestArtifacts.Helpers;

namespace StockBox_TestArtifacts.Mocks
{

    /// <summary>
    /// Class <c>MockFrameListFactory</c> creates an SbFrameList from locally
    /// saved CSVs
    /// </summary>
    public class MockFrameListFactory : ISbFrameListProvider
    {
        public EFile DataTarget_Daily { get; set; }
        public EFile DataTarget_Weekly { get; set; }
        public EFile DataTarget_Monthly { get; set; }
        private IDataPointListProvider _provider;

        public MockFrameListFactory(ICallContextProvider ctx, IDataPointListProvider provider)
        {
            _provider = provider;
        }

        public List<ISbFrame> Create(IDomainCombinationsProvider combos, ISymbolProvider symbol)
        {
            SbFrameList ret = new SbFrameList();

            ret.Add(CreateDailySbFrame(combos, symbol));
            ret.Add(CreateWeeklySbFrame(combos, symbol));
            ret.Add(CreateMonthlySbFrame(combos, symbol));

            return ret;
        }

        public SbFrame CreateDailySbFrame(IDomainCombinationsProvider dailyCombos, ISymbolProvider symbol)
        {
            var ret = new DailyFrame(_provider.Create(), symbol);
            var reader = new Reader();

            var toDataPointListAdapater = new DeedleToDataPointListAdapter(reader.GetFileStream(DataTarget_Daily));

            ret.AddData(toDataPointListAdapater.Convert());
            MapIndicators(ret, dailyCombos);
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(IDomainCombinationsProvider weeklyCombos, ISymbolProvider symbol)
        {
            var ret = new WeeklyFrame(_provider.Create(), symbol);
            var reader = new Reader();
            var toDataPointListAdapater = new DeedleToDataPointListAdapter(reader.GetFileStream(DataTarget_Weekly));

            ret.AddData(toDataPointListAdapater.Convert());

            MapIndicators(ret, weeklyCombos);
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(IDomainCombinationsProvider monthlyCombos, ISymbolProvider symbol)
        {
            var ret = new MonthlyFrame(_provider.Create(), symbol);
            var reader = new Reader();
            var toDataPointListAdapater = new DeedleToDataPointListAdapter(reader.GetFileStream(DataTarget_Monthly));

            ret.AddData(toDataPointListAdapater.Convert());
            MapIndicators(ret, monthlyCombos);
            return ret;
        }

        private void MapIndicators(SbFrame frame, IDomainCombinationsProvider combos)
        {
            foreach (var c in (combos as DomainCombinationList).GetIndicators())
            {
                frame.AddIndicator(IndicatorFactory.Create(c.DomainKeyword, c.Indices));
            }
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

        public SbFrame CreateDailySbFrame(ISymbolProvider symbol)
        {
            var ret = new DailyFrame(_provider.Create(), symbol);
            var reader = new Reader();
            var toDataPointListAdapater = new DeedleToDataPointListAdapter(reader.GetFileStream(DataTarget_Daily));

            ret.AddData(toDataPointListAdapater.Convert());
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(ISymbolProvider symbol)
        {
            var ret = new WeeklyFrame(_provider.Create(), symbol);
            var reader = new Reader();
            var toDataPointListAdapater = new DeedleToDataPointListAdapter(reader.GetFileStream(DataTarget_Weekly));

            ret.AddData(toDataPointListAdapater.Convert());
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(ISymbolProvider symbol)
        {
            var ret = new MonthlyFrame(_provider.Create(), symbol);
            var reader = new Reader();
            var toDataPointListAdapater = new DeedleToDataPointListAdapter(reader.GetFileStream(DataTarget_Monthly));

            ret.AddData(toDataPointListAdapater.Convert());
            return ret;
        }

        public void HydrateFrameList(List<ISbFrame> frameList, IDomainCombinationsProvider domainCombinationsProvider)
        {
            AddIndicators(frameList, domainCombinationsProvider);
        }
    }
}
