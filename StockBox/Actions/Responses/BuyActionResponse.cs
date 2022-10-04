using System;


namespace StockBox.Actions.Responses
{

    /// <summary>
    /// Class <c>BuyActionResponse</c>
    /// </summary>
    public class BuyActionResponse : TransactionResponse
    {
        public BuyActionResponse(bool isSuccess, string message = "", object source = null)
            : base(isSuccess, message, source)
        {
        }
    }
}
