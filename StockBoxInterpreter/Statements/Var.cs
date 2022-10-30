using System;
using StockBox.Associations.Tokens;
using StockBox.Interpreter.Expressions;

namespace StockBox.Interpreter.Statements
{
    public class Var : Stmt
    {
        public Token Name { get; set; }
        public Expr Initializer { get; set; }

        public Var(Token name, Expr initializer)
        {
            Name = name;
            Initializer = initializer;
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.visitVarStmt(this);
        }
    }
}
