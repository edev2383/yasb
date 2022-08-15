using System;
using StockBox.States;

namespace StockBox.Actions
{
    /// <summary>
    /// BuyError is really just a Move command. If we're performing a Buy Error,
    /// that means the Buy command came back with a failure response and we can
    /// transition the stock into the ActiveError state
    /// </summary>
    public class BuyError : SbActionBase
    {
        public BuyError(ISbActionAdapter adapter) : base(adapter, new ActiveErrorState())
        {
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
