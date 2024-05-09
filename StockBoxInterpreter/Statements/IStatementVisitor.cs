using System;
namespace StockBox.Interpreter.Statements
{
    public interface IStatementVisitor
    {
        void VisitBlockStmt(Stmt.Block stmt);
        //void visitClassStmt(Class stmt);
        void VisitExpressionStmt(Stmt.Expression stmt);
        //void visitFunctionStmt(Function stmt);
        void VisitIfStmt(Stmt.If stmt);
        //void visitPrintStmt(Print stmt);
        //void visitReturnStmt(Return stmt);
        void VisitVarStmt(Stmt.Var stmt);
        //void visitWhileStmt(While stmt);
    }
}
