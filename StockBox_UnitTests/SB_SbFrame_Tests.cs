using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.Context;
using StockBox.Data.SbFrames;
using StockBox.Interpreter.Scanner;
using StockBox_UnitTests.Helpers;
using System;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_SbFrame_Tests
    {
        [TestMethod]
        public void SB_SbFrame_01_Tests()
        {
            var src = "SMA(25)";
            var scanner = new Scanner(src);
            var tokens = scanner.ScanTokens();
            var parser = new Parser(tokens);
            var expr = parser.Parse();

            var exprAnalyzer = new ExpressionAnalyzer(expr);
            exprAnalyzer.Scan();

            // 
            var factory = new FrameListFactory(new Reader(), new DeedleAdapter());
            var sbframelist = factory.Create(exprAnalyzer.Combos);

            Assert.IsNotNull(sbframelist);

            var dailyFrame = sbframelist.FindByFrequency(EFrequency.eDaily);

            Assert.IsNotNull(dailyFrame);
            Assert.IsTrue(dailyFrame.Length > 0);
        }
    }
}
