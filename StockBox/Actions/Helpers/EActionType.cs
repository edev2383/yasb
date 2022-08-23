using System;


namespace StockBox.Actions.Helpers
{

    public enum EActionType
    {
        eUnknown = 0,
        eBuy = 1,
        eSell = 2,
        eMoveGeneral = 3,
        eMoveFailure = 4,
        eMoveSuccess = 5,
    }
}
