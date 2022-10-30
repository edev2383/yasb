using System;
namespace StockBox.Validation
{
    public interface IValidationResultProvider
    {
        EResult Result { get; }
        string Message { get; }
        object ValidationObject { get; }
    }
}
