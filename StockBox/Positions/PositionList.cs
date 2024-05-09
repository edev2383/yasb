using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Base.Utilities;

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
            ret.AverageAmountPerWon = (double)ret.TotalWinningDollars / (double)ret.TotalWinningPositions;
            ret.AverageAmountPerLoss = (double)ret.TotalLosingDollars / (double)ret.TotalLosingPositions;
            ret.WinPercent = (double)ret.TotalWinningPositions / (double)ret.TotalNumberOfPositions * 100;
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

        public string CreateConsoleDump()
        {
            var sb = new StringBuilder();

            sb.Add("|Symbol".PadLeft(6));
            sb.Add("Open Bal.".PadLeft(12));
            sb.Add("Open Date".PadLeft(22));
            sb.Add("Entry Price".PadLeft(12));
            sb.Add("Close Date".PadLeft(22));
            sb.Add("Open Len.".PadLeft(12));
            sb.Add("Exit Price".PadLeft(12));
            sb.Add("Share Diff".PadLeft(12));
            sb.Add("Ttl Shares".PadLeft(12));
            sb.Add("Total P&L".PadLeft(12));
            sb.Add("RiskExit?".PadLeft(12));
            sb.Add("\r\n");

            foreach (var item in this)
            {
                // symbol
                sb.Add(item.Symbol.Name.PadLeft(6));
                // open balance
                sb.Add(Math.Round(Convert.ToDecimal(item.StartingBalance), 2).ToString().PadLeft(12));
                // open date
                sb.Add(item.Transactions.First().Timestamp.Date.ToString().PadLeft(22));
                // entry price
                sb.Add(Math.Round(item.EntryPrice, 2).ToString().PadLeft(12));
                // close date
                sb.Add(item.Transactions.Count > 1 ? item.Transactions[1].Timestamp.Date.ToString().PadLeft(22) : string.Empty.PadLeft(22));
                // Days Active
                sb.Add(item.ActiveLength.ToString().PadLeft(12));
                // exit price
                sb.Add(Math.Round(item.CurrentPrice, 2).ToString().PadLeft(12));
                // price diff
                sb.Add(Math.Round(item.ShareDiff, 2).ToString().PadLeft(12));
                // share count
                sb.Add(item.TotalShares.ToString().PadLeft(12));
                // P&L
                sb.Add(Math.Round(item.ProfitLoss, 2).ToString().PadLeft(12));
                // RiskExit?
                sb.Add(item.RiskExitPerformed.ToString().PadLeft(12));
                sb.Add("\r\n");
            }

            Console.WriteLine(sb.Build('|'));
            return sb.Build('|');
        }
    }
}
