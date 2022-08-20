using System;
using StockBox.Rules;
using StockBox.Validation;

namespace StockBox.Services
{
    public abstract class SbServiceBase : ISbService
    {
        public SbServiceBase()
        {
        }

        public abstract ValidationResultList ProcessRules(RuleList rules);
    }
}
