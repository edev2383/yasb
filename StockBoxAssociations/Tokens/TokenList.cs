using System;
using System.Collections.Generic;

namespace StockBox.Associations.Tokens
{
    public class TokenList : List<Token>
    {

        public TokenList() { }

        public TokenList(List<Token> source)
        {
            AddRange(source);
        }

        public TokenList(TokenList source) : base(source) { }

        public TokenList Clone()
        {
            var ret = new TokenList();
            foreach (Token token in this)
                ret.Add(token.Clone());
            return ret;
        }
    }
}
