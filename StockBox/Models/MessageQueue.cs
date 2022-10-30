using System;
using StockBox.Actions.Helpers;


namespace StockBox.Models
{

    /// <summary>
    /// Class <c>MessageQueue</c> models the MessageQueue table. Items are put
    /// into the MessageQueue and a sweeper service will send items as needed.
    /// </summary>
    public class MessageQueue
    {

        public int? MessageQueueId { get; set; }
        public int? UserId { get; set; }
        public string Message { get; set; }
        public DateTime? CreateDate { get; set; }
        public EMessageQueueDeliveryType DeliveryType { get; set; }
        public EMessageQueuePriority Priority { get; set; }
        public EMessageQueueStatus Status { get; set; }

        public MessageQueue()
        {
        }
    }
}
