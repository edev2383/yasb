using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Associations.Tokens;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Indicators;
using StockBox.Data.SbFrames;
using StockBox.Interpreter.Scanner;
using StockBox.Rules;
using StockBox.Services;
using StockBox.Setups;
using StockBox_UnitTests.Accessors;
using StockBox_TestArtifacts.Helpers;
using StockBox.Associations.Enums;
using StockBox.Models;
using StockBox_TestArtifacts.Builders.StockBox.Rules;

namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_SbFrame_Tests
    {
        [TestMethod]
        public void SB_SbFrame_01_Tests()
        {
            // source string
            var src = "SMA(25)";
            var scanner = new Scanner(src);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);

            // analyze the resultant expression
            var exprAnalyzer = new ExpressionAnalyzer(parser.Parse());
            exprAnalyzer.Scan();

            Assert.AreEqual(1, exprAnalyzer.Combos.Count);

            // from the analyzed expression, create the desired SbFrameList
            var factory = new FrameListFactory(new Reader(), new DeedleAdapter());
            var sbframelist = factory.Create(exprAnalyzer.Combos, new Symbol("MSFT")) as SbFrameList;

            Assert.IsNotNull(sbframelist);

            // find the daily frame
            var dailyFrame = sbframelist.FindByFrequency(EFrequency.eDaily);

            Assert.IsNotNull(dailyFrame);
            Assert.IsTrue(dailyFrame.Length > 0);
            Assert.AreEqual(dailyFrame.Inidcators.Count, 1);
            Assert.AreEqual(dailyFrame.Inidcators.First().Name, "SMA(25)");
        }

        [TestMethod, Description("Proof of concept to isolate unique columns/indices pairs")]
        public void SB_SbFrame_02_Test()
        {
            var closeIdx1 = new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close");
            var smaIdx2Inc25 = new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 });
            var slosto = new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "SloSto", new int[] { 14, 3 });
            var smaIdx4Inc6 = new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 6 });
            var smaIdx6Inc6 = new DomainCombination(6, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 6 });
            var closeIndx5 = new DomainCombination(5, new Token_Accessor(TokenType.eDaily), "Close");

            var dcl = new DomainCombinationList { closeIdx1, smaIdx2Inc25, slosto, smaIdx4Inc6, smaIdx6Inc6, closeIndx5, };

            var testList = new DomainCombinationList();

            foreach (var item in dcl)
                if (!testList.ContainsComparableDomainKeywordWithMatchingIndex(item))
                    testList.Add(item);

            // expect 4 unique items
            Assert.AreEqual(4, testList.Count);

            // check that we can find the comparable items in the testList
            Assert.IsTrue(testList.ContainsItem(closeIdx1) || testList.ContainsItem(closeIndx5));
            Assert.IsTrue(testList.ContainsItem(smaIdx2Inc25));
            Assert.IsTrue(testList.ContainsItem(slosto));
            Assert.IsTrue(testList.ContainsItem(smaIdx4Inc6) || testList.ContainsItem(smaIdx6Inc6));
        }

        [TestMethod, Description("Proof of concept to isolate unique columns/indices pairs, with domain code")]
        public void SB_SbFrame_03_Tests()
        {
            var closeIdx1 = new DomainCombination(1, new Token_Accessor(TokenType.eDaily), "Close");
            var smaIdx2Inc25 = new DomainCombination(2, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 25 });
            var slosto = new DomainCombination(3, new Token_Accessor(TokenType.eDaily), "SloSto", new int[] { 14, 3 });
            var smaIdx4Inc6 = new DomainCombination(4, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 6 });
            var smaIdx6Inc6 = new DomainCombination(6, new Token_Accessor(TokenType.eDaily), "SMA", new int[] { 6 });
            var closeIndx5 = new DomainCombination(5, new Token_Accessor(TokenType.eDaily), "Close");

            var dcl = new DomainCombinationList { closeIdx1, smaIdx2Inc25, slosto, smaIdx4Inc6, smaIdx6Inc6, closeIndx5, };

            var testList = dcl.GetUniqueDomainCombos();

            // expect 4 unique items
            Assert.AreEqual(4, testList.Count);

            // check that we can find the comparable items in the testList
            Assert.IsTrue(testList.ContainsItem(closeIdx1) || testList.ContainsItem(closeIndx5));
            Assert.IsTrue(testList.ContainsItem(smaIdx2Inc25));
            Assert.IsTrue(testList.ContainsItem(slosto));
            Assert.IsTrue(testList.ContainsItem(smaIdx4Inc6) || testList.ContainsItem(smaIdx6Inc6));
        }

        [TestMethod]
        public void SB_SbFrame_04_Tests()
        {
            var rules = new PatternBuilder()
                            .WithRule(new Rule("Close > 82"))
                            .WithRule(new Rule("2 Days ago SMA(5) > 78"))
                            .WithRule(new Rule("SMA(150) > 68"))
                            .Build();

            var setup = new Setup(rules);

            var actSer = new ActiveService(new Scanner(), new Parser());
            setup.Process(actSer);

            var analyzer = new ExpressionAnalyzer(rules.Expressions);
            analyzer.Scan();

            var factory = new FrameListFactory(new Reader(), new DeedleAdapter());
            var frameList = factory.Create(analyzer.Combos, new Symbol("MSFT")) as SbFrameList;

            Assert.IsTrue(frameList.Count > 0);

            // find the daily frame
            var dailyFrame = frameList.FindByFrequency(EFrequency.eDaily);

            Assert.IsNotNull(dailyFrame);
            Assert.IsTrue(dailyFrame.Length > 0);
            Assert.AreEqual(dailyFrame.Inidcators.Count, 2);
            Assert.IsTrue(dailyFrame.Inidcators.ContainsItem(new SimpleMovingAverage("sma", 150)));
            Assert.IsTrue(dailyFrame.Inidcators.ContainsItem(new SimpleMovingAverage("sma", 5)));
        }
    }
}
