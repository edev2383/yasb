using System;
using StockBox.Models;
using StockBox.RiskProfiles;

namespace StockBox.Positions
{

    /// <summary>
    /// Class <c>Position</c> models an open investment of a stock.
    /// </summary>
    public class Position
    {

        public SymbolProfile Symbol { get { return _symbol; } }
        public TransactionList Transactions { get { return _transactions; } }
        public RiskProfile RiskProfile { get { return _riskProfile; } }
        public int? TotalShares { get; set; }
        // this value can differ from TotalShares when the user opts to sell
        // half their stake at a given target
        public int? ActiveShares { get; set; }
        public double? TotalDollars { get; set; }
        public double? EntryPrice { get; set; }
        public double? CurrentPrice { get; set; }
        public double? ProfitLoss { get { return CalculateProfitLoss(); } }

        public int? PositionId { get; set; }
        public Guid? Token { get; set; }

        private TransactionList _transactions = new TransactionList();
        private SymbolProfile _symbol;
        private RiskProfile _riskProfile;

        public Position()
        {
        }

        public double CalculateProfitLoss()
        {
            return 0;
        }
    }
}
