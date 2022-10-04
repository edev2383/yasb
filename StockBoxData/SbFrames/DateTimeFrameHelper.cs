using System;
using StockBox.Associations;
using StockBox.Associations.Enums;
using StockBox.Associations.Tokens;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// Class <c>DateTimeFrameHelper</c> converts CombinationList and EFrequency
    /// into a DateTime origin for a data range
    /// </summary>
    public class DateTimeFrameHelper
    {

        public static DateTime Get(IDomainCombinationsProvider combos, EFrequency interval)
        {
            // To ensure we get a sufficient dataset, we want to get more than
            // what is requested. This can be adjusted as we get more usage
            // details
            var marginMultiplier = -2;

            // Always take the max between direct index and indicators indices
            var max = Math.Max(combos.GetMaxIndex(), combos.GetMaxIndicatorIndex());

            // if there is no max set, set to 2 by default
            if (max == 0) max = 2;

            // set a minimal multiplier coef of 1 (daily)
            var frequencyMultiplier = 1;
            switch (interval)
            {
                // account for offset of weekend/holidays.
                // Note: using a 2 wasn't quite enough to guarantee total
                // expected coverage, so to err on the side of caution, use 3.
                case EFrequency.eDaily:
                    max += Math.Ceiling(max / 7) * 3;
                    break;
                case EFrequency.eWeekly:
                    frequencyMultiplier = 7;
                    break;
                case EFrequency.eMonthly:
                    frequencyMultiplier = 31;
                    break;
                default:
                    break;
            }

            return GetOrigin().AddDays((marginMultiplier) * frequencyMultiplier * max);
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
