using System;
using StockBox.Associations.Tokens;
using static StockBox.Data.Scraper.Providers.HistoryYahooFinanceProvider;

namespace StockBox.Data.SbFrames
{
    public class DateTimeFrameHelper
    {
        public static DateTime Get(DomainCombinationList combos, EHistoryInterval interval)
        {
            // Always take the max between direct index and indicators indices
            var max = Math.Max(combos.GetMaxIndex(), combos.GetMaxIndicatorIndex());

            // if there is no max set, set to 2 by default
            if (max == 0) max = 2;

            // set a minimal multiplier coef of 1 (daily)
            var multiplier = 1;
            switch (interval)
            {
                // account for offset of weekend/holidays
                case EHistoryInterval.eDaily:
                    max += Math.Ceiling(max / 7) * 3;
                    break;
                case EHistoryInterval.eWeekly:
                    multiplier = 7;
                    break;
                case EHistoryInterval.eMonthly:
                    multiplier = 31;
                    break;
                default:
                    break;
            }
            return GetOrigin().AddDays((-2) * multiplier * max);
        }

        /// <summary>
        /// Adjust for requests done on weekend days
        /// </summary>
        /// <returns></returns>
        public static DateTime GetOrigin()
        {
            var ret = DateTime.Now.Date;
            if (ret.DayOfWeek == DayOfWeek.Sunday)
                ret = ret.AddDays(-2);
            if (ret.DayOfWeek == DayOfWeek.Saturday)
                ret = ret.AddDays(-1);
            return ret;
        }
    }
}
