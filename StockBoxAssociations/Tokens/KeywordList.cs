using System;
using System.Collections.Generic;
using static StockBox.Associations.Tokens.TokenType;

namespace StockBox.Associations.Tokens
{
    public abstract class KeywordList : List<Keyword>
    {

        public KeywordList()
        {
            Init();
        }

        public void Add(string lexeme, TokenType tokenType, bool hasIndices = false)
        {
            Add(new Keyword(lexeme, tokenType, hasIndices));
        }

        public Keyword Find(string lexeme)
        {
            foreach (Keyword item in this)
            {
                if (item.Lexeme == lexeme.ToLower())
                    return item;
            }
            return new Keyword();
        }

        protected abstract void Init();
    }
}
