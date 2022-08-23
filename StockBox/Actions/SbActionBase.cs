using StockBox.RiskProfiles;
using StockBox.States;


namespace StockBox.Actions
{

    /// <summary>
    /// An action acts as a wrapper around an external call to an API, i.e.,
    /// the ISbActionAdapter, the expected response, if any, and a state to
    /// which the given symbol will be transitioned
    ///
    /// An Action such as a Buy involves multiple steps, i.e., move stock to
    /// ActivePending, perform API call, and based on the API response move the
    /// stock to the Active or ActiveError states. How do we handle these? By
    /// creating a CompoundAction and chaining them, or somehow re-access?
    /// </summary>
    public abstract class SbActionBase : ISbAction
    {

        public SbActionBase(SbActionBase source) : this(source._adapter.Clone(), source._transitionState.Clone())
        {
        }

        public SbActionBase(ISbActionAdapter adapter, StateBase transitionState)
        {
            _adapter = adapter;
            _transitionState = transitionState;
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

        public abstract object PerformAction();
        public abstract SbActionBase Clone();
    }
}
