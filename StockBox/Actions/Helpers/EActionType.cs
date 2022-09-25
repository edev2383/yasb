using System;


namespace StockBox.Actions.Helpers
{

    public enum EActionType
    {
        eUnknown = 0,
        eBuy,
        eSell,
        eMoveGeneral,
        eMoveFailure,
        eMoveSuccess,
        eAlert,
    }
}
