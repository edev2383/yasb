using System;
using StockBox.Associations;
using StockBox.Data.Managers;

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
    }
}
