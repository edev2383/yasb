using System;
using StockBox.Associations.Tokens;
using StockBox.Interpreter.Expressions;


namespace StockBox.Interpreter.Scanner
{

    /// <summary>
    /// This needs work. The analyzer will take an expression returned by the
    /// Parser and build the DomainCombinationList. The 
    /// </summary>
    public class ExpressionAnalyzer : IVisitor
    {

        public DomainCombinationList Combos { get; set; } = new DomainCombinationList();
        private Expr _expression;

        public ExpressionAnalyzer(Expr expression)
        {
            _expression = expression;
        }

        public void Scan()
        {
            Scan(_expression);
        }

        public object VisitBinaryExpr(Binary expr)
        {
            if (expr.Right is DomainLiteral)
            {
                var dl = (DomainLiteral)expr.Right;
                var combo = new DomainCombination(Scan(expr.Left), expr.Operator, (string)dl.Column, dl.Indices);
                Combos.Add(combo);
            }
            else
            {
                if (expr.Left is Binary)
                    Scan(expr.Left);
                if (expr.Right is Binary)
                    Scan(expr.Right);
            }


            return null;
        }

        public object VisitUnaryExpr(Unary expr)
        {
            object right = Scan(expr.Right);
            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            object left = Scan(expr.Left);
            object right = Scan(expr.Right);
            return null;
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitDomainExpr(DomainExpr expr)
        {
            object left = Scan(expr.Left);
            object right = Scan(expr.Right);
            return null;
        }

        public object VisitDomainLiteral(DomainLiteral expr)
        {
            object left = Scan(expr.Left);
            object right = Scan(expr.Right);
            return null;
        }

        private object Scan(Expr expression)
        {
            return expression.AcceptAnalyzer(this);
        }
    }
}
