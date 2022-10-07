using System;
using System.Collections.Generic;


namespace StockBox.Positions
{

    /// <summary>
    /// 
    /// </summary>
    public class PositionList : List<Position>
    {

        public PositionList()
        {
        }

        public Position GetCurrentPosition()
        {
            Position ret = null;
            foreach (var item in this)
                if (item.IsOpen)
                    return item;
            return ret;
        }

        public double GetTotalDollars()
        {
            double ret = 0;
            foreach (var item in this)
                ret += item.CalculateProfitLoss();
            return ret;
        }

        public PositionSummary CreateSummary()
        {
            var ret = new PositionSummary();
            ret.WinningPositions = GetWinningPositions();
            ret.LosingPositions = GetLosingPositions();
            ret.RiskExitedPositions = GetRiskExitedPositions();
            ret.TotalNumberOfPositions = this.Count;
            ret.TotalWinningPositions = ret.WinningPositions.Count;
            ret.TotalLosingPositions = ret.LosingPositions.Count;
            ret.TotalWinningDollars = ret.WinningPositions.GetTotalDollars();
            ret.TotalProfitLoss = GetTotalDollars();
            ret.TotalLosingDollars = ret.LosingPositions.GetTotalDollars();
            ret.AverageAmountPerWon = ((double)ret.TotalWinningDollars / (double)ret.TotalWinningPositions);
            ret.AverageAmountPerLoss = ((double)ret.TotalLosingDollars / (double)ret.TotalLosingPositions);
            ret.WinPercent = ((double)ret.TotalWinningPositions / (double)ret.TotalNumberOfPositions) * 100;
            return ret;
        }

        public PositionList GetWinningPositions()
        {
            var ret = new PositionList();
            foreach (var item in this)
                if (!item.IsOpen && item.CalculateProfitLoss() >= 0)
                    ret.Add(item);
            return ret;
        }

        public PositionList GetLosingPositions()
        {
            var ret = new PositionList();
            foreach (var item in this)
                if (!item.IsOpen && item.CalculateProfitLoss() < 0)
                    ret.Add(item);
            return ret;
        }

        public PositionList GetRiskExitedPositions()
        {
            var ret = new PositionList();
            foreach (var item in this)
                if (item.RiskExitPerformed)
                    ret.Add(item);
            return ret;
        }
    }
}
