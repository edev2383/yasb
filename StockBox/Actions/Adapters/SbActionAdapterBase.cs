using System;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox.Actions.Adapters
{

    /// <summary>
    /// Class <c>SbActionAdapterBase</c> 
    /// </summary>
    public abstract class SbActionAdapterBase : ISbActionAdapter
    {
        public ISbAction ParentAction { get; set; }

        public ISbActionAdapter Clone()
        {
            throw new NotImplementedException();
        }

        public abstract ActionResponse PerformAction(DataPoint dataPoint);
    }
}
