using System;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox.Actions.Adapters
{

    /// <summary>
    /// Class <c>BacktestMoveActionAdapter</c>
    /// </summary>
    public class BacktestMoveActionAdapter : BacktestActionAdapterBase
    {
        public BacktestMoveActionAdapter()
        {
        }

        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            var ret = new ActionResponse(true);
            ParentAction.Symbol.TransitionState(ParentAction.TransitionState);
            ret.Message = $"Symbol {ParentAction.Symbol.Symbol.Name} was moved to {ParentAction.TransitionState.Name}";
            ret.Source = dataPoint;
            return ret;
        }
    }
}
