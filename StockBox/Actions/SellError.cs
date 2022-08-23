using System;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;


namespace StockBox.Actions
{

    public class SellError : SbActionBase
    {

        public SellError(SellError source) : base(source) { }

        public SellError(ISbActionAdapter adapter) : base(adapter, new InactiveErrorState(), EActionType.eMoveFailure)
        {
        }

        public override SbActionBase Clone()
        {
            return new SellError(this);
        }

        public override ActionResponse PerformAction()
        {
            return null;
        }
    }
}
