using System;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
using StockBox.Rules;
using StockBox.Validation;


namespace StockBox.Services
{

    /// <summary>
    /// Class <c>ActiveService</c> accepts a Scanner and Parser to reduce a
    /// RuleList down to a series of expressions.
    /// 
    /// Note: Use of this class has changed since original inception. It was
    /// originally going to scan, parse, and evaluate the statements, however,
    /// it began to be clear that it was doing too much, and attempting to get
    /// all of the related behavior in this one class was not wise.
    ///
    /// In addition, we introduce the analyzing step between Parsing and
    /// Evaluation, which further complicated the original intent.
    ///
    /// It NOW merely acts as a results repository for the scanning/parsing
    /// process of a given RuleSet. The DI of the Scanner/Parser allows us to
    /// extend the Scanners/Parsers as other needs arise, i.e., we see a future
    /// need for a parser that evalutes to different expressions when the data
    /// target is a database, rather than a scraped API. 
    ///
    /// TODO: Rename this class to something more appropriate
    /// </summary>
    public class ActiveService : SbServiceBase, IValidationResultsListProvider
    {

        private Scanner _scanner;
        private Parser _parser;

        /// <summary>
        /// Results obj will aggregate the results of all scanners, parsers,
        /// and interpreters for an full result history of each execution
        /// </summary>
        private ValidationResultList _results = new ValidationResultList();

        public ActiveService(Scanner scanner, Parser parser)
        {
            _scanner = scanner;
            _parser = parser;
        }

        public ValidationResultList GetResults()
        {
            return _results;
        }

        /// <summary>
        /// Iterate through the rules in the RuleList and parse the statement
        /// into an expression. The resultant expression is added to the
        /// RuleList where it can be analyzed and then interpretted
        /// </summary>
        /// <param name="rules"></param>
        /// <returns></returns>
        public override ValidationResultList ProcessRules(RuleList rules)
        {
            var ret = new ValidationResultList();

            foreach (Rule rule in rules)
            {
                var tokens = _scanner.ScanTokens(rule.Statement);
                rules.AddExpr(_parser.Parse(tokens), rule.Statement);
                // rules.AddStmts(_parser.ParseStatements(tokens), rule.Statement);
            }

            // add process results to the service results object
            _results.AddRange(_scanner.GetResults());
            _results.AddRange(_parser.GetResults());
            return ret;
        }
    }
}
