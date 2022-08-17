using System;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
using StockBox.Rules;
using StockBox.Validation;


namespace StockBox.Services
{

    /// <summary>
    /// ActiveService is the forward looking testing service use to track
    /// the states of YTD setups
    /// </summary>
    public class ActiveService : SbServiceBase
    {

        private Scanner _scanner;
        private Parser _parser;
        private SbInterpreter _interperter;

        /// <summary>
        /// Results obj will aggregate the results of all scanners, parsers,
        /// and interpreters for an full result history of each execution
        /// </summary>
        private ValidationResultList _results = new ValidationResultList();

        public ActiveService(Scanner scanner, Parser parser, SbInterpreter interpreter)
        {
            _scanner = scanner;
            _parser = parser;
            _interperter = interpreter;
        }


        /// <summary>
        /// Iterate through the rules in the RuleList and evaluate the statement
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public override ValidationResultList Process(RuleList rules)
        {
            var ret = new ValidationResultList();

            foreach (Rule rule in rules)
            {
                var tokens = _scanner.ScanTokens(rule.Statement);
                var expression = _parser.Parse(tokens);
                var result = (bool)_interperter.Interpret(expression);

                // add the results of the expression eval to the return object
                ret.Add(new ValidationResult(result, rule.Statement));

            }

            // add process results to the service results object
            _results.AddRange(_scanner.GetResults());
            _results.AddRange(_parser.GetResults());
            _results.AddRange(_interperter.GetResults());

            return ret;
        }
    }
}
