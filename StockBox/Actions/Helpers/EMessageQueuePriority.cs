using System;
namespace StockBox.Actions.Helpers
{
    public enum EMessageQueuePriority
    {
        Unknown = 0,
        Citical = 1,
        Elevated = 2,
        General = 3,
    }
}
