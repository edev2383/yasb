using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox.Actions
{

    public class SellError : SbActionBase
    {

        public SellError(SellError source) : base(source) { }

        public SellError(ISbActionAdapter adapter) : base(adapter, new InactiveErrorState(), EActionType.eMoveFailure)
        {
        }

        public override ISbAction Clone()
        {
            return new SellError(this);
        }

        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            return null;
        }
    }
}
