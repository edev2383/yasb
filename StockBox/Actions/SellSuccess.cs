using System;
using StockBox.States;


namespace StockBox.Actions
{

    public class SellSuccess : SbActionBase
    {

        public SellSuccess(SellSuccess source) : base(source) { }

        public SellSuccess(ISbActionAdapter adapter) : base(adapter, new InactivePendingState())
        {
        }

        public override SbActionBase Clone()
        {
            return new SellSuccess(this);
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
