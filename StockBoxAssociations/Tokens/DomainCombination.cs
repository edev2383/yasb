using System;
using System.Linq;

namespace StockBox.Associations.Tokens
{

    /// <summary>
    /// A domain combination is a way to describe the relevance of a domain
    /// expression.
    /// </summary>
    public class DomainCombination
    {

        /// <summary>
        /// Target IndexFromZero-Origin. We are interested in the max value here
        /// coupled w/ the IntervalFrequency to make sure that the right type
        /// and length of data is queried. Example: IntervalIndex of 3 with an
        /// IntervalFrequency of eWeekly, means we need to make sure that we
        /// have at least 3 weeks of Weekly type data in the adapter
        /// </summary>
        public double IntervalIndex { get; set; }

        /// <summary>
        /// Frequency of data, i.e., eDaily, eWeekly, eMonthly. This could
        /// expand eventually to include shorter intervales, i.e., hour/minute
        /// </summary>
        public Token IntervalFrequency { get; set; }

        /// <summary>
        /// Column key, i.e., Close, Open, SMA, etc
        /// </summary>
        public string DomainKeyword { get; set; }

        /// <summary>
        /// Colum related indicies, i.e., SMA(25), indicies is int[]{25}
        /// </summary>
        public int[] Indices { get; set; }

        public bool IsIndicator { get { return IsKeywordAnIndicator(); } }

        public DomainCombination(object intervalIndex, Token intervalFrequency, string domainKeyword, int[] indices = null)
        {
            double.TryParse(intervalIndex.ToString(), out double result);
            IntervalIndex = result;
            IntervalFrequency = intervalFrequency;
            DomainKeyword = domainKeyword;
            Indices = indices;
        }

        public DomainCombination(DomainCombination source)
        {
            IntervalIndex = source.IntervalIndex;
            IntervalFrequency = source.IntervalFrequency;
            DomainKeyword = source.DomainKeyword;
            Indices = source.Indices;
        }

        public DomainCombination Clone()
        {
            return new DomainCombination(this);
        }

        private bool IsKeywordAnIndicator()
        {
            bool ret = false;

            switch (DomainKeyword.ToLower())
            {
                case "sma": ret = true; break;
                default: break;
            }
            return ret;
        }

        public bool IdentifiesAs(DomainCombination item)
        {
            if (IntervalIndex != item.IntervalIndex) return false;
            if (IntervalFrequency != item.IntervalFrequency) return false;
            if (DomainKeyword != item.DomainKeyword) return false;
            if (Indices == null && item.Indices == null) return true;
            if (Indices == null) return false;
            foreach (int i in Indices)
                if (!item.Indices.Contains(i)) return false;
            return true;
        }

        public bool IsMatchMinusIntervalIndex(DomainCombination item)
        {
            if (DomainKeyword != item.DomainKeyword) return false;
            // if both are null, we can guard out w/ a true
            if (Indices == null && item.Indices == null) return true;
            if (Indices == null) return false;
            if (Indices.Count() != item.Indices.Count()) return false;
            if (Indices.Intersect(item.Indices).Count() != item.Indices.Count()) return false;
            return true;
        }
    }
}
