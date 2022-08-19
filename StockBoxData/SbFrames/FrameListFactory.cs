using System;
using StockBox.Associations;
using StockBox.Associations.Tokens;
using StockBox.Data.Adapters.DataFrame;
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
        private ICallContextProvider _ctx;
        private IDataFrameAdapter _adapter;

        public FrameListFactory(ICallContextProvider ctx, IDataFrameAdapter adapter)
        {
            _ctx = ctx;
            _adapter = adapter;
        }

        public SbFrameList Create(DomainCombinationList combos)
        {
            SbFrameList ret = new SbFrameList();

            var daily = CreateDailySbFrame(combos.GetDailyDomainCombos());
            if (daily != null)
                ret.Add(daily);

            var weekly = CreateWeeklySbFrame(combos.GetWeeklyDomainCombos());
            if (weekly != null)
                ret.Add(weekly);

            var monthly = CreateMonthlySbFrame(combos.GetMonthyDomainCombos());
            if (monthly != null)
                ret.Add(monthly);

            return ret;
        }

        public SbFrame CreateDailySbFrame(DomainCombinationList dailyCombos)
        {
            var ret = new DailyFrame(_adapter);
            double maxIndex = dailyCombos.GetMaxIndex();
            //_adapter.AddData(_ctx.)
            MapIndicators(ret, dailyCombos);
            return ret;
        }

        public SbFrame CreateWeeklySbFrame(DomainCombinationList weeklyCombos)
        {
            var ret = new WeeklyFrame(_adapter);
            double maxIndex = weeklyCombos.GetMaxIndex();
            MapIndicators(ret, weeklyCombos);
            return ret;
        }

        public SbFrame CreateMonthlySbFrame(DomainCombinationList monthlyCombos)
        {
            var ret = new MonthlyFrame(_adapter);
            double maxIndex = monthlyCombos.GetMaxIndex();
            MapIndicators(ret, monthlyCombos);
            return ret;
        }

        private void MapIndicators(SbFrame frame, DomainCombinationList combos)
        {
            foreach (var c in combos)
            {
                if (c.IsIndicator)
                    frame.AddIndicator(IndicatorFactory.Create(c.DomainKeyword, c.Indices));
            }
        }
    }
}
