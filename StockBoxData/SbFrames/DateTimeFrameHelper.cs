using System;
using System.Linq;
using StockBox.Associations;
using StockBox.Associations.Enums;
using StockBox.Base.Tokens;


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
            var marginMultiplier = 2;

            // max is the number of units (days, weeks, months) to request. We
            // always take the max between direct index and indicators indices
            var max = Math.Max(combos.GetMaxIndex(), combos.GetMaxIndicatorIndex());

            // if there is no max set, set to 2 by default
            if (max == 0) max = 2;

            // set a minimal multiplier coef of 1 (daily).
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
                    {
                        // if the provided combo list has domain tokens, we want
                        // to go back for a larger historical dataset. In reality
                        // we only want this if we're querying for AllTimeHigh
                        // or AllTimeLow, but we can add that split at a later
                        // time.
                        var castCombos = combos as DomainCombinationList;
                        if (castCombos.First().IsDomainToken())
                            max = 12 * 40; // max = 40 years
                        frequencyMultiplier = 31;
                    }
                    break;
                default:
                    break;
            }

            // Explanation of variables:
            // -1: We're subtracting days from the origin, which would be today
            //     (adjusted to most recent Friday if request is on weekend), so
            //     use -1 to explicitly imply the inverse.
            // marginMultiplier: Our default request is double what is indexed,
            //     because we need the additional data to perform the proper
            //     calculations. Example: SMA(50) needs 50 days of data to begin
            //     populating the values at the start of the requested range
            // frequencyMultiplier: An offset to account for the requested freq.
            //     i.e., daily, weekly, monthly
            // max: The maximum found requested index, i.e., SMA(50) = 50, or
            //     "14 days ago" = 14. Note: For monthly builds, this value is
            //     overwritten if there are DomainTokens present in the combo
            //     list, as DomainTokens require the full historical dataset
            return GetOrigin().AddDays((-1) * (marginMultiplier) * frequencyMultiplier * max);
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
