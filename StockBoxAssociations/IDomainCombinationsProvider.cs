using System;
namespace StockBox.Associations
{
    public interface IDomainCombinationsProvider
    {
        double GetMaxIndex();
        double GetMaxIndicatorIndex();
        IDomainCombinationsProvider GetDailyDomainCombos();
        IDomainCombinationsProvider GetWeeklyDomainCombos();
        IDomainCombinationsProvider GetMonthyDomainCombos();
    }
}
