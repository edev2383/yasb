using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;

namespace StockBox.Actions
{

    // actions need an adapter to interface w/ the user's brokerage API
    public interface ISbAction
    {
        EActionType ActionType { get; }
        StateBase TransitionState { get; }
        ISbActionAdapter Adapter { get; }

        object Response { get; set; }

        /**
         * 
         * */
        ActionResponse PerformAction();
        ISbAction Clone();
    }
}
