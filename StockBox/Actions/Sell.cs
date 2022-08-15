using System;
using StockBox.States;

namespace StockBox.Actions
{
    public class Sell : SbActionBase
    {
        public Sell(ISbActionAdapter adapter) : base(adapter, new InactivePendingState())
        {
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
