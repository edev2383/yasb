using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Mapping;
using StockBox.Base.Tokens;

namespace StockBox.Interpreter.Environments
{
    public class Environment
    {

        private Environment Enclosing;
        private Dictionary<string, object> Values = new Dictionary<string, object>();

        public Environment()
        {
            Enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            Enclosing = enclosing;
        }

        public void Define(string name, object value)
        {
            Values.Add(name, value);
        }


        public object Get(string name)
        {
            return Get(new Token(TokenType.eVar, name, null, 0, 0));
        }

        public object Get(Token name)
        {
            var valueFound = Values.TryGetValue(name.Lexeme, out object retValue);
            if (valueFound)
            {
                return retValue;
            }

            if (Enclosing != null) return Enclosing.Get(name.Lexeme);

            throw new Exception($"Undefined variable '{name.Lexeme}'.");
        }

        public void Assign(Token name, object value)
        {
            if (Values.ContainsKey(name.Lexeme))
            {
                Values[name.Lexeme] = value;
                return;
            }

            if (Enclosing != null)
            {
                Enclosing.Assign(name, value);
                return;
            }

            throw new Exception("Undefined variable '" + name.Lexeme + "'.");
        }
    }
}
