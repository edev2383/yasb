using StockBox.States;


namespace StockBox.Models
{
    public class SymbolProfile
    {
        public Symbol Symbol { get; set; }
        public StateBase State { get; set; }

        public SymbolProfile(Symbol symbol, StateBase state)
        {
            Symbol = symbol;
            State = state;
        }

        public SymbolProfile() { }

        public override string ToString()
        {
            return string.Format($"SymbolProfile: ({Symbol.SymbolId}) {Symbol.Name}, CurrentState: {State}");
        }

        public bool IdenfifiesAs(SymbolProfile item)
        {
            if (item.Symbol != Symbol) return false;
            if (item.State != State) return false;
            return true;
        }
    }
}
