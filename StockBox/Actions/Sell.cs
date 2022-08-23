using System;
using StockBox.States;

namespace StockBox.Actions
{
    public class Sell : SbActionBase
    {

        public Sell(Sell source) : base(source) { }

        public Sell(ISbActionAdapter adapter) : base(adapter, new InactivePendingState())
        {
        }

        public override SbActionBase Clone()
        {
            return new Sell(this);
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
