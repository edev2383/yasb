using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockBox.Interpreter.Expressions;
using StockBox.Interpreter.Scanner;
using StockBox.Associations.Tokens;


namespace StockBox_UnitTests
{

    [TestClass]
    public class SB_Parser_Tests
    {

        [TestMethod]
        public void SB_Parser_01_ParserCanBeCreated()
        {
            Assert.IsNotNull(new Parser(new TokenList()));
        }

        [TestMethod]
        public void SB_Parser_02_SimpleStatementParsedCorrectly()
        {
            var p = GetParserWithSource("1 + 2");
            var expression = p.Parse();
            Assert.IsInstanceOfType(expression.Left, typeof(Literal));
            Assert.IsInstanceOfType(expression.Operator, typeof(Token));
            Assert.IsInstanceOfType(expression.Right, typeof(Literal));
        }

        [TestMethod]
        public void SB_Parser_03_GroupingStatementParsedCorrectly()
        {
            var p = GetParserWithSource("(1+2)+5");
            var expression = p.Parse();
            Assert.IsInstanceOfType(expression.Left, typeof(Grouping));
            Expr groupExpression = ((Grouping)expression.Left).Expression;

            Assert.IsInstanceOfType(groupExpression, typeof(Expr));
            Assert.IsInstanceOfType(groupExpression.Left, typeof(Literal));
            Assert.AreEqual(((Literal)groupExpression.Left).Value, 1d);
            Assert.AreEqual(((Literal)groupExpression.Right).Value, 2d);

            Assert.IsInstanceOfType(expression.Operator, typeof(Token));
            Assert.IsInstanceOfType(expression.Right, typeof(Literal));
            Assert.AreEqual(((Literal)expression.Right).Value, 5d);

        }

        [TestMethod]
        public void SB_Parser_04_DomainExprParsedCorrectly()
        {
            var p = GetParserWithSource("2 days ago Close > 250");
            var expression = p.Parse();
            Assert.IsInstanceOfType(expression, typeof(Binary));
            Assert.IsInstanceOfType(expression.Left, typeof(Binary));
            Assert.IsInstanceOfType(expression.Operator, typeof(Token));
            Assert.AreEqual(expression.Operator.Type, TokenType.eGreat);

            Assert.IsInstanceOfType(expression.Right, typeof(Literal));
            Literal exprRight = (Literal)expression.Right;
            Assert.AreEqual(250d, exprRight.Value);

            Binary left = (Binary)expression.Left;
            Assert.IsInstanceOfType(left.Left, typeof(Literal));
            Literal leftLiteral = (Literal)left.Left;
            Assert.AreEqual(2d, leftLiteral.Value);
            Assert.IsInstanceOfType(left.Right, typeof(DomainLiteral));
        }

        [TestMethod]
        public void SB_Parser_05_PercentExprParsedCorrectly()
        {
            var p = GetParserWithSource("20%");
            var expression = p.Parse();
            Assert.IsInstanceOfType(expression, typeof(Binary));
            Assert.IsInstanceOfType(expression.Left, typeof(Literal));
            Assert.IsInstanceOfType(expression.Operator, typeof(Token));
            Assert.IsInstanceOfType(expression.Right, typeof(Literal));
        }

        [TestMethod]
        public void SB_Parser_06_CrossOverExprParsedCorrectly()
        {
            var p = GetParserWithSource("Close x Yesterday's High");
            var expression = p.Parse();
        }

        private Parser GetParserWithSource(string source)
        {
            var s = new Scanner(source);
            return new Parser(s.ScanTokens());
        }

        [TestMethod]
        public void SB_Parser_05_ParserCanCorrectlyParseACompoundExpression()
        {
            var source = "100 > 40 AND 60 > 40";
            var parser = GetParserWithSource(source);
            var expression = parser.Parse();
        }
    }
}
