using System;
using StockBox.States;

namespace StockBox.Actions
{
    // actions need an adapter to interface w/ the user's brokerage API
    public interface ISbAction
    {
        StateBase TransitionState { get; }
        ISbActionAdapter Adapter { get; }
        object Response { get; set; }

        /**
         * 
         * */
        object PerformAction();
    }
}
