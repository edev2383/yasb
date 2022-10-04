using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Models;
using StockBox.Data.SbFrames;

namespace StockBox.Actions
{

    public interface ISbAction
    {
        EActionType ActionType { get; }
        StateBase TransitionState { get; }
        ISbActionAdapter Adapter { get; }
        SymbolProfile Symbol { get; set; }

        object Response { get; set; }

        ActionResponse PerformAction(DataPoint dataPoint);
        ISbAction Clone();
    }
}
