using StockBox.RiskProfiles;
using StockBox.States;
using StockBox.Actions.Adapters;
using StockBox.Actions.Helpers;
using StockBox.Actions.Responses;
using StockBox.Models;
using StockBox.Data.SbFrames;

namespace StockBox.Actions
{

    /// <summary>
    /// Class <c>SbActionBase</c> wraps around an external call to an API, i.e.,
    /// the ISbActionAdapter, the expected response, if any, and a state to
    /// which the given symbol will be transitioned.
    /// </summary>
    public abstract class SbActionBase : ISbAction
    {
        public SymbolProfile Symbol { get; set; }
        public EActionType ActionType { get { return _actionType; } }
        private EActionType _actionType;

        /// An Action such as a Buy involves multiple steps, i.e., move stock to
        /// ActivePending, perform API call, and based on the API response move the
        /// stock to the Active or ActiveError states. How do we handle these? By
        /// creating a CompoundAction and chaining them, or somehow re-access?
        public SbActionBase(SbActionBase source) : this(source._adapter.Clone(), source._transitionState.Clone(), source._actionType)
        {
        }

        public SbActionBase(ISbActionAdapter adapter, StateBase transitionState, EActionType actionType)
        {
            _adapter = adapter;
            if (_adapter != null) // adapter is null for some unit tests (for now)
                _adapter.ParentAction = this;
            _transitionState = transitionState;
            _actionType = actionType;
        }

        /// <summary>
        /// The target state we attempt to transition to. If successful, we
        /// perform the action
        /// </summary>
        public StateBase TransitionState { get { return _transitionState; } }

        /// <summary>
        /// The adapter will actually perform the actions. This behavior ranges
        /// from updating CRUD methods to performing API calls to brokerages
        /// </summary>
        public ISbActionAdapter Adapter { get { return _adapter; } }

        /// <summary>
        /// The response object is a general object that can be returned to the
        /// StateMachine and processed by the DomainController (or other)
        /// </summary>
        public object Response { get; set; }

        /// <summary>
        /// Determines the settings for any brokerage action
        /// </summary>
        public RiskProfile RiskProfile { get; set; }

        /// <summary>
        /// private constructs
        /// </summary>
        private ISbActionAdapter _adapter;
        private StateBase _transitionState;

        public abstract ActionResponse Act(DataPoint dataPoint);
        public abstract ISbAction Clone();

    }
}
