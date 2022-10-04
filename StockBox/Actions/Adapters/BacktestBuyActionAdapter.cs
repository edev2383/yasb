using System;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.States;

namespace StockBox.Actions.Adapters
{

    /// <summary>
    /// Class <c>BacktestBuyActionAdapter</c> mocks a buy action/response
    /// </summary>
    public class BacktestBuyActionAdapter : BacktestActionAdapterBase
    {
        public BacktestBuyActionAdapter() : base()
        {
        }

        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            var ret = new BuyActionResponse(true);
            ParentAction.Symbol.TransitionState(new ActiveState());
            ret.Message = $"Bought Symbol '{ParentAction.Symbol.Symbol.Name}' at, or near, ${dataPoint.Close}";
            ret.Source = dataPoint;
            return ret;
        }

    }
}
