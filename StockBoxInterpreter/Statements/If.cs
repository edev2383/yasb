using System;
using StockBox.Interpreter.Expressions;

namespace StockBox.Interpreter.Statements
{
    public class If : Stmt
    {
        public Expr Condition { get; set; }
        public Stmt ThenPath { get; set; }
        public Stmt ElsePath { get; set; }

        public If(Expr condition, Stmt thenPath, Stmt elsePath)
        {
            Condition = condition;
            ThenPath = thenPath;
            ElsePath = elsePath;
        }

        public override void Accept(IStatementVisitor visitor)
        {
            visitor.visitIfStmt(this);
        }
    }
}
