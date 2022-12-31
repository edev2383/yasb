using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Associations.Tokens;
using StockBox.Interpreter.Expressions;
using StockBox.Interpreter.Scanner;
using StockBox.Rules;
using StockBox.Services;
using StockBox_UnitTests.Accessors;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Expr_Tests
    {

        [TestMethod, Description("Analyzer returns expected number of DomainCombinations")]
        public void SB_Expr_01_ExpressionAnalyzer()
        {
            var src = "SMA(25)";
            var scanner = new Scanner(src);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var expr = parser.Parse();

            var exprAnalyzer = new ExpressionAnalyzer(expr);

            exprAnalyzer.Scan();

            Assert.IsTrue(exprAnalyzer.Combos.Count == 1);
        }

        [TestMethod, Description("Analyzer returns expected number of DomainCombinations")]
        public void SB_Expr_02_ExpressionAnalyzer()
        {
            var src = "SMA(25) > 2 Weeks Ago Close";
            var scanner = new Scanner(src);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var expr = parser.Parse();

            var exprAnalyzer = new ExpressionAnalyzer(expr);

            exprAnalyzer.Scan();

            Assert.IsTrue(exprAnalyzer.Combos.Count == 2);
        }

        [TestMethod, Description("Expected Exception thrown when querying a non-subset of data")]
        [ExpectedException(typeof(Exception), "CombinationList queried before subset requested")]
        public void SB_Expr_03_DomainCombinationListThrowsExpectedException()
        {
            var dcl = new DomainCombinationList
            {
                new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }),
                new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close")
            };

            dcl.GetIndicators();
        }

        [TestMethod, Description("Ensure the combolist subset returns the expected primary index")]
        public void SB_Expr_04_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList
            {
                new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }),
                new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close")
            };

            var daily = dcl.GetDailyDomainCombos() as DomainCombinationList;
            var maxIndex = daily.GetMaxIndex();

            Assert.AreEqual(daily.First().IntervalFrequency.Type, TokenType.eDaily);
            Assert.AreEqual(4, maxIndex);
        }

        [TestMethod, Description("Ensure the combolist subset returns the expected indicator index")]
        public void SB_Expr_05_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList
            {
                new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }),
                new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close")
            };

            var indicators = dcl.GetIndicatorsByInterval(TokenType.eDaily);

            Assert.AreEqual(indicators.First().IntervalFrequency.Type, TokenType.eDaily);
            Assert.AreEqual(25, indicators.First().Indices.First());
        }

        [TestMethod, Description("Ensure the proper subset is pulled from the primary list")]
        public void SB_Expr_06_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList
            {
                new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }),
                new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close")
            };

            var weekly = dcl.GetWeeklyDomainCombos() as DomainCombinationList;
            var maxIndex = weekly.GetMaxIndex();

            Assert.AreEqual(weekly.First().IntervalFrequency.Type, TokenType.eWeekly);
            Assert.AreEqual(5, maxIndex);
        }

        [TestMethod, Description("Ensure the proper subset is pulled from the primary list")]
        public void SB_Expr_08_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList
            {
                new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(2, new Token_Accessor(TokenType.eMonthly), "SMA", new int[] { 25 }),
                new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"),
                new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close")
            };

            var monthly = dcl.GetMonthyDomainCombos() as DomainCombinationList;

            Assert.AreEqual(monthly.First().IntervalFrequency.Type, TokenType.eMonthly);
            Assert.AreEqual(25, monthly.First().Indices.First());
        }

        [TestMethod, Description("Analyzer returns expected number of DomainCombinations")]
        public void SB_Expr_07_ExpressionAnalyzer_CanAnalyzeMultpleExpression()
        {
            var expressionList = new List<Expr>();

            var rules = new RuleList() {
                new Rule("SMA(25) > 140"),
                new Rule("2 Weeks Ago Close"),
                new Rule("2 Days Ago Close"),
                new Rule("3 Weeks Ago SMA(14)"),
            };

            var scanner = new Scanner();
            var parser = new Parser();

            foreach (var r in rules)
            {
                var tkns = scanner.ScanTokens(r.Statement);
                expressionList.Add(parser.Parse(tkns));
            }

            var exprAnalyzer = new ExpressionAnalyzer(expressionList);
            exprAnalyzer.Scan();

            Assert.IsTrue(exprAnalyzer.Combos.Count == 4);
        }

        [TestMethod, Description("Analyzer returns expected number of DomainCombinations")]
        public void SB_Expr_08_ExpressionAnalyzer_CanAnalyzeMultpleExpression_WithMoreCollaborationBetweenObjects()
        {
            var rules = new RuleList() {
                new Rule("SMA(25) > 140"),
                new Rule("2 Weeks Ago Close"),
                new Rule("2 Days Ago Close"),
                new Rule("3 Weeks Ago SMA(14)"),
                new Rule("4 Months Ago Open"),
            };

            var service = new ActiveService(new Scanner(), new Parser());
            service.ProcessRules(rules);

            var exprAnalyzer = new ExpressionAnalyzer(rules.Expressions);
            exprAnalyzer.Scan();

            Assert.IsTrue(exprAnalyzer.Combos.Count == 5);
        }

        [TestMethod, Description("Analyzer can create a DomainCombination for 52WeekHigh token")]
        public void SB_Expr_09_ExpressionAnalyzer_CanRecognize52WeekHighToken()
        {
            var rules = new Pattern()
            {
                new Rule("CLOSE > (@52WeekHigh * 0.95)"),
            };
            var service = new ActiveService(new Scanner(), new Parser());
            service.ProcessRules(rules);

            var exprAnalyzer = new ExpressionAnalyzer(rules.Expressions);
            exprAnalyzer.Scan();

            Assert.IsTrue(exprAnalyzer.Combos.Count == 2);
        }
    }
}
