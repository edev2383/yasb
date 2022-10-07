using System;
using StockBox.Associations.Tokens;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// Class <c>Literal</c> describes a literal value, i.e., a string,
    /// a number, or boolean.
    /// </summary>
    public class Literal : Expr
    {
        public Literal(object value) : base(value)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitLiteralExpr(this);
        }

        public override Expr Clone()
        {
            return new Literal(Value);
        }
    }
}
