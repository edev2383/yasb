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

        public SymbolProfile(Symbol symbol, StateBase state)
        {
            Symbol = symbol;
            State = state;
        }

        public SymbolProfile() { }

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
            return Equals(setup.OriginState);
        }
    }
}
