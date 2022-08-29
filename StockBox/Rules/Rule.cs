using System;
using StockBox.Validation;

namespace StockBox.Rules
{
    /// <summary>
    /// A wrapper around a rule statement. Statement is a string representing
    /// an expression that describes a state of a stock chart scenario, i.e.,
    /// "Close > Open", "2 days ago High > High", "SMA(25) > SMA(5)". A
    /// collection of Rules can describe complex states and resolve to a simple
    /// series of booleans to make decisions.
    ///
    /// The statement will be later scanned and parsed by the interpreter using
    /// a Deedle.Frame of stock data
    /// </summary>
    public class Rule
    {
        public string Statement { get { return _statement; } }
        private readonly string _statement;

        public Rule(string statement)
        {
            _statement = statement;
        }

        public bool IdentifiesAs(Rule item)
        {
            if (item.Statement != Statement) return false;
            return true;
        }

        public Rule Clone()
        {
            return new Rule(_statement);
        }
    }
}
