using System;


namespace StockBox.Positions
{

    /// <summary>
    /// Class <c>PositionSummary</c> is a data BDO wrapper around aggregated
    /// Position data
    /// </summary>
    public class PositionSummary
    {
        public int TotalNumberOfPositions { get; set; }
        public int TotalWinningPositions { get; set; }
        public int TotalLosingPositions { get; set; }

        public double TotalProfitLoss { get; set; }
        public double TotalWinningDollars { get; set; }
        public double TotalLosingDollars { get; set; }

        public double AverageAmountPerWon { get; set; }
        public double AverageAmountPerLoss { get; set; }

        public double WinPercent { get; set; }

        public PositionList WinningPositions { get; set; }
        public PositionList LosingPositions { get; set; }
        public PositionList RiskExitedPositions { get; set; }

        /// <summary>
        /// Entry Price * Share Count Aggr(All Positions)
        /// </summary>
        public double TotalAmountRisked { get; set; }

        /// <summary>
        /// Total Profit / TotalAmountRisked
        /// </summary>
        public double ProfitPerDollarRisked { get; set; }

        public PositionSummary()
        {
        }
    }
}
