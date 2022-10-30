using System;
namespace StockBox.Actions.Helpers
{
    public enum EMessageQueuePriority
    {
        eCritical = 1,
        eElevated = 2,
        eGeneral = 3,
        eNonce = 4,
    }
}
