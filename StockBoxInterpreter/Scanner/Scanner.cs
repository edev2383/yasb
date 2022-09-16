using System;
using StockBox.Associations.Tokens;
using static StockBox.Associations.Tokens.TokenType;
using StockBox.Validation;


namespace StockBox.Interpreter.Scanner
{

    public class Scanner : IValidationResultsListProvider
    {

        protected string _source;
        protected TokenList _tokens;

        private int start = 0;
        private int current = 0;
        private int line = 1;

        private static KeywordList _domainKeywords = new DomainKeywords();

        private int _lexemeLength
        {
            get { return current - start; }
        }

        private ValidationResultList _results = new ValidationResultList();

        public Scanner(string source)
        {
            _source = source;
        }

        public Scanner() { }

        public TokenList ScanTokens(string source)
        {
            _source = source;
            return ScanTokens();
        }

        /// <summary>
        /// Reset Scanner to origin state to scan another Statement
        /// </summary>
        private void ResetScanner()
        {
            _tokens = new TokenList();
            start = 0;
            current = 0;
            line = 1;
        }

        public TokenList ScanTokens()
        {
            ResetScanner();

            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }

            _tokens.Add(new Token(eEOF, "", null, line, current));

            return _tokens.Clone();
        }

        public void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(eLeftParen); break;
                case ')': AddToken(eRightParen); break;
                case '{': AddToken(eLeftBrace); break;
                case '}': AddToken(eRightBrace); break;
                case ',': AddToken(eComma); break;
                case '.': AddToken(eDot); break;
                case '-': AddToken(eMinus); break;
                case '+': AddToken(ePlus); break;
                case ';': AddToken(eSemicolon); break;
                case '*': AddToken(eStar); break;
                case '!':
                    AddToken(Match('=') ? eBangEqual : eBang);
                    break;
                case '=':
                    AddToken(Match('=') ? eEqualEqual : eEqual);
                    break;
                case '<':
                    AddToken(Match('=') ? eLessEqual : eLess);
                    break;
                case '>':
                    AddToken(Match('=') ? eGreatEqual : eGreat);
                    break;
                case '/':
                    if (Match('/'))
                        // A comment goes until the end of the line
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    else
                        AddToken(eSlash);
                    break;
                case '&':
                    if (Match('&'))
                        AddToken(eAnd);
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    line++;
                    break;
                case '"': String(); break;
                default:
                    if (IsDigit(c))
                        Number();
                    else if (IsAlpha(c))
                        Identifier();
                    else
                        _results.Add(new ValidationResult(EResult.eFail, $"Unexpected character: '{c}' {line}:{current}"));
                    break;
            }
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                c == '_';
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();

            string text = _source.Substring(start, _lexemeLength);
            Keyword keyword = _domainKeywords.Find(text);

            // if we don't find an actual DomainKeyword, add the type as
            // a generic eIdentifier. Currently we are not expecting generic
            // identifiers and this will probably throw and error in the Parser.
            if (keyword.IsDeficient())
            {
                AddToken((TokenType)eIdentifier);
            }
            else
            {
                // an Indicator WITHOUT indices, can be added directly. If it
                // has indices, we need to account for the parenthesis when it
                // comes to the Literal, because we want to left with only a
                // comma-delimited string of ints, i.e., "14,3" for the Indices
                // token
                if (keyword.IsIndicator && keyword.HasIndices)
                {
                    if (!PeekPrevious().IsOfNumericOrIndexType())
                    {
                        InjectUnsourcedToken(TokenType.eNumber, "0", 0, line, current);
                        InjectUnsourcedToken(TokenType.eDaily, "day", "day", line, current);
                    }
                    // add the indicator immediately
                    AddToken((TokenType)eIndicator);

                    if (Peek() == '(')
                    {
                        while (PeekNext() != ')' && !IsAtEnd()) Advance();

                        if (IsAtEnd())
                        {
                            _results.Add(new ValidationResult(EResult.eFail, $"Unterminated indicator indices. {line}:{current}"));
                            return;
                        }

                        // trailing ')'
                        Advance();
                        Advance();

                        // at this point:
                        // start is the beginning of the indicator
                        // opening paren is start + keyword.lexeme.length
                        int indexStart = start + keyword.Lexeme.Length + 1; // 1 = for the open paren
                        int stringLength = current - (indexStart + 1); // 1 = for the trailing paren
                        string value = _source.Substring(indexStart, stringLength);
                        AddToken(eIndicatorIndices, value);

                    }
                }
                else
                {
                    /** -------------------------------------------------------
                    | 
                    |  Handling Non-Indexed DomainKeywords
                    |
                    | - -------------------------------------------------------
                    | at this point, we have a keyword that is non-indice'd,
                    | i.e., a column, or an indicator that does not require a
                    | numeric index (ex. SMA(15), SloSto(14,3). However, in
                    | cases of CURRENT Daily/Weekly/Monthly columns, we need
                    | to INJECT the IndexFromZero value as an "UnsourcedToken"
                    | an example rule would be "Close > XXX". The Close has a
                    | reference, but the SbInterpreter is going to look for
                    | a "Literal Operator DomainLiteral" pattern in order to
                    | calculate a value for the expression. In the "Close"
                    | example, we need to inject both the eNumber/Literal and
                    | the eDaily/Operator, however if a user were to use a rule
                    | like "Daily Close", or "Weekly High", we only have to
                    | inject the eNumber/Literal, which you can see below
                    |
                    | More testing needs to be done when working with Daily/Wk/M
                    | indexes and Indicators, i.e., "Weekly SMA(15)"
                    | - */
                    switch (keyword.TokenType)
                    {
                        case TokenType.eAnd:
                            AddToken(TokenType.eAnd);
                            break;
                        case TokenType.eDaily:
                            // check if the previous token is an eNumber, if not
                            // inject a (0) to provide an IndexFromZero literal
                            if (!PeekPrevious().IsOfNumericType())
                                InjectUnsourcedToken(TokenType.eNumber, "0", 0, line, current);
                            AddToken(TokenType.eDaily);
                            break;
                        case TokenType.eWeekly:
                            // check if the previous token is an eNumber, if not
                            // inject a (0) to provide an IndexFromZero literal
                            if (!PeekPrevious().IsOfNumericType())
                                InjectUnsourcedToken(TokenType.eNumber, "0", 0, line, current);
                            AddToken(TokenType.eWeekly);
                            break;
                        case TokenType.eMonthly:
                            // check if the previous token is an eNumber, if not
                            // inject a (0) to provide an IndexFromZero literal
                            if (!PeekPrevious().IsOfNumericType())
                                InjectUnsourcedToken(TokenType.eNumber, "0", 0, line, current);
                            AddToken(TokenType.eMonthly);
                            break;
                        case TokenType.eLast:
                            AddToken(TokenType.eNumber, 1);
                            break;
                        case TokenType.eAgo:
                            // ignore the "ago" string
                            break;
                        case TokenType.eTrue:
                            AddToken(TokenType.eTrue, true);
                            break;
                        case TokenType.eFalse:
                            AddToken(TokenType.eTrue, false);
                            break;
                        default:
                            if (!PeekPrevious().IsOfNumericOrIndexType())
                            {
                                InjectUnsourcedToken(TokenType.eNumber, "0", 0, line, current);
                                InjectUnsourcedToken(TokenType.eDaily, "day", "day", line, current);
                            }
                            AddToken((TokenType)keyword.TokenType);
                            break;
                    }
                }

            }
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private void Number()
        {
            while (IsDigit(Peek())) Advance();
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                Advance();
                while (IsDigit(Peek())) Advance();
            }

            AddToken(eNumber, double.Parse(_source.Substring(start, _lexemeLength)));
        }

        private void String()
        {
            while (Peek() != '"' && !IsAtEnd())
            {
                if (Peek() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                _results.Add(new ValidationResult(EResult.eFail, "Unterminated string."));
                return;
            }

            // The closing ".
            Advance();

            // Trim the surrounding quotes.
            int stringLength = current - 1 - (start + 1);
            string value = _source.Substring(start + 1, stringLength);
            AddToken(eString, value);
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsAtEnd()
        {
            return current >= _source.Length;
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[current];
        }

        private char PeekNext()
        {
            if (current + 1 >= _source.Length) return '\0';
            return _source[current + 1];
        }

        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[current] != expected) return false;
            current++;
            return true;
        }

        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            string lexemeText = _source.Substring(start, _lexemeLength);
            _tokens.Add(new Token(type, lexemeText, literal, line, current));
        }

        private void InjectUnsourcedToken(TokenType type, string lexeme, object literal, int line, int character)
        {
            _tokens.Add(new Token(type, lexeme, literal, line, character));
        }

        private char Advance()
        {
            return _source[current++];
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private Token PeekPrevious()
        {
            if (_tokens.Count == 0) return new Token();
            return _tokens[^1];
        }

        public ValidationResultList GetResults()
        {
            return _results;
        }
    }
}
