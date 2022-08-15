using System;
using StockBox.Rules;
using StockBox.Validation;

namespace StockBox.Services
{
    /// <summary>
    /// BacktestService is used to test historical data against Setups/Rules,
    /// etc.
    /// </summary>
    public class BacktestService : SbServiceBase
    {
        public BacktestService()
        {
        }

        public override ValidationResultList Process(RuleList rules)
        {
            throw new NotImplementedException();
        }
    }
}
