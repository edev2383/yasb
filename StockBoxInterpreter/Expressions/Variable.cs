using System;
using StockBox.Associations.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class Variable : Expr
    {

        public Variable(Token name) : base(null, null, null, name)
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
