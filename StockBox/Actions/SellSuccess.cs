using System;
using StockBox.States;

namespace StockBox.Actions
{
    public class SellSuccess : SbActionBase
    {
        public SellSuccess(ISbActionAdapter adapter) : base(adapter, new InactivePendingState())
        {
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
