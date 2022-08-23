using System;
using StockBox.Data.Managers;

namespace StockBox.Actions.Adapters
{
    public class SbActionAdapterBase : ISbActionAdapter
    {
        public SbActionAdapterBase()
        {
        }

        public SbManagerBase Manager { get; set; }
        public ISbAction ParentAction { get; set; }

        public ISbActionAdapter Clone()
        {
            throw new NotImplementedException();
        }
    }
}
