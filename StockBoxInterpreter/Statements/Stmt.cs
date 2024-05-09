using System;
using System.Collections.Generic;
using StockBox.Base.Tokens;
using StockBox.Interpreter.Expressions;

namespace StockBox.Interpreter.Statements
{
    public abstract class Stmt
    {
        public abstract void Accept(IStatementVisitor visitor);

        public class Expression : Stmt
        {
            public Expr Expr { get; set; }

            public Expression(Expr expr)
            {
                Expr = expr;
            }

            public override void Accept(IStatementVisitor visitor)
            {
                visitor.VisitExpressionStmt(this);
            }
        }

        public class If : Stmt
        {
            public Expr Condition { get; set; }
            public Stmt ThenBranch { get; set; }
            public Stmt ElseBranch { get; set; }

            public If(Expr condition, Stmt thenBranch, Stmt elseBranch)
            {
                Condition = condition;
                ThenBranch = thenBranch;
                ElseBranch = elseBranch;
            }

            public override void Accept(IStatementVisitor visitor)
            {
                visitor.VisitIfStmt(this);
            }
        }

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
                visitor.VisitVarStmt(this);
            }
        }

        public class Block : Stmt
        {
            public List<Stmt> Statements { get; set; }
            public Block(List<Stmt> statements)
            {
                Statements = statements;
            }

            public override void Accept(IStatementVisitor visitor)
            {
                visitor.VisitBlockStmt(this);
            }
        }
    }
}
