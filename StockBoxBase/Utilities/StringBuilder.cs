using System;
using System.Collections.Generic;

namespace StockBox.Base.Utilities
{
    public class StringBuilder : List<object>
    {
        public StringBuilder()
        {
        }

        public string Build(char glue)
        {
            return string.Join(glue, this);
        }

        public string Build(string glue)
        {
            return string.Join(glue, this);
        }
    }
}
