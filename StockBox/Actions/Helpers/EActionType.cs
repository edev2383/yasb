using System;


namespace StockBox.Actions.Helpers
{

    public enum EActionType
    {
        Unknown = 0,
        BuyMarket,
        BuyStop,
        BuyLimit,
        SellMarket,
        SellStop,
        SellLimit,
        MoveGeneral,
        MoveAutoFailure,
        MoveAutoSuccess,
        Alert,
    }
}
