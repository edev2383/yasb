using System;


namespace StockBox.Interpreter.Expressions
{

    /// <summary>
    /// 
    /// </summary>
    public interface IVisitor
    {
        object VisitBinaryExpr(Binary expr);
        object VisitUnaryExpr(Unary expr);
        object VisitGroupingExpr(Grouping expr);
        object VisitLiteralExpr(Literal expr);
        object VisitDomainExpr(DomainExpr expr);
        object VisitDomainLiteral(DomainLiteral expr);
        object VisitDomainToken(DomainToken expr);
        object VisitAssignExpr(Assign expr);
        object VisitVariableExpr(Variable expr);
        object VisitLogicalExpr(Logical expr);
    }
}
