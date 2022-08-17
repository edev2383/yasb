using System;
using StockBox.States;


namespace StockBox.Actions
{

    /// <summary>
    /// BuySuccess is really just a Move command. If we're performing a Buy
    /// Success, that means the Buy command came back with a positive response
    /// and we can transition the stock into the Active state
    /// </summary>
    public class BuySuccess : SbActionBase
    {

        public BuySuccess(ISbActionAdapter adapter) : base(adapter, new ActiveState())
        {
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
