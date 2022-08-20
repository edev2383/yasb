using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Interpreter;
using StockBox.Interpreter.Scanner;
using StockBox.Rules;
using StockBox.Services;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Rules_Tests
    {
        [TestMethod]
        public void SB_Rules_01_RuleCanBeCreated()
        {
            var rule = new Rule("");
            Assert.IsNotNull(rule);
        }

        [TestMethod]
        public void SB_Rules_02_CanProcessSimpleRuleSets_ExpectTrue()
        {
            var rulelist = new RuleList();
            rulelist.Add(new Rule("5 < 10"));
            rulelist.Add(new Rule("15 > 10"));

            var scanner = new Scanner();
            var parser = new Parser();
            var interpreter = new SbInterpreter();

            var service = new ActiveService(scanner, parser);

            service.ProcessRules(rulelist);
            var results = rulelist.Evalute(interpreter);

            Assert.IsTrue(results.Success);
        }

        [TestMethod]
        public void SB_Rules_03_CanProcessSimpleRuleSets_ExpectFalse()
        {
            var rulelist = new RuleList();
            rulelist.Add(new Rule("5 > 10"));
            rulelist.Add(new Rule("15 > 10"));

            var scanner = new Scanner();
            var parser = new Parser();
            var interpreter = new SbInterpreter();

            var service = new ActiveService(scanner, parser);

            service.ProcessRules(rulelist);
            var results = rulelist.Evalute(interpreter);

            Assert.IsFalse(results.Success);
        }

        [TestMethod]
        public void SB_Rules_04_CanProcessSimpleRuleSets_ExpectTrue()
        {
            var rulelist = new RuleList
            {
                new Rule("5 < 10"),
                new Rule("15 > 10"),
                new Rule("15 != 10"),
                new Rule("15 == (10 + 5)")
            };

            var service = new ActiveService(new Scanner(), new Parser());

            service.ProcessRules(rulelist);

            // this results list will be an aggregate of all scanner and parser
            // actions
            var results = service.GetResults();

            var exprResults = rulelist.Evalute(new SbInterpreter());

            Assert.AreEqual(rulelist.Expressions.Count, 4);
            Assert.IsTrue(exprResults.Success);
        }

        [TestMethod]
        public void SB_Rules_05_CanProcessSimpleRuleSets_ExpectFalse()
        {
            var rulelist = new RuleList
            {
                new Rule("5 < 10"),
                new Rule("15 > 10"),
                new Rule("15 != 10"),
                new Rule("15 != (10 + 5)")
            };

            var service = new ActiveService(new Scanner(), new Parser());

            service.ProcessRules(rulelist);

            // this results list will be an aggregate of all scanner and parser
            // actions
            var results = service.GetResults();

            var exprResults = rulelist.Evalute(new SbInterpreter());

            Assert.AreEqual(rulelist.Expressions.Count, 4);
            Assert.IsTrue(exprResults.HasFailures);
        }
    }
}
