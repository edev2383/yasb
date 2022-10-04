using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox.Actions
{

    public class SellSuccess : SbActionBase
    {

        public SellSuccess(SellSuccess source) : base(source) { }

        public SellSuccess(ISbActionAdapter adapter) : base(adapter, new InactivePendingState(), EActionType.eMoveSuccess)
        {
        }

        public override ISbAction Clone()
        {
            return new SellSuccess(this);
        }

        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            return null;
        }
    }
}
