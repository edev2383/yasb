using System;
using StockBox.Actions;
using StockBox.Models;


namespace StockBox.States
{
    /// <summary>
    /// There are 6 application defined states, with corresponding
    /// responsible SbAction Parents:
    /// 
    /// - ActivePendingState
    /// - ActiveState
    /// - ActiveErrorState
    /// - InactivePendingState
    /// - InactiveState
    /// - InactiveErrorState
    ///
    /// These states are responsible for the handling of Buy/Sell
    /// actions that are defined by Setups. All other states will
    /// be UserDefinedState(s) that the user describes by state name
    ///
    /// ?: How to handle a split position? i.e., a setup decides to
    /// sell half of a position? I think the symbol would remain
    /// active, as the position is still open, rather than trying to
    /// split a symbol over two different states.
    ///
    /// However, this opens up another question. Can a symbol be in two
    /// states at once? i.e., I have two setups and a symbol tests true
    /// for both. Do I restrict a second position from being opened,
    /// just because an open position already exists?
    /// </summary>
    public abstract class StateBase : IState
    {
        public string Name { get { return _name; } }
        private readonly string _name;

        public StateDataModel State { get; set; }

        /// <summary>
        /// A reference to the parent SbActionBase object. When the State is
        /// entered, the Action method is called which returns the parent's
        /// PerformAction method response object
        /// </summary>
        public SbActionBase ParentAction { get; set; }

        public StateBase(StateDataModel state) : this(state.Name)
        {
            State = state;
        }

        public StateBase(string name)
        {
            _name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Action()
        {
            if (ParentAction != null) return ParentAction.PerformAction();
            return null;
        }

        public override string ToString()
        {
            return string.Format("State: {0}) {1} - {2}", State.StateDataModelId, State.Name, State.Type);
        }

        /// <summary>
        /// Return true if provided item matches, or is equivalent to, this item
        /// </summary>
        /// <param name="tryState"></param>
        /// <returns></returns>
        public bool IdentifiesAs(StateBase tryState)
        {
            if (Name != tryState.Name) return false;
            if (State.StateDataModelId != tryState.State.StateDataModelId) return false;
            if (State.Name != tryState.State.Name) return false;
            if (State.Type != tryState.State.Type) return false;
            return true;
        }

        #region Equality Overrides

        public static bool operator ==(StateBase left, StateBase right)
        {
            if (ReferenceEquals(left, null))
                return true;
            if (ReferenceEquals(right, left))
                return false;
            if (ReferenceEquals(right, null))
                return false;
            return left.Equals(right);
        }

        public static bool operator !=(StateBase left, StateBase right)
        {
            return !(left == right);
        }

        public bool Equals(StateBase obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return Name.Equals(obj.Name);
        }

        public override bool Equals(object obj) => Equals(obj as StateBase);

        public override int GetHashCode()
        {
            if (State != null)
                return HashCode.Combine(State.Name, State.StateDataModelId, State.Type);
            else
                return HashCode.Combine(_name);
        }

        #endregion
    }
}
