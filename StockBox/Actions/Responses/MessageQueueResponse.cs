using System;
namespace StockBox.Actions.Responses
{
    public class MessageQueueResponse : ActionResponse
    {
        public MessageQueueResponse(bool isSuccess, string message = "", object source = null)
            : base(isSuccess, message, source)
        {
        }
    }
}
