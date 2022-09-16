﻿using System;
using StockBox.Associations.Tokens;
using static StockBox.Data.Scraper.Providers.HistoryYahooFinanceProvider;

namespace StockBox.Data.SbFrames
{
    public class DateTimeFrameHelper
    {
        public static DateTime Get(DomainCombinationList combos, EHistoryInterval interval)
        {
            var max = combos.GetMaxIndex();
            var multiplier = 1;
            switch (interval)
            {
                case EHistoryInterval.eWeekly:
                    multiplier = 5;
                    break;
                case EHistoryInterval.eMonthly:
                    multiplier = 25;
                    break;
                default:
                    break;
            }
            return DateTime.Now.AddDays((-1) * multiplier * max);
        }
    }
}