using System;
using System.Collections.Generic;
using StockBox.Interpreter;
using StockBox.Interpreter.Expressions;
using StockBox.Services;
using StockBox.Validation;

namespace StockBox.Rules
{
    public class RuleList : List<Rule>
    {
        private readonly ValidationResultList _results = new ValidationResultList();

        public bool Success { get { return _results.Success; } }
        public bool HasFailures { get { return _results.HasFailures; } }

        /// <summary>
        /// The RuleList contains an aggregated list of the Statements > Expr
        /// so the Expressions can be analyzed prior to Evaluation
        /// </summary>
        public List<Expr> Expressions { get; set; } = new List<Expr>();

        /// <summary>
        /// This method runs the aggregated Expression through the interpreter
        /// to finalize evaluation. Separating this execution from the Rules
        /// being evaluated directly allows us to analyze the expression list
        /// before evaluation, allowing the application to pull a proper dataset
        /// 
        /// </summary>
        /// <param name="interpreter"></param>
        /// <returns></returns>
        public ValidationResultList Evalute(SbInterpreter interpreter)
        {
            var ret = new ValidationResultList();
            foreach (var e in Expressions)
            {
                var thisResult = EResult.eFail;
                var exprResult = interpreter.Interpret(e) as bool?;
                if (exprResult != null)
                    thisResult = (bool)exprResult ? EResult.eSuccess : EResult.eFail;
                ret.Add(new ValidationResult(thisResult, e.Statement, e));
            }
            return ret;
        }

        public RuleList()
        {
        }

        public RuleList(List<Rule> source)
        {
            AddRange(source);
        }

        public ValidationResultList ProcessRules(ISbService service)
        {
            _results.AddRange(service.ProcessRules(this));
            return _results;
        }

        public void AddExpr(Expr e, string statement)
        {
            e.Statement = statement;
            Expressions.Add(e);
        }

        public bool ContainsItem(Rule item)
        {
            foreach (Rule r in this)
                if (r.IdentifiesAs(item))
                    return true;
            return false;
        }
    }
}
