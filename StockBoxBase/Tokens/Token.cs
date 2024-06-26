﻿using System;


namespace StockBox.Base.Tokens
{

    /// <summary>
    /// Written rule statements are scanned and broken into Tokens. Each token
    /// has a TokenType which informs how the Parser class will handle it. The
    /// lexeme is a string representation of the entry scanned. Literal is the
    /// entered as the actual *literal* value. Line and Char are integer values
    /// representing the location of the Token in the provided Statement string.
    /// </summary>
    public class Token
    {

        public TokenType Type { get { return _type; } }
        public string Lexeme { get { return _lexeme; } }
        public object Literal { get { return _literal; } }
        public int? Line { get { return _line; } }
        public int? Char { get { return _char; } }


        private TokenType _type;
        private string _lexeme;
        private object _literal;
        private int? _line;
        private int? _char;


        public Token() { }
        public Token(TokenType type, string lexeme, object literal, int line, int character)
        {
            _type = type;
            _lexeme = lexeme;
            _literal = literal;
            _line = line;
            _char = character;
        }

        public Token(Token source) : this(source.Type, source.Lexeme, source.Literal, (int)source.Line, (int)source.Char)
        { }

        public override string ToString()
        {
            return $"{Type} {Lexeme} {Literal} / {Line}:{Char}";
        }

        public bool IsDeficient()
        {
            if (_type == TokenType.eDeficient) return true;
            if (string.IsNullOrEmpty(_lexeme)) return true;
            if (_literal == null) return true;
            if (_line == null) return true;
            if (_char == null) return true;
            return false;
        }

        public bool IsOfNumericType()
        {
            return Type == TokenType.eNumber;
        }

        public bool IsOfNumericOrIndexType()
        {
            if (IsOfNumericType()) return true;
            if (IsOfDomainIndexType()) return true;
            return false;
        }

        public bool IsOfDomainIndexType()
        {
            if (Type == TokenType.eDaily) return true;
            if (Type == TokenType.eWeekly) return true;
            if (Type == TokenType.eMonthly) return true;
            return false;
        }

        public bool IsDomainToken()
        {
            return _lexeme.StartsWith('@');
        }

        public Token Clone()
        {
            return new Token(this);
        }

        public bool IdentifiesAs(Token item)
        {
            if (Type != item.Type) return false;
            if (Lexeme != item.Lexeme) return false;
            if (Literal != item.Literal) return false;
            if (Char != item.Char) return false;
            if (Line != item.Line) return false;
            return true;
        }

        public string ErrorMessage_IsDeficient()
        {
            return "Token is not yet hyrdated";
        }

        public string ErrorMessage_IsOfDomainOperatorType()
        {
            return $"Token MUST BE of Domain Operator type. {Type.ToString()} given";
        }
    }
}
