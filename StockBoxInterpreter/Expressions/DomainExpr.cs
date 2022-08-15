using System;
using StockBox.Interpreter.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class DomainExpr : Expr
    {
        /**
         * How do we define "two days ago Close" as an expression? 
         * 
         * index:   Token.Type.eTwo
         * op:      Token.Type.eDaily
         * column:  DomainLiteral.value = "Close"
         * 
         * "two days ago SMA(25)"
         * 
         * index:   Token.Type.eTwo
         * op:      Token.Type.eDaily
         * column:  DomainLiteral.Value = "SMA", DomainLiteral.indices = [25]
         * 
         * */


        
        public DomainExpr(Literal index, Token op, DomainLiteral literal): base(index, op, literal)
        {
           
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitDomainExpr(this);
        }
    }
}
