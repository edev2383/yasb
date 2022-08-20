using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Validation;


namespace StockBox.Associations.Tokens
{

    /// <summary>
    /// 
    /// </summary>
    public class DomainCombinationList : List<DomainCombination>
    {

        public DomainCombinationList()
        {
        }

        public DomainCombinationList GetDailyDomainCombos()
        {
            return GetDomainCombosByInterval(TokenType.eDaily);
        }

        public DomainCombinationList GetWeeklyDomainCombos()
        {
            return GetDomainCombosByInterval(TokenType.eWeekly);
        }

        public DomainCombinationList GetMonthyDomainCombos()
        {
            return GetDomainCombosByInterval(TokenType.eMonthly);
        }

        private DomainCombinationList GetDomainCombosByInterval(TokenType type)
        {
            var ret = new DomainCombinationList();
            foreach (var combo in this)
                if (combo.IntervalFrequency.Type == type)
                    ret.Add(combo);
            return ret;
        }

        /// <summary>
        /// Return the max index value to inform the request length of any
        /// data queries
        /// </summary>
        /// <returns></returns>
        public double GetMaxIndex()
        {
            if (this.Count == 0) return 0;

            var vr = IsHomogenousGroup();
            if (vr.HasFailures)
                throw new Exception("The DomainCombinationList being queried contains more than one type of IntervalFrequency Token. Before calling `GetMaxIndex`, request a specific frequency subset of data, i.e., GetDailyDomainCombos()");

            return this.Select(x => x.IntervalIndex).Max();
        }

        /// <summary>
        /// Return the max indicator index to inform the request length of any
        /// data queries
        /// </summary>
        /// <returns></returns>
        public int GetMaxIndicatorIndex()
        {
            if (this.Count == 0) return 0;

            var vr = IsHomogenousGroup();
            if (vr.HasFailures)
                throw new Exception("The DomainCombinationList being queried contains more than one type of IntervalFrequency Token. Before calling `GetMaxIndicatorIndex`, request a specific frequency subset of data, i.e., GetDailyDomainCombos()");

            var foundInts = new List<int>();
            foreach (var combo in this)
                if (combo.Indices != null)
                    foundInts.AddRange(combo.Indices);
            return foundInts.Max();
        }

        /// <summary>
        /// Checks to make sure that the group being tested only contains a
        /// single type of IntervalFrequency
        /// </summary>
        /// <returns></returns>
        private ValidationResultList IsHomogenousGroup()
        {
            var ret = new ValidationResultList();

            // this isn't exactly null safe, but this method should only be
            // called AFTER the calling method performs a zero check on the
            // list.Count prop
            ret.Add(this.Count > 0, "List contains at least one DomainCombination", this.FirstOrDefault());

            var first = this.First();

            foreach (var item in this)
                ret.Add(new ValidationResult(item.IntervalFrequency.IsOfSameType(first.IntervalFrequency), "Item is a homogenous match.", item));

            return ret;
        }
    }
}
