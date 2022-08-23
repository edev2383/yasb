using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;


namespace StockBox.Actions
{

    /// <summary>
    /// BuySuccess is really just a Move command. If we're performing a Buy
    /// Success, that means the Buy command came back with a positive response
    /// and we can transition the stock into the Active state
    /// </summary>
    public class BuySuccess : SbActionBase
    {

        public BuySuccess(BuySuccess source) : base(source) { }

        public BuySuccess(ISbActionAdapter adapter) : base(adapter, new ActiveState(), EActionType.eMoveSuccess)
        {
        }

        public override SbActionBase Clone()
        {
            return new BuySuccess(this);
        }

        public override ActionResponse PerformAction()
        {
            return null;
        }
    }
}
