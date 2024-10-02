using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.Positions;

namespace StockBox.Actions
{

    public class SellSuccess : SbActionBase
    {

        public SellSuccess(SellSuccess source) : base(source) { }

        public SellSuccess(ISbActionAdapter adapter) : base(adapter, new InactivePendingState(), EActionType.MoveAutoSuccess)
        {
        }

        public override ISbAction Clone()
        {
            return new SellSuccess(this);
        }

        public override ActionResponse Act(DataPoint dataPoint, Position position)
        {
            return null;
        }
    }
}
