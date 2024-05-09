using StockBox.Base.Tokens;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// Class <c>Binary</c> is a simple expression of two values with an
    /// operator, i.e., `2 == 4`, `1 + 2`, etc.
    /// </summary>
    public class Binary : Expr
    {
        public Binary(Expr left, Token op, Expr right) : base(left, op, right)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitBinaryExpr(this);
        }

        public override Expr Clone()
        {
            return new Binary(Left, Operator, Right);
        }
    }
}
