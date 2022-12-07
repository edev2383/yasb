using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox_UnitTests.Accessors;
using StockBox_TestArtifacts.Helpers;
using StockBox.Associations.Tokens;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;
using StockBox.Data.SbFrames;
using StockBox.Interpreter;
using StockBox.Interpreter.Expressions;
using StockBox.Interpreter.Scanner;
using StockBox.Rules;
using StockBox.Services;
using System;
using StockBox.Models;
using StockBox.Positions;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Interpreter_Tests
    {
        [TestMethod, Description("Standard test that object can be created")]
        public void SB_Interpreter_01_CanBeCreated()
        {
            var interpreter = new SbInterpreter();
            Assert.IsNotNull(interpreter);
        }

        [TestMethod, Description("Test that a Literal is interpreted as the value it's given (int)")]
        public void SB_Interpreter_02_CanInterpretLiteral_Int()
        {
            int originalValue = 15;
            Literal literal = new Literal(originalValue);
            SbInterpreter interpreter = new SbInterpreter();
            object result = literal.Accept(interpreter);

            Assert.AreEqual(result, originalValue);
        }

        [TestMethod, Description("Test that a Literal is interpreted as the value it's given (string)")]
        public void SB_Interpreter_02_CanInterpretLiteral_String()
        {
            string originalValue = "This is a test string";
            Literal literal = new Literal(originalValue);
            SbInterpreter interpreter = new SbInterpreter();
            object result = literal.Accept(interpreter);

            Assert.AreEqual(result, originalValue);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types")]
        public void SB_Interpreter_03_CanInterpretUnary()
        {
            bool originalValue = true;

            Token token = new Token(TokenType.eBang, "!", null, 0, 0);
            Literal literal = new Literal(originalValue);
            Unary unary = new Unary(token, literal);
            SbInterpreter interpreter = new SbInterpreter();
            object result = unary.Accept(interpreter);

            Assert.IsTrue(result is bool);
            Assert.IsFalse((bool)result);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types")]
        public void SB_Interpreter_04_CanInterpretUnary()
        {
            string source = "-4";
            Scanner scanner = new Scanner(source);
            Parser parser = new Parser(scanner.ScanTokens());
            Expr expression = parser.Parse();

            SbInterpreter interpreter = new SbInterpreter();
            object result = interpreter.Interpret(expression);

            Assert.IsTrue(result is double);
            Assert.AreEqual(result, -4d);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types")]
        public void SB_Interpreter_05_CanInterpretBinary()
        {
            string source = "3 > 4";
            Scanner scanner = new Scanner(source);
            Parser parser = new Parser(scanner.ScanTokens());
            Expr expression = parser.Parse();

            SbInterpreter interpreter = new SbInterpreter();
            object result = interpreter.Interpret(expression);

            Assert.IsTrue(result is bool);
            Assert.IsFalse((bool)result);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types")]
        public void SB_Interpreter_06_CanInterpretDomainExpr()
        {
            double expected = 89.300003;
            var index = new Literal(2.0);
            var op = new Token(TokenType.eDaily, "Days", null, 0, 0);
            var literal = new DomainLiteral("Close");
            var expr = new DomainExpr(index, op, literal);
            var stream = new Reader().GetFileStream(EFile.eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            var framelist = new SbFrameList();
            framelist.Add(dailyFrame);
            var interpreter = new SbInterpreter(framelist);
            var result = interpreter.Interpret(expr);

            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types derived from a source statement that is scanned and parsed")]
        public void SB_Interpreter_09_CanInterpretDomainExpressionFromParsedTokens()
        {
            double expected = 89.300003;
            var source = "2 days ago Close";
            var scanner = new Scanner(source);
            var parser = new Parser(scanner.ScanTokens());
            var stream = new Reader().GetFileStream(EFile.eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            var framelist = new SbFrameList();
            framelist.Add(dailyFrame);
            var interpreter = new SbInterpreter(framelist);
            var result = interpreter.Interpret(parser.Parse());
            Assert.AreEqual(expected, result);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types derived from a source statement that is scanned and parsed")]
        public void SB_Interpreter_10_CanInterpretCompoundDomainExpressionFromParsedTokens()
        {
            var source = "2 days ago Close > 250";
            var scanner = new Scanner(source);
            var parser = new Parser(scanner.ScanTokens());
            var stream = new Reader().GetFileStream(EFile.eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            var framelist = new SbFrameList();
            framelist.Add(dailyFrame);
            var interpreter = new SbInterpreter(framelist);
            var result = interpreter.Interpret(parser.Parse());
            Assert.IsFalse((bool)result);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types derived from a source statement that is scanned and parsed")]
        public void SB_Interpreter_11_CanInterpretCompoundDomainExpressionFromParsedTokens()
        {
            var source = "2 days ago Close < 250";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var stream = new Reader().GetFileStream(EFile.eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            var framelist = new SbFrameList() { dailyFrame };
            var interpreter = new SbInterpreter(framelist);
            var result = interpreter.Interpret(parser.Parse());
            Assert.IsTrue((bool)result);
        }

        [TestMethod, Description("Test the interpreter can properly evaluate more complex Expr types derived from a source statement that is scanned and parsed")]
        public void SB_Interpreter_12_CanInterpretCompoundDomainExpressionFromParsedTokens()
        {
            var source = "Close < 250";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var stream = new Reader().GetFileStream(EFile.eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            var framelist = new SbFrameList() { dailyFrame };
            var interpreter = new SbInterpreter(framelist);
            var result = interpreter.Interpret(parser.Parse());
            Assert.IsTrue((bool)result);
        }

        [TestMethod, Description("Test the interpreter evaluate a Compound Domain Expression with an Indicator value")]
        public void SB_Interpreter_13_CanInterpretCompoundDomainExpressionWithIndicators()
        {
            var source = "SMA(25) < 250";
            var scanner = new Scanner(source);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var stream = new Reader().GetFileStream(EFile.eAmdDaily);
            var adapter = new DeedleAdapter(stream);
            var sma = new SimpleMovingAverage("SMA", 25);
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            dailyFrame.AddIndicator(sma);
            var framelist = new SbFrameList() { dailyFrame };
            var interpreter = new SbInterpreter(framelist);
            var result = interpreter.Interpret(parser.Parse());
            Assert.IsTrue((bool)result);
        }

        [TestMethod, Description("Interpreter can handle a missing value, by throwing a general exception and returning false at catch")]
        public void SB_Interpreter_14_InterpreterCanHandleAMissingIndicatorValue()
        {
            // setup the data list to include a single data point, that will
            // not be enough to satisfy the requirements of the rulelist
            var adapter = new DataFrameAdapter_Accessor();

            // add a single datapoint with indicator data
            adapter.CreateAndAddDataPoint(DateTime.Now, 14, 10, 13, 12, 1000, "SMA(5)", 10);

            // init the frame/fraemlist
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            var framelist = new SbFrameList() { dailyFrame };

            // create and process the rules
            var rules = new RuleList() {
                new Rule("2 Days Ago SMA(5) > 10"),
            };

            var service = new ActiveService(new Scanner(), new Parser());
            service.ProcessRules(rules);

            // use the Interpreter to evaluate the rules expressions
            var result = rules.Evalute(new SbInterpreter(framelist));

            Assert.IsTrue(result.HasFailures);
        }

        [TestMethod, Description("")]
        public void SB_Interpreter_14_InterpreterCanCalculateCrossOverExpressions()
        {
            // setup the data list to include a single data point, that will
            // not be enough to satisfy the requirements of the rulelist
            var adapter = new DataFrameAdapter_Accessor();

            // add specific datapoints. The crossover operator means that the
            // value on the left was below the value on the right for the
            // previous record and has "crossed over" and is now greater than
            // the value on the right.
            // In this example, the previous record (index 1) 11 < 12 and the
            // current record (index 0) 13 > 12. So we expect a successful test
            adapter.CreateAndAddDataPoint(DateTime.Now, 0, 0, 13, 12, 1000);
            adapter.CreateAndAddDataPoint(DateTime.Now, 0, 0, 11, 12, 1000);

            // init the frame/fraemlist
            var dailyFrame = new DailyFrame(adapter, new Symbol(string.Empty));
            var framelist = new SbFrameList() { dailyFrame };

            // create and process the rules
            var rules = new RuleList() {
                new Rule("Open x Close"),
            };

            var service = new ActiveService(new Scanner(), new Parser());
            service.ProcessRules(rules);

            // use the Interpreter to evaluate the rules expressions
            var result = rules.Evalute(new SbInterpreter(framelist));

            // hazzah
            Assert.IsTrue(result.Success);
        }

        [TestMethod, Description("Test the rule > scanner > parser > interpreter flow can understadn the @Entry token")]
        public void SB_Interpreter_15_InterpreterCanCalculateEntryPriceAsExpected()
        {
            var symbol = new Symbol(string.Empty);

            var adapter = new DataFrameAdapter_Accessor();

            adapter.CreateAndAddDataPoint(DateTime.Now, 0, 0, 0, 10, 1000);
            adapter.CreateAndAddDataPoint(DateTime.Now.AddDays(-1), 0, 0, 0, 15, 1000);

            var position = new Position(Guid.NewGuid(), symbol);
            position.EntryPrice = 12.50;

            var pattern = new Pattern() {
                new Rule("@Entry > Close"),
                new Rule("@Entry < yesterday's Close"),
            };

            var dailyFrame = new DailyFrame(adapter, symbol);
            var framelist = new SbFrameList() { dailyFrame };

            var service = new ActiveService(new Scanner(), new Parser());
            service.ProcessRules(pattern);

            var result = pattern.Evalute(new SbInterpreter(framelist, position));
            Assert.IsTrue(result.Success);

        }
    }
}
