using System;


namespace StockBox.Positions
{

    /// <summary>
    /// 
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

        public PositionSummary()
        {
        }
    }
}
