namespace StockBox.Interpreter.Tokens
{
    public class Keyword
    {
        public string Lexeme { get { return _lexeme; } }
        public TokenType? TokenType { get { return _tokenType; } }
        public bool HasIndices {  get { return _hasIndices;  } }
        public bool IsIndicator {  get { return _tokenType == Tokens.TokenType.eIndicator; } }

        private readonly string _lexeme;
        private readonly TokenType? _tokenType;
        private readonly bool _hasIndices;

        public Keyword() { }

        public Keyword(string lexeme, TokenType tokenType, bool hasIndices = false)
        {
            _lexeme = lexeme;
            _tokenType = tokenType;
            _hasIndices = hasIndices;
        }

        public bool IsDeficient()
        {
            return _lexeme == null || _tokenType == null;
        }
    }
}
