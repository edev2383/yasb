using System;
namespace StockBox.Validation
{
    public interface IValidationResult
    {
        EResult Result { get; }
        string Message { get; }
        object ValidationObject { get; }
        // include them both to allow us to be more explicit inline
        bool IsSuccess { get; }
        bool IsFailure { get; }
    }
}
