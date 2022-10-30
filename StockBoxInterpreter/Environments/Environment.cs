using System;
using System.Collections.Generic;
using StockBox.Associations.Tokens;

namespace StockBox.Interpreter.Environments
{
    public class Environment
    {
        private Dictionary<string, object> Values = new Dictionary<string, object>();
        public Environment()
        {
        }

        public void Define(string name, object value)
        {
            Values.Add(name, value);
        }

        public object Get(Token name)
        {
            var valueFound = Values.TryGetValue(name.Lexeme, out object retValue);
            if (!valueFound)
                throw new Exception($"Undefined variable '{name.Lexeme}'.");
            return retValue;
        }
    }
}
