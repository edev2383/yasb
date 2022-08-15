using System;
using System.Collections.Generic;
using StockBox.Services;
using StockBox.Validation;

namespace StockBox.Rules
{
    public class RuleList : List<Rule>
    {
        private readonly ValidationResultList _results = new ValidationResultList();

        public bool Success { get { return _results.Success; } }
        public bool HasFailures { get { return _results.HasFailures; } }

        public RuleList()
        {
        }

        public RuleList(List<Rule> source)
        {
            AddRange(source);
        }

        public ValidationResultList Process(ISbService service)
        {
            _results.AddRange(service.Process(this));
            return _results;
        }
    }
}
