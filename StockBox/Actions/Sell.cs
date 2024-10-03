using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.Positions;

namespace StockBox.Actions
{

    public class Sell : SbActionBase
    {

        public Sell(Sell source) : base(source) { }

        public Sell(ISbActionAdapter adapter) : base(adapter, new InactivePendingState(), EActionType.SellMarket)
        {
        }

        public override ISbAction Clone()
        {
            return new Sell(this);
        }

        public override ActionResponse Act(DataPoint dataPoint, Position position)
        {
            return Adapter.PerformAction(dataPoint, position);
        }
    }
}
