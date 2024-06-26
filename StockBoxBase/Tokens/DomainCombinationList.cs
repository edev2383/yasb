﻿using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Associations;
using StockBox.Validation;


namespace StockBox.Base.Tokens
{

    /// <summary>
    /// 
    /// </summary>
    public class DomainCombinationList : List<DomainCombination>, IDomainCombinationsProvider
    {

        public DomainCombinationList()
        {
        }

        public IDomainCombinationsProvider GetDailyDomainCombos()
        {
            return GetDomainCombosByInterval(TokenType.eDaily);
        }

        public IDomainCombinationsProvider GetWeeklyDomainCombos()
        {
            return GetDomainCombosByInterval(TokenType.eWeekly);
        }

        public IDomainCombinationsProvider GetMonthyDomainCombos()
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
        /// Compress a combinationlist down to the unique combos of keyword and
        /// indices
        /// 
        /// </summary>
        /// <returns></returns>
        public DomainCombinationList GetUniqueDomainCombos()
        {
            if (IsHomogenousGroup().HasFailures)
                throw new Exception("The DomainCombinationList being queried contains more than one type of IntervalFrequency Token. Before calling `GetUniqueDomainCombos`, request a specific frequency subset of data, i.e., GetDailyDomainCombos()");

            var ret = new DomainCombinationList();
            foreach (var item in this)
                if (!ret.ContainsComparableDomainKeywordWithMatchingIndex(item))
                    ret.Add(item);
            return ret;
        }

        /// <summary>
        /// Return all unqiue domain combos that are of indicator type 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DomainCombinationList GetIndicatorsByInterval(TokenType type)
        {
            return GetDomainCombosByInterval(type).GetUniqueDomainCombos().GetIndicators();
        }

        /// <summary>
        /// Return all unique domain columns that are NOT indicators
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DomainCombinationList GetDomainColumnsByInterval(TokenType type)
        {
            return GetDomainCombosByInterval(type).GetUniqueDomainCombos().GetDomainColumns();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DomainCombinationList GetDomainColumns()
        {
            if (IsHomogenousGroup().HasFailures)
                throw new Exception("The DomainCombinationList being queried contains more than one type of IntervalFrequency Token. Before calling `GetDomainColumns`, request a specific frequency subset of data, i.e., GetDailyDomainCombos()");

            var ret = new DomainCombinationList();
            foreach (var item in this)
                if (!item.IsIndicator)
                    ret.Add(item);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DomainCombinationList GetIndicators()
        {
            if (IsHomogenousGroup().HasFailures)
                throw new Exception("The DomainCombinationList being queried contains more than one type of IntervalFrequency Token. Before calling `GetIndicators`, request a specific frequency subset of data, i.e., GetDailyDomainCombos()");

            var ret = new DomainCombinationList();
            foreach (var item in this)
                if (item.IsIndicator)
                    ret.Add(item);
            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double GetMaxIndex()
        {
            if (IsHomogenousGroup().HasFailures)
                throw new Exception("The DomainCombinationList being queried contains more than one type of IntervalFrequency Token. Before calling `GetIndicators`, request a specific frequency subset of data, i.e., GetDailyDomainCombos()");

            return this.Select(x => x.IntervalIndex).Max();
        }

        public double GetMaxIndicatorIndex()
        {
            if (IsHomogenousGroup().HasFailures)
                throw new Exception("The DomainCombinationList being queried contains more than one type of IntervalFrequency Token. Before calling `GetIndicators`, request a specific frequency subset of data, i.e., GetDailyDomainCombos()");
            var grp = new List<int>();
            foreach (var item in GetIndicators())
                grp.AddRange(item.Indices);
            return grp.Count > 0 ? (double)grp.Max() : 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ContainsItem(DomainCombination item)
        {
            foreach (var dc in this)
                if (dc.IdentifiesAs(item))
                    return true;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ContainsComparableDomainKeywordWithMatchingIndex(DomainCombination item)
        {
            foreach (var dc in this)
                if (item.IsMatchMinusIntervalIndex(dc)) return true;
            return false;
        }

        /// <summary>
        /// Checks to make sure that the group being tested only contains a
        /// single type of IntervalFrequency
        /// </summary>
        /// <returns></returns>
        private ValidationResultList IsHomogenousGroup()
        {
            var ret = new ValidationResultList();

            if (this.Count == 0) return ret;
            var type = this.First().IntervalFrequency.Type;
            foreach (var item in this)
                ret.Add(new ValidationResult(item.IntervalFrequency.Type == type, "Item is a homogenous match.", item));

            return ret;
        }

        /// <summary>
        /// Return a subset list comprised only of type DomainToken, i.e.,
        /// @52WeekHigh, @52WeekLow, @AllTimeHigh, @AllTimeLow.
        /// </summary>
        /// <returns></returns>
        public IDomainCombinationsProvider GetDomainTokens()
        {
            var ret = new DomainCombinationList();
            foreach (var combo in this)
                if (combo.IsDomainToken())
                    ret.Add(combo);
            return ret;
        }
    }
}
