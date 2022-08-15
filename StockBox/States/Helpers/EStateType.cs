using System;
namespace StockBox.States.Helpers
{
    public enum EStateType
    {
        eUnknown = 0,
        eActivePending = 1,
        eActive = 2,
        eActiveError = 3,
        eInactivePending = 4,
        eInactive = 5,
        eInactiveError = 6,
        eUserDefined = 7,
    }
}
