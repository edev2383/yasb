using System;
namespace StockBox.Actions
{
    /// <summary>
    /// The ActionAdapter will provide the interface for all actions. This
    /// includes simple state transitions, i.e., updating database records,
    /// to sending/receiving API requests
    /// </summary>
    public interface ISbActionAdapter
    {

        ISbActionAdapter Clone();
    }
}
