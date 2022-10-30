using System;
using StockBox.Interpreter.Expressions;

namespace StockBox.Interpreter.Statements
{
    public class Expression : Stmt
    {
        public Expr Expr { get; set; }
        public Expression(Expr expr)
        {
            Expr = expr;
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.visitExpressionStmt(this);
        }
    }
}
