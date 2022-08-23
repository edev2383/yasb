using System;
using StockBox.States;


namespace StockBox.Actions
{

    public class SellError : SbActionBase
    {

        public SellError(SellError source) : base(source) { }

        public SellError(ISbActionAdapter adapter) : base(adapter, new InactiveErrorState())
        {
        }

        public override SbActionBase Clone()
        {
            return new SellError(this);
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
