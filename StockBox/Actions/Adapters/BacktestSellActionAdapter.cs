using System;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.States;

namespace StockBox.Actions.Adapters
{

    /// <summary>
    /// Class <c>BacktestSellActionAdapter</c> mocks a sell action/response
    /// </summary>
    public class BacktestSellActionAdapter : BacktestActionAdapterBase
    {
        public BacktestSellActionAdapter() : base()
        {
        }

        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            var ret = new SellActionResponse(true);
            ParentAction.Symbol.TransitionState(new InactiveState());
            ret.Message = $"Sold Symbol '{ParentAction.Symbol.Symbol.Name}' at, or near, ${dataPoint.Close}";
            ret.Source = dataPoint;
            return ret;
        }
    }
}
