using System;
using StockBox.Interpreter.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class DomainLiteral : Expr
    {
        public DomainLiteral(object column, params int[] indices) : base(column, indices)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            //StackTrace  "   at StockBox.Interpreter.SbInterpreter.VisitDomainLiteral(DomainLiteral expr) in /Users/jefferyedick/Projects/StockBox/StockBoxInterpreter/SbInterpreter.cs:line 83\n   at StockBox.Interpreter.Expressions.DomainLiteral.Accept(IVisitor visitor) in /Users/jefferyedick/Projects/StockBox/StockBoxInterpreter/Expressions/DomainLiteral.cs:line 14\n   at StockBox.Interpreter.SbInterpreter.evaluate(Expr expr) in /Users/jefferyedick/Projects/StockBox/StockBoxInterpreter/SbInterpreter.cs:line 205\n   at StockBox.Interpreter.SbInterpreter.VisitBinaryExpr(Binary expr) in /Users/jefferyedick/Projects/StockBox/StockBoxInterpreter/SbInterpreter.cs:line 94\n   at StockBox.Interpreter.Expressions.Binary.Accept(IVisitor visitor) in /Users/jefferyedick/Projects/StockBox/StockBoxInterpreter/Expressions/Binary.cs:line 14\n   at StockBox.Interpreter.SbInterpreter.evaluate(Expr expr) in /Users/jefferyedick/Projects/StockBox/StockBoxInterpreter/SbInterpreter.cs:line 205\n   at StockBox.Interpreter.SbInterpre…	string
            return visitor.VisitDomainLiteral(this);
        }
    }
}
