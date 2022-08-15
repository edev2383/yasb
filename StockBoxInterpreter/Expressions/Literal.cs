using System;
using StockBox.Interpreter.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class Literal : Expr
    {
        public Literal(object value):base(value)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }
    }
}
