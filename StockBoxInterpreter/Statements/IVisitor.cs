using System;
namespace StockBox.Interpreter.Statements
{
    public interface IStatementVisitor
    {
        //void visitBlockStmt(Block stmt);
        //void visitClassStmt(Class stmt);
        void visitExpressionStmt(Expression stmt);
        //void visitFunctionStmt(Function stmt);
        void visitIfStmt(If stmt);
        //void visitPrintStmt(Print stmt);
        //void visitReturnStmt(Return stmt);
        void visitVarStmt(Var stmt);
        //void visitWhileStmt(While stmt);
    }
}
