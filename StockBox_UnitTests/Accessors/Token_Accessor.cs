using System;
using StockBox.Base.Tokens;

namespace StockBox_UnitTests.Accessors
{
    public class Token_Accessor : Token
    {
        public Token_Accessor(TokenType type) : base(type, "", "", 0, 0)
        {
        }
    }
}
