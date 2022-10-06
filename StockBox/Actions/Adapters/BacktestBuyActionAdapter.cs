using System;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;
using StockBox.Positions;
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
            // for backtest to continue, we transition directly to Active
            // because the backtest ignores the pending/error states
            ParentAction.Symbol.TransitionState(new ActiveState());
            var riskProfile = ParentAction.RiskProfile;
            var vrShares = riskProfile.CalculateTotalShares(dataPoint.Close);

            ret.Message = $"Bought ({vrShares.Shares}) Symbol '{ParentAction.Symbol.Symbol.Name}' at, or near, ${dataPoint.Close}";

            var transaction = new Transaction((int)vrShares.Shares, dataPoint.Close);
            transaction.Type = Positions.Helpers.ETransactionType.eBuy;
            ret.Source = transaction;
            return ret;
        }

    }
}
