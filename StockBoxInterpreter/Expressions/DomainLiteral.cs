using System;
using StockBox.Associations.Tokens;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// Class <c>DomainLiteral</c> typically evalutes to a Column/Indicator
    /// header, e.g., "Close", "Open", "SMA(25)", etc.
    /// </summary>
    public class DomainLiteral : Expr
    {
        public DomainLiteral(object column, params int[] indices) : base(column, indices)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitDomainLiteral(this);
        }

        public override Expr Clone()
        {
            return new DomainLiteral(Column, Indices);
        }
    }
}
