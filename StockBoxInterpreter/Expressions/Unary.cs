using System;
using StockBox.Associations.Tokens;


namespace StockBox.Interpreter.Expressions
{

    public class Unary : Expr
    {
        public Unary(Token op, Expr right) : base(null, op, right)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }

        public override object AcceptAnalyzer(IVisitor visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }
    }
}
