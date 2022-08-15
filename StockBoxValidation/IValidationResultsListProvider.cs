using System;
namespace StockBox.Validation
{
    public interface IValidationResultsListProvider
    {
        ValidationResultList GetResults();
    }
}
