using System;
using StockBox.States;

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
        public Buy(ISbActionAdapter adapter) : base(adapter, new ActivePendingState())
        {
        }

        public override object PerformAction()
        {
            return null;
        }
    }
}
