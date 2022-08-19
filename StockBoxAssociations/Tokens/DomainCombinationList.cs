using System;
using System.Collections.Generic;
using System.Linq;

namespace StockBox.Associations.Tokens
{

    public class DomainCombinationList : List<DomainCombination>
    {

        public DomainCombinationList()
        {
        }

        public DomainCombinationList GetDailyDomainCombos()
        {
            var ret = new DomainCombinationList();
            foreach (var combo in this)
                if (combo.IntervalFrequency.Type == TokenType.eDaily)
                    ret.Add(combo);
            return ret;
        }

        public DomainCombinationList GetWeeklyDomainCombos()
        {
            var ret = new DomainCombinationList();
            foreach (var combo in this)
                if (combo.IntervalFrequency.Type == TokenType.eWeekly)
                    ret.Add(combo);
            return ret;
        }

        public DomainCombinationList GetMonthyDomainCombos()
        {
            var ret = new DomainCombinationList();
            foreach (var combo in this)
                if (combo.IntervalFrequency.Type == TokenType.eMonthly)
                    ret.Add(combo);
            return ret;
        }

        public double GetMaxIndex()
        {
            if (this.Count == 0) return 0;
            return this.Select(x => x.IntervalIndex).Max();
        }
    }
}
