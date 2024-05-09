using System;
using StockBox.Base.Tokens;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// Class <c>DomainExpr</c> is an Sb domain specific Binary.
    /// `2 days ago Close`, where `2` is the index, `days ago` becomes the
    /// TokenType.eDaily as an operator, and the right expression is a
    /// DomainLiteral column/indicator.
    /// </summary>
    public class DomainExpr : Expr
    {

        public DomainExpr(Literal index, Token op, DomainLiteral literal) : base(index, op, literal)
        {

        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitDomainExpr(this);
        }

        public override Expr Clone()
        {
            return new DomainExpr(Left as Literal, Operator, Right as DomainLiteral);
        }
    }
}
