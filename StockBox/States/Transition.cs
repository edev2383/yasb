using System;
namespace StockBox.States
{
    public class Transition
    {
        public StateBase StartState { get { return _startState; } }
        public StateBase EndState { get { return _endState; } }

        private readonly StateBase _startState;
        private readonly StateBase _endState;

        public Transition(StateBase startState, StateBase endState)
        {
            _startState = startState;
            _endState = endState;
        }

        public override string ToString()
        {
            return string.Format("State Transition: {0}) -> {1}", StartState, EndState);
        }

        /// <summary>
        /// Return true if provided item matches, or is equivalent to, this item
        /// </summary>
        /// <param name="tryTransition"></param>
        /// <returns></returns>
        public bool IdentifiesAs(Transition tryTransition)
        {
            if (!tryTransition.StartState.Equals(StartState)) return false;
            if (!tryTransition.EndState.Equals(EndState)) return false;
            return true;
        }

        #region Equality Overrides
        public static bool operator ==(Transition left, Transition right)
        {
            if (ReferenceEquals(left, null))
                return true;
            if (ReferenceEquals(right, left))
                return false;
            if (ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(Transition left, Transition right)
        {
            return !(left == right);
        }

        public bool Equals(Transition obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return _startState.Equals(obj.StartState) && _endState.Equals(obj.EndState);
        }
        public override bool Equals(object obj) => Equals(obj as Transition);

        public override int GetHashCode()
        {
            return HashCode.Combine(_startState, _endState);
        }
        #endregion
    }
}
