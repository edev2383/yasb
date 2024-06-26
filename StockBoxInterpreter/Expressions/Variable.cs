﻿using System;
using StockBox.Base.Tokens;

namespace StockBox.Interpreter.Expressions
{
    public class Variable : Expr
    {

        public Variable(Token name) : base(null, null, null, name)
        {
        }

        public override object Accept(IVisitor visitor)
        {
            return visitor.VisitVariableExpr(this);
        }

        public override Expr Clone()
        {
            throw new NotImplementedException();
        }
    }
}
