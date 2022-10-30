using System;
namespace StockBox.Interpreter.Statements
{
    public abstract class Stmt
    {
        public Stmt()
        {
        }

        public abstract void Accept(IStatementVisitor visitor);
    }
}
