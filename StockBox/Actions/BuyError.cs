using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.Positions;

namespace StockBox.Actions
{

    /// <summary>
    /// BuyError is really just a Move command. If we're performing a Buy Error,
    /// that means the Buy command came back with a failure response and we can
    /// transition the stock into the ActiveError state
    /// </summary>
    public class BuyError : SbActionBase
    {

        public BuyError(BuyError source) : base(source) { }

        public BuyError(ISbActionAdapter adapter) : base(adapter, new ActiveErrorState(), EActionType.MoveAutoFailure)
        {
        }

        public override ISbAction Clone()
        {
            return new BuyError(this);
        }

        public override ActionResponse Act(DataPoint dataPoint, Position position)
        {
            return null;
        }
    }
}
