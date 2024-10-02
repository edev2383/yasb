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

        public override ActionResponse PerformAction(DataPoint dataPoint, Position position)
        {
            /// transition the Symbol to its new Inactive state
            ParentAction.Symbol.TransitionState(new InactiveState());

            /// return a successful sell response
            return new SellActionResponse(isSuccess: true,
                message: $"Sold Symbol '{ParentAction.Symbol.Symbol.Name}' at, or near, ${dataPoint.Close}",
                source: Order.Sell(
                            symbol: ParentAction.Symbol.Symbol,
                            shareCount: 0,
                            sharePrice: dataPoint.Close,
                            position: position));
        }
    }
}
