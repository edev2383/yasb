using System;
namespace StockBox.Interpreter.Expressions
{
    public class Grouping : Expr
    {
        public Expr Expression { get { return _expression; } }
        private Expr _expression;

        public Grouping(Expr expression) : base(expression.Left, expression.Operator, expression.Right)
        {
            _expression = expression;
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitGroupingExpr(this);
        }

    }
}
