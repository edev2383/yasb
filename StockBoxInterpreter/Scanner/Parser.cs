using System;
using System.Linq;
using StockBox.Interpreter.Expressions;
using StockBox.Base.Tokens;
using StockBox.Validation;
using static StockBox.Base.Tokens.TokenType;
using StockBox.Interpreter.Statements;
using System.Collections.Generic;
using NHibernate.Hql.Ast.ANTLR.Tree;

namespace StockBox.Interpreter.Scanner
{

    /// <summary>
    /// Class <c>Parser</c> accepts a TokenList and outputs a complex Expression
    /// to be interpretted into values.
    /// </summary>
    public class Parser : IValidationResultsListProvider
    {
        public TokenList Tokens
        {
            get
            {
                if (_tokens == null)
                    _tokens = new TokenList();
                return _tokens;
            }
            set
            {
                _tokens = value;
            }
        }
        protected TokenList _tokens;
        protected int _current = 0;

        protected TokenList _cacheTokens = new TokenList();

        protected ValidationResultList _results = new ValidationResultList();

        public Parser(TokenList tokens)
        {
            Tokens = tokens;
        }

        public Parser() { }

        public Expr Parse(TokenList tokens)
        {
            Tokens = tokens;
            return Parse();
        }

        public List<Stmt> ParseStatements(List<Token> tokens)
        {
            Tokens.AddRange(tokens);
            List<Stmt> statements = new List<Stmt>();
            while (!IsAtEnd())
            {
                statements.Add(Declaration());
            }

            return statements;
        }

        private Stmt Declaration()
        {
            if (Match(eVar)) return VarDeclaration();
            return Statement();
        }

        private Stmt VarDeclaration()
        {
            Token name = Consume(eIdentifier, "Expect variable name.");

            Expr initializer = null;
            if (Match(eEqual))
            {
                initializer = Expression();
            }

            Consume(eSemicolon, "Expect ';' after variable declaration.");
            return new Stmt.Var(name, initializer);
        }
        private Stmt Statement()
        {
            //if (Match(PRINT)) return printStatement();
            if (Match(eLeftBrace)) return new Stmt.Block(Block());
            return ExpressionStatement();
        }

        private List<Stmt> Block()
        {
            var ret = new List<Stmt>();
            while (!Check(eRightBrace) && !IsAtEnd())
            {
                ret.Add(Declaration());
            }

            Consume(eRightBrace, "Expect '}' after block.");

            return ret;
        }

        private Stmt ExpressionStatement()
        {
            Expr expr = Expression();
            Consume(eSemicolon, "Expect ';' after expression.");
            return new Stmt.Expression(expr);
        }
        /// <summary>
        /// Parse will iterate through the tokens, combining them recursively to
        /// return a Expr that the SbInterpreter will then evaluate
        /// </summary>
        /// <returns></returns>
        public Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch (Exception e)
            {
                _results.Add(new ValidationResult(EResult.eFail, e.Message));
                throw;
            }
            finally
            {
                _cacheTokens.AddRange(Tokens.Clone());
                Tokens = null;
                _current = 0;
            }
        }

        protected Expr Expression()
        {
            return Assignment();
        }

        protected Expr Or()
        {
            Expr expr = And();

            while (Match(eOr))
            {
                Token op = Previous();
                Expr right = And();
                expr = new Logical(expr, op, right);
            }

            return expr;
        }

        protected Expr And()
        {
            Expr expr = Equality();

            while (Match(eAnd))
            {
                Token op = Previous();
                Expr right = Equality();
                expr = new Logical(expr, op, right);
            }

            return expr;
        }

        protected Expr Assignment()
        {
            Expr expr = Or();

            if (Match(eEqual))
            {
                Token equals = Previous();
                Expr value = Assignment();

                if (expr is Variable)
                {
                    Token name = ((Variable)expr).Name;
                    return new Assign(name, value);
                }

                _results.Add(new ValidationResult(false, "Invalid assignment target.", equals));
            }

            return expr;
        }

        protected Expr Equality()
        {
            Expr expr = Comparison();

            while (Match(eBangEqual, eEqualEqual))
            {
                Token oper = Previous();
                Expr right = Comparison();
                expr = new Binary(expr, oper, right);
            }

            return expr;
        }

