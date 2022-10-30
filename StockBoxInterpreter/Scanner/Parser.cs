using System;
using System.Linq;
using StockBox.Interpreter.Expressions;
using StockBox.Associations.Tokens;
using StockBox.Validation;
using static StockBox.Associations.Tokens.TokenType;


namespace StockBox.Interpreter.Scanner
{

    /// <summary>
    /// Class <c>Parser</c> accepts a TokenList and outputs a complex Expression
    /// to be interpretted into values.
    /// </summary>
    public class Parser : IValidationResultsListProvider
    {

        protected TokenList _tokens;
        protected int _current = 0;

        protected TokenList _cacheTokens = new TokenList();

        protected ValidationResultList _results = new ValidationResultList();

        public Parser(TokenList tokens)
        {
            _tokens = tokens;
        }

        public Parser() { }

        public Expr Parse(TokenList tokens)
        {
            _tokens = tokens;
            return Parse();
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
                _cacheTokens.AddRange(_tokens.Clone());
                _tokens = null;
                _current = 0;
            }
        }

        protected Expr Expression()
        {
            return Equality();
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

            while (Match(eCrossOver, eGreat, eGreatEqual, eLess, eLessEqual))
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
            return _tokens[_current - 1];
        }

        protected bool IsAtEnd()
        {
            return Peek().Type == eEOF;
        }

        protected Token Peek()
        {
            return _tokens[_current];
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
