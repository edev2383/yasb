using System;
using System.Linq;
using StockBox.Validation;

namespace StockBox.States
{
    /// <summary>
    /// Contains and controls all State transitions. All available States and
    /// valid Transitions are exlicitly defined beforehand to prevent invalid
    /// states. UserDefinedState transitions are Nondeterministic, i.e., any
    /// UserDefinedState can be transitioned into any other UserDefinedState.
    /// There are application defined states that behave deterministicly. These
    /// states (ActivePendingState, ActiveState, ActiveErrorState,
    /// InactivePendingState, InactiveState, InactiveErrorState) specifically
    /// handle the Buy/Sell actions of given setups.
    /// </summary>
    public class StateMachine
    {

        public StateBase CurrentState { get { return _currentState; } }
        public StateList States
        {
            get
            {
                if (_states == null)
                    _states = new StateList();
                return _states;
            }
        }

        /// <summary>
        /// A list of all valid transitions
        /// </summary>
        protected TransitionList _transitions = new TransitionList();

        /// <summary>
        /// On transition, _currentState gets added to _history, in order to
        /// keep track of performed transitions
        /// </summary>
        protected StateList _history = new StateList();

        /// <summary>
        /// A list of all valid states. Transition states MUST already exist in
        /// the StateList (_states) to be added
        /// </summary>
        protected StateList _states;

        /// <summary>
        /// The current state of the StateMachine, provided via constructor.
        /// When attempting to transition to a new state, we must confirm the
        /// SM is able to Transition from _currentState to newState.
        /// </summary>
        protected StateBase _currentState;

        /// <summary>
        /// Return the last state in the _history
        /// </summary>
        protected StateBase _previousState { get { return _history.Last(); } }

        public StateMachine(StateList states)
        {
            _states = states;
        }

        public StateMachine(StateList states, StateBase startState) : this(states)
        {
            _currentState = startState;
        }

        public StateMachine(StateMachine source)
        {
            _transitions = source._transitions;
            _states = source._states;
        }

        public StateMachine CreateWithStateAndTransitions()
        {
            return new StateMachine(this);
        }


        /// <summary>
        /// Manually set the current state
        /// </summary>
        /// <param name="state"></param>
        public void SetCurrentState(StateBase state)
        {
            _currentState = state;
        }

        /// <summary>
        /// Add a Transition object to the StateMachines list of valid
        /// Transitions (_transitions)
        /// </summary>
        /// <param name="transition"></param>
        public void AddTransition(Transition transition)
        {
            var vr = TransitionCanBeAdded(transition);
            if (vr.Success)
                _transitions.Add(transition);
        }

        /// <summary>
        /// Return true if the Transition already exists in the StateMachine's
        /// _transitions list
        /// </summary>
        /// <param name="tryTransition"></param>
        /// <returns></returns>
        public bool TransitionExists(Transition tryTransition)
        {
            return _transitions.ContainsItem(tryTransition);
        }

        /// <summary>
        /// Validate the provided Transition can be added to the list of valid
        /// Transitions.
        ///
        /// A Transition can only be added if its states are valid and if the
        /// Transition does not exist already
        /// </summary>
        /// <param name="tryTransition"></param>
        /// <returns></returns>
        public ValidationResultList TransitionCanBeAdded(Transition tryTransition)
        {
            var ret = new ValidationResultList();

            // Can only add transitions that have valid start and end states
            ret.Add(States.ContainsItem(tryTransition.StartState), "Transition.StateState found in StateList (_states)");
            ret.Add(States.ContainsItem(tryTransition.EndState), "Transition.EndState found in StateList (_states)");

            // we want true/success if the provided transitions is NOT already
            // in the _transitions property
            ret.Add(!TransitionExists(tryTransition), "Transitions (_transitions) DOES NOT contain provided transtion");

            return ret;
        }

        /// <summary>
        /// Attempt to transition the StateMachine to the next state. If
        /// successful, we add the current state to the _history, update current
        /// state to the provided next state and perform the state's Action
        /// method.
        /// </summary>
        /// <param name="tryState"></param>
        /// <returns></returns>
        public ValidationResultList TryNextState(StateBase tryState)
        {
            var vr = new ValidationResultList();

            // create a Transition object based on the SM's current state and
            // the provided next state. If true, we get success and can perform
            // the requested transition
            vr.Add(TransitionExists(new Transition(_currentState, tryState)), $"Provided state {tryState.ToString()} is valid target from {_currentState.ToString()}");
            if (vr.Success)
                PerformTransition(tryState);
            return vr;
        }

        /// <summary>
        /// Add current state to history, update current state to next state 
        /// </summary>
        /// <param name="nextState"></param>
        /// <returns></returns>
        protected void PerformTransition(StateBase nextState)
        {
            _history.Add(_currentState.Clone());
            _currentState = nextState;
        }
    }
}