        protected Expr Comparison()
        {
            Expr expr = Term();

            while (Match(eCrossOver, eGreaterThan, eGreaterThanOrEqual, eLessThan, eLessThenOrEqual))
            {
                Token oper = Previous();
                Expr right = Term();
                expr = new Binary(expr, oper, right);
            }

            return expr;
        }

        protected Expr Term()
        {
            Expr expr = Factor();

            while (Match(eMinus, ePlus))
            {
                Token oper = Previous();
                Expr right = Factor();
                expr = new Binary(expr, oper, right);
            }

            return expr;
        }

        protected Expr Factor()
        {
            Expr expr = Unary();

            while (Match(eDaily, eWeekly, eMonthly, eSlash, eStar))
            {
                Token oper = Previous();
                Expr right = Unary();
                expr = new Binary(expr, oper, right);
            }

            return expr;
        }

        protected Expr Unary()
        {
            if (Match(eBang, eMinus))
            {
                Token oper = Previous();
                Expr right = Unary();
                return new Unary(oper, right);
            }

            return Primary();
        }

        protected Expr Primary()
        {
            if (Match(eAllTimeHigh)) return new DomainToken(Previous());
            if (Match(eAllTimeLow)) return new DomainToken(Previous());
            if (Match(e52WeekHigh)) return new DomainToken(Previous());
            if (Match(e52WeekLow)) return new DomainToken(Previous());
            if (Match(eEntryPoint)) return new DomainToken(Previous());
            if (Match(eFalse)) return new Literal(false);
            if (Match(eTrue)) return new Literal(true);
            if (Match(eNull)) return new Literal(null);

            if (Match(eIdentifier))
            {
                return new Variable(Previous());
            }

            if (Match(eNumber, eString))
                return new Literal(Previous().Literal);

            if (Match(eLeftParen))
            {
                Expr expr = Expression();
                Consume(eRightParen, "Expect ')' after expression.");
                return new Grouping(expr);
            }

            if (Match(eColumn))
                return new DomainLiteral(Previous().Lexeme);

            if (Match(eIndicator))
            {
                // if we've matched an indicator, set the indicator and we need
                // to deal with the possibel indices. If the next token is NOT
                // of type eIndicator, we can just declare the DomainLiteral w/
                // the Token.Lexeme and leave indices null
                Token indicator = Previous();
                int[] indices = null;
                Token indicesToken = null;
                if (Peek().Type == TokenType.eIndicatorIndices)
                {
                    // otherwise we need to get the indicesToken and convert the
                    // comma-separated values of the token.Literal to an array
                    // of integers. Most indicator usage will be a single int,
                    // i.e., SMA(25), however there are others that have more
                    // than one given index, i.e., SlowSto(14,3)
                    indicesToken = Advance();
                    indices = ((string)indicesToken.Literal).Split(',').Select(x => int.Parse(x)).ToArray();
                }
                return new DomainLiteral(indicator.Lexeme, indices);
            }

            // at this point the given token hasn't been handled
            throw new Exception("Expect expression");
        }

        protected Token Consume(TokenType type, string message)
        {
            if (Check(type)) return Advance();
            _results.Add(new ValidationResult(EResult.eFail, message));
            return new Token();
        }

        protected bool Match(params TokenType[] args)
        {
            foreach (TokenType item in args)
            {
                if (Check(item))
                {
                    Advance();
                    return true;
                }
            }
            return false;
        }

        protected Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        protected bool Check(TokenType tryType)
        {
            if (IsAtEnd()) return false;
            return Peek().Type == tryType;
        }

        protected Token Previous()
        {
            return Tokens[_current - 1];
        }

        protected bool IsAtEnd()
        {
            return Peek().Type == eEOF;
        }

        protected Token Peek()
        {
            return Tokens[_current];
        }

        public ValidationResultList GetResults()
        {
            return _results;
        }

        protected void Synchronize()
        {
            Advance();
            while (!IsAtEnd())
            {
                if (Previous().Type == eSemicolon) return;

                switch (Peek().Type)
                {
                    case eIf:
                    case eVar:
                        return;
                }
            }
            Advance();
        }
    }
}
