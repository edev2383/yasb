using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Interpreter.Scanner;


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
    }
}
