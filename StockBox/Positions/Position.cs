using System;
using System.Collections.Generic;
using StockBox.Associations;
using StockBox.Base.Types;
using StockBox.Data.SbFrames;
using StockBox.Models;
using StockBox.RiskProfiles;

namespace StockBox.Positions
{

    /// <summary>
    /// Class <c>Position</c> models an open investment of a stock.
    /// </summary>
    public class Position : IPosition
    {

        public ISymbolProvider Symbol { get { return _symbol; } }
        public TransactionList Transactions { get { return _transactions; } }
        public RiskProfile RiskProfile { get { return _riskProfile; } }

        public bool IsOpen { get { return _transactions.HasOpenTransaction(); } }

        public bool RiskExitPerformed { get; set; } = false;
        public int TotalShares { get; set; }
        // this value can differ from TotalShares when the user opts to sell
        // a portion their stake at a given target
        public int? ActiveShares { get; set; }
        public double? TotalDollars { get; set; }
        public double EntryPrice { get; set; }
        public double CurrentPrice { get; set; }

        public OptionList<DateTime> EntryDates { get; set; } = new OptionList<DateTime>();
        public DateTime? EntryDate
        {
            get
            {
                if (EntryDates == null)
                    EntryDates = new OptionList<DateTime>();
                var date = EntryDates.First();
                if (date is Some<DateTime>) return ((Some<DateTime>)date).Value;
                return null;
            }
        }

        public OptionList<DateTime> ExitDates { get; set; } = new OptionList<DateTime>();
        public DateTime? ExitDate
        {
            get
            {
                if (ExitDates == null)
                    ExitDates = new OptionList<DateTime>();
                var date = ExitDates.First();
                if (date is Some<DateTime>) return ((Some<DateTime>)date).Value;
                return null;
            }
        }

        public double ActiveLength
        {
            get
            {
                DateTime exit;
                if (ExitDate == null)
                    exit = DateTime.Now;
                else
                    exit = (DateTime)ExitDate;
                return (exit - (DateTime)EntryDate).TotalDays;
            }
        }

        public double ShareDiff
        {
            get
            {
                if (CurrentPrice == 0) return 0;
                return CurrentPrice - EntryPrice;
            }
        }

        public double ProfitLoss { get { return CalculateProfitLoss(); } }

        public int? PositionId { get; set; }
        public Guid? Token { get { return _token; } }

        private TransactionList _transactions = new TransactionList();
        private ISymbolProvider _symbol;
        private RiskProfile _riskProfile;
        private Guid _token;

        public Position(Guid? token, ISymbolProvider symbol)
        {
            _token = token != null ? (Guid)token : Guid.NewGuid();
            _symbol = symbol;
        }

        public double CalculateOriginalInvestment()
        {
            return TotalShares * EntryPrice;
        }

        public double CalculateProfitLoss(DataPoint dataPoint)
        {
            CurrentPrice = dataPoint.Close;
            return CalculateProfitLoss();
        }

        public double CalculateProfitLoss()
        {
            return ShareDiff * TotalShares;
        }

        public void AddBuy(Transaction transaction)
        {
            TotalShares = transaction.ShareCount != null ? (int)transaction.ShareCount : 0;
            ActiveShares = transaction.ShareCount;
            EntryPrice = transaction.SharePrice;
            EntryDates.Add(transaction.Timestamp);
            transaction.PositionToken = _token;
            _transactions.Add(transaction);
        }

        public void AddSell(Transaction transaction)
        {
            ActiveShares -= transaction.ShareCount;
            CurrentPrice = transaction.SharePrice;
            ExitDates.Add(transaction.Timestamp);
            transaction.PositionToken = _token;
            _transactions.Add(transaction);
            if (ActiveShares <= 0)
            {
                // do closing activities here...
            }
        }
    }
}
