using StockBox.Data.SbFrames;
using StockBox.Setups;
using StockBox.States;


namespace StockBox.Models
{

    public class SymbolProfile
    {

        /// <summary>
        /// Symbol model
        /// </summary>
        public Symbol Symbol { get; set; }

        /// <summary>
        /// The current state of profile
        /// </summary>
        public StateBase State { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataPoint DataPoint { get; set; }

        /// <summary>
        /// A list to hold onto previous state objects. It's unlikely that this
        /// will ever contain more than one state, as we most likely want to
        /// prevent multiple state transitions for a single symbol, however
        /// during backtesting this could show us our transitions
        /// </summary>
        private StateList _stateCache = new StateList();

        public bool IsDirty { get; set; } = false;

        public SymbolProfile(Symbol symbol, StateBase state)
        {
            Symbol = symbol;
            State = state;
        }

        public SymbolProfile() { }

        /// <summary>
        /// Transition the profile to a new state, keeping the old state
        /// cached and marking the profile dirty. Dirty profiles cannot be
        /// transitioned again, preventing a symbol from triggering a second
        /// action
        /// </summary>
        /// <param name="nextState"></param>
        public void TransitionState(StateBase nextState)
        {
            _stateCache.Add(State.Clone());
            State = nextState;
            IsDirty = true;
        }

        public override string ToString()
        {
            return string.Format($"SymbolProfile: ({Symbol.Token}) {Symbol.Name}, CurrentState: {State}");
        }

        public bool IdenfifiesAs(SymbolProfile item)
        {
            if (item.Symbol != Symbol) return false;
            if (item.State != State) return false;
            return true;
        }

        public bool IsRelatedBySetup(Setup setup)
        {
            return State.Equals(setup.OriginState);
        }
    }
}
