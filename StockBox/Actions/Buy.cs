using StockBox.States;
using StockBox.Validation;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Data.SbFrames;

namespace StockBox.Actions
{

    /// <summary>
    /// Buy brokerage initiator. The state is transitioned to Pending while we
    /// wait for a response from the API. If successful, we transition to
    /// Active state, otherwise to ActiveError. The action will be an API call
    /// to a brokerage, or in the case of testing, the creation of a test record
    /// </summary>
    public class Buy : SbActionBase
    {

        public Buy(Buy source) : base(source) { }

        public Buy(ISbActionAdapter adapter) : base(adapter, new ActivePendingState(), EActionType.eBuy)
        {
        }

        public override ISbAction Clone()
        {
            return new Buy(this);
        }

        public override ActionResponse PerformAction(DataPoint dataPoint)
        {
            return Adapter.PerformAction(dataPoint);
        }
    }
}
