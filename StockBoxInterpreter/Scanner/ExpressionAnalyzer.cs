using System;
using System.Collections.Generic;
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
        private List<Expr> _expressions;

        public ExpressionAnalyzer(Expr expression) : this(new List<Expr> { expression }) { }

        public ExpressionAnalyzer(List<Expr> expressions)
        {
            _expressions = expressions;
        }

        public void Scan()
        {
            foreach (var e in _expressions)
                Scan(e);
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

        public object Scan(Expr expression)
        {
            return expression.AcceptAnalyzer(this);
        }
    }
}
