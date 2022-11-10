using System;
using System.Collections.Generic;
using StockBox.Rules;

namespace StockBox_TestArtifacts.Builders.StockBox.Rules
{

    /// <summary>
    /// 
    /// </summary>
    public class PatternBuilder
    {
        private List<Rule> _rules = new List<Rule>();
        private int _id = 1;
        public PatternBuilder()
        {
        }

        public PatternBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public PatternBuilder WithStatement(string s)
        {
            return WithRule(new Rule(s));
        }

        public PatternBuilder WithRule(Rule r)
        {
            _rules.Add(r);
            return this;
        }

        public Pattern Build()
        {
            return new Pattern(_id, _rules);
        }
    }
}
