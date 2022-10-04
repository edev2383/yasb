using System;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox.Actions.Adapters
{

    /// <summary>
    /// The ActionAdapter will provide the interface for all actions. This
    /// includes simple state transitions, i.e., updating database records,
    /// to sending/receiving API requests
    /// </summary>
    public interface ISbActionAdapter
    {
        ISbAction ParentAction { get; set; }
        ISbActionAdapter Clone();
        ActionResponse PerformAction(DataPoint dataPoint);
    }
}
