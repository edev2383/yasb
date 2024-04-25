using System;
using StockBox.Base.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class Assign : Expr
    {
        public Assign(Token name, Expr value) : base(name, value)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitAssignExpr(this);
        }

        public override Expr Clone()
        {
            throw new NotImplementedException();
        }
    }
}

