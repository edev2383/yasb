﻿using System;
using System.Collections.Generic;
using StockBox.Base.Tokens;
using StockBox.Interpreter.Expressions;
using StockBox.Interpreter.Statements;
using static StockBox.Base.Tokens.TokenType;


namespace StockBox.Interpreter.Scanner
{

    /// <summary>
    /// 
    /// </summary>
    public class StatementParser : Parser
    {
        public StatementParser(TokenList tokens) : base(tokens)
        {
        }

        public List<Stmt> StatementParse()
        {
            try
            {

                var statements = new List<Stmt>();
                while (!IsAtEnd())
                    statements.Add(Declaration());
                return statements;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        protected Stmt Statement()
        {


            return ExpressionStatement();
        }

        protected Stmt ExpressionStatement()
        {
            Expr expr = Expression();

            return new Stmt.Expression(expr);
        }

        protected Stmt Declaration()
        {
            try
            {
                if (Match(eVar)) return VarDeclaration();

                return Statement();
            }
            catch (Exception e)
            {
                Synchronize();
                return null;
            }
        }

        protected Stmt.Var VarDeclaration()
        {
            Token name = Consume(eIdentifier, "Expect variable name.");

            Expr initializer = null;
            if (Match(eEqual))
                initializer = Expression();

            Consume(eSemicolon, "Expect ';' after variable declaration.");

            return new Stmt.Var(name, initializer);
        }
    }
}
