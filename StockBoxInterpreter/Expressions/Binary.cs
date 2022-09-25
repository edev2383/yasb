using StockBox.Associations.Tokens;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// 
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

    }
}
