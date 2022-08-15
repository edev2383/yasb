using System;
using StockBox.Interpreter.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class Unary : Expr
    {
        public Unary(Token op, Expr right): base(null, op, right)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}
