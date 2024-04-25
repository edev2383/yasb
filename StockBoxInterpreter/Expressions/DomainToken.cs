using System;
using StockBox.Base.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class DomainToken : Expr
    {
        public DomainToken(Token token) : base(null, null, token, null)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitDomainToken(this);
        }

        public override Expr Clone()
        {
            throw new NotImplementedException();
        }
    }
}
