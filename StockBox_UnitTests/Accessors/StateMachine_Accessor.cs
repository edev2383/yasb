using System;
using StockBox.States;

namespace StockBox_UnitTests.Accessors
{
    public class StateMachine_Accessor : StateMachine
    {
        public StateMachine_Accessor(StateList states) : base(states)
        {
        }

        public StateMachine_Accessor(StateList states, StateBase currentState) : base(states, currentState)
        {
        }

        public TransitionList GetTransitions()
        {
            return _transitions;
        }
    }
}
