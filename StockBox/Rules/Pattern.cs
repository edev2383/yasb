using System;
using System.Collections.Generic;

namespace StockBox.Rules
{

    /// <summary>
    /// Class <c>Pattern</c> is a domain-specific data model for organzing rules
    /// </summary>
    public class Pattern : RuleList
    {

        public int? PatternId { get; set; }
        public RuleList Rules { get; set; }

        public Pattern() { }

        public Pattern(int? patternId, List<Rule> source) : base(source)
        {
            PatternId = patternId;
        }

        public Pattern Clone()
        {
            return new Pattern(PatternId, this);
        }
    }
}
