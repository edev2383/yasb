using System;
using StockBox.States;

namespace StockBox.Actions
{
    public class SellError : SbActionBase
    {
        public SellError(ISbActionAdapter adapter) : base(adapter, new InactiveErrorState())
        {
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
