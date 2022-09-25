using System;
using StockBox.Associations.Tokens;


namespace StockBox.Interpreter.Expressions
{

    public class DomainLiteral : Expr
    {
        public DomainLiteral(object column, params int[] indices) : base(column, indices)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitDomainLiteral(this);
        }

    }
}
