﻿using System;
using System.Collections.Generic;
using StockBox.Base.Tokens;
using StockBox.Interpreter.Expressions;


namespace StockBox.Interpreter.Scanner
{

    /// <summary>
    /// Class <c>ExpressionAnalyzer</c> builds a DomainCombinationList from a
    /// provided List of Expr(essions). 
    ///
    /// This needs work. 
    /// </summary>
    public class ExpressionAnalyzer : IVisitor
    {

        public DomainCombinationList Combos { get; set; } = new DomainCombinationList();
        private readonly List<Expr> _expressions;

        public ExpressionAnalyzer(Expr expression) : this(new List<Expr> { expression }) { }

        public ExpressionAnalyzer(List<Expr> expressions)
        {
            _expressions = expressions;
        }

        public void Scan()
        {
            foreach (var e in _expressions)
                Scan(e);
        }

        public object VisitBinaryExpr(Binary expr)
        {
            if (expr.Right is DomainLiteral)
            {
                var dl = (DomainLiteral)expr.Right;
                var combo = new DomainCombination(Scan(expr.Left), expr.Operator, (string)dl.Column, dl.Indices);
                Combos.Add(combo);
            }
            else
            {
                // At present, we are only interested in scanning Binary Exprs
                // because we forced all DomainExpressions to be a subset of
                // Binary
                if (expr.Left is Binary)
                    Scan(expr.Left);
                if (expr.Right is Binary)
                    Scan(expr.Right);
                if (expr.Right is Grouping)
                    VisitBinaryExpr(((Grouping)expr.Right).Expression as Binary);
                if (expr.Left is DomainToken)
                    VisitDomainToken(expr.Left as DomainToken);
                if (expr.Right is DomainToken)
                    VisitDomainToken(expr.Right as DomainToken);
            }


            return null;
        }

        /// <summary>
        /// Evaluate any found domain tokens prefixed w/ an `@` char.
        ///
        /// Current list includes @Entry, @AllTimeHigh, @AllTimeLow, @52WeekHigh
        /// and @52WeekLow
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public object VisitDomainToken(DomainToken expr)
        {
            var combo = new DomainCombination(0, expr.Operator, expr.Operator.Lexeme);
            Combos.Add(combo);
            return null;
        }


        public object VisitUnaryExpr(Unary expr)
        {
            object right = Scan(expr.Right);
            return null;
        }

        public object VisitGroupingExpr(Grouping expr)
        {
            // Groupings are generally two Binary Exprs separated by an operator
            // so we can just scan them like anything else
            object left = Scan(expr.Left);
            object right = Scan(expr.Right);
            return null;
        }

        public object VisitLiteralExpr(Literal expr)
        {
            return expr.Value;
        }

        public object VisitDomainExpr(DomainExpr expr)
        {
            object left = Scan(expr.Left);
            object right = Scan(expr.Right);
            return null;
        }

        public object VisitDomainLiteral(DomainLiteral expr)
        {
            object left = Scan(expr.Left);
            object right = Scan(expr.Right);
            return null;
        }

        public object Scan(Expr expression)
        {
            return expression.Accept(this);
        }

        public object VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }

        public object VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }

        public object VisitLogicalExpr(Logical expr)
        {
            throw new NotImplementedException();
        }
    }
}
