using System;


namespace StockBox.Actions.Responses
{

    /// <summary>
    /// Class <c>SellActionResponse</c>
    /// </summary>
    public class SellActionResponse : TransactionResponse
    {
        public SellActionResponse(bool isSuccess, string message = "", object source = null)
            : base(isSuccess, message, source)
        {
        }
    }
}
