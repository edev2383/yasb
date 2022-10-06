using System;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.Positions;
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

            var transaction = new Transaction(0, dataPoint.Close);
            transaction.Type = Positions.Helpers.ETransactionType.eSell;
            ret.Source = transaction;
            return ret;
        }
    }
}
