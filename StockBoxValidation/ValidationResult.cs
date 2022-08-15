using System;
namespace StockBox.Validation
{
    public class ValidationResult : IValidationResult
    {
        public ValidationResult(EResult result, string message, object aux = null)
        {
            _result = result;
            _message = message;
            _aux = aux;
        }

        public ValidationResult(bool result, string message, object aux = null)
            : this(result ? EResult.eSuccess : EResult.eFail, message, aux)
        {
        }

        public EResult Result { get { return _result; } }
        public string Message { get { return _message; } }
        public object Aux { get { return _aux; } }
        public bool IsSuccess { get { return !IsFailure; } }
        public bool IsFailure { get { return _result == EResult.eFail; } }

        private readonly EResult _result;
        private readonly string _message;
        private readonly object _aux;
    }
}
