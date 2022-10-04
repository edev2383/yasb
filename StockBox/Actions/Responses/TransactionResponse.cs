using System;


namespace StockBox.Actions.Responses
{

    public class TransactionResponse : ActionResponse
    {
        public TransactionResponse(bool isSuccess, string message, object source = null) : base(isSuccess, message, source)
        {
        }
    }
}
