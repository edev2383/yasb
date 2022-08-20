using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Associations.Tokens;
using StockBox.Interpreter.Expressions;
using StockBox.Interpreter.Scanner;
using StockBox.Rules;
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
            var dcl = new DomainCombinationList();
            dcl.Add(new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }));
            dcl.Add(new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close"));

            dcl.GetMaxIndex();
        }

        [TestMethod, Description("Ensure the combolist subset returns the expected primary index")]
        public void SB_Expr_04_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList();
            dcl.Add(new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }));
            dcl.Add(new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close"));

            var daily = dcl.GetDailyDomainCombos();
            var maxIndex = daily.GetMaxIndex();

            Assert.AreEqual(daily.First().IntervalFrequency.Type, TokenType.eDaily);
            Assert.AreEqual(4, maxIndex);
        }

        [TestMethod, Description("Ensure the combolist subset returns the expected indicator index")]
        public void SB_Expr_05_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList();
            dcl.Add(new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }));
            dcl.Add(new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close"));

            var daily = dcl.GetDailyDomainCombos();
            var maxIndicatorIndex = daily.GetMaxIndicatorIndex();

            Assert.AreEqual(daily.First().IntervalFrequency.Type, TokenType.eDaily);
            Assert.AreEqual(25, maxIndicatorIndex);
        }

        [TestMethod, Description("Ensure the proper subset is pulled from the primary list")]
        public void SB_Expr_06_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList();
            dcl.Add(new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 }));
            dcl.Add(new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close"));

            var weekly = dcl.GetWeeklyDomainCombos();
            var maxIndex = weekly.GetMaxIndex();

            Assert.AreEqual(weekly.First().IntervalFrequency.Type, TokenType.eWeekly);
            Assert.AreEqual(5, maxIndex);
        }

        [TestMethod, Description("Ensure the proper subset is pulled from the primary list")]
        public void SB_Expr_08_DomainCombinationListReturnsCorrectIndexValue()
        {
            var dcl = new DomainCombinationList();
            dcl.Add(new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(2, new Token_Accessor(TokenType.eMonthly), "SMA", new int[] { 25 }));
            dcl.Add(new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "Close"));
            dcl.Add(new DomainCombination(5, new Token_Accessor(TokenType.eWeekly), "Close"));

            var monthly = dcl.GetMonthyDomainCombos();
            var maxIndex = monthly.GetMaxIndex();

            Assert.AreEqual(monthly.First().IntervalFrequency.Type, TokenType.eMonthly);
            Assert.AreEqual(2, maxIndex);
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
    }
}
