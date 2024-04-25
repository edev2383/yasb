using System;
using StockBox.Base.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class Logical : Expr
    {
        public Logical(Expr left, Token op, Expr right) : base(left, op, right)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            throw new NotImplementedException();
        }

        public override Expr Clone()
        {
            throw new NotImplementedException();
        }
    }
}

