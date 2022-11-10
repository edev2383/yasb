using System;
using StockBox.Rules;


namespace StockBox_TestArtifacts.Builders.StockBox.Rules
{

    /// <summary>
    /// 
    /// </summary>
    public class RuleBuilder
    {
        private string _statement;

        public RuleBuilder WithStatement(string s)
        {
            _statement = s;
            return this;
        }

        public Rule Build()
        {
            return new Rule(_statement);
        }
    }
}
