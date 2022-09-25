using System;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// Class <c>Grouping</c> is a parentesied statement.
    /// In the statement `(2 + 4) - 1`, the `2 + 4` becomes the group expr. 
    /// </summary>
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
