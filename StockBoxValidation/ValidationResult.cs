using System;


namespace StockBox.Validation
{

    /// <summary>
    /// Class <c>ValidationResult</c> encapsulates a "result", along with an
    /// optional object. Typically this can be used to track the building
    /// process of more complex operations. The optional `ValidationObject` can
    /// be anything relevant to the operation. 
    /// </summary>
    public class ValidationResult : IValidationResult
    {
        public ValidationResult(EResult result, string message, object aux = null)
        {
            _result = result;
            _message = message;
            _validationObject = aux;
        }

        public ValidationResult(bool result, string message, object aux = null)
            : this(result ? EResult.eSuccess : EResult.eFail, message, aux)
        {
        }

        public EResult Result { get { return _result; } }
        public string Message { get { return _message; } }

        public object ValidationObject { get { return _validationObject; } }
        public bool IsSuccess { get { return !IsFailure; } }
        public bool IsFailure { get { return _result == EResult.eFail; } }

        private readonly EResult _result;
        private readonly string _message;
        private readonly object _validationObject;
    }
}
