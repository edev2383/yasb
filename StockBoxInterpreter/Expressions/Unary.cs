using StockBox.Associations.Tokens;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// Class <c>Unary</c> is a Binary without a left value. These include
    /// statements such as `-1` or `!true`.
    /// </summary>
    public class Unary : Expr
    {
        public Unary(Token op, Expr right) : base(null, op, right)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitUnaryExpr(this);
        }

        public override Expr Clone()
        {
            return new Unary(Operator, Right);
        }
    }
}
