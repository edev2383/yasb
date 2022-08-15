using System;
using StockBox.Rules;
using StockBox.Validation;

namespace StockBox.Services
{
    /// <summary>
    /// The Service interface defines the forward and backward testing services
    /// that serve as the entry point for Setups/Rules/Interpreters, etc. 
    /// </summary>
    public interface ISbService
    {

        ValidationResultList Process(RuleList rules);
    }
}
