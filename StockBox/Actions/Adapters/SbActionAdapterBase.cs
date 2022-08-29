using System;
using StockBox.Data.Managers;

namespace StockBox.Actions.Adapters
{
    public class SbActionAdapterBase : ISbActionAdapter
    {
        public SbActionAdapterBase()
        {
        }


        public ISbAction ParentAction { get; set; }

        public ISbActionAdapter Clone()
        {
            throw new NotImplementedException();
        }
    }
}
