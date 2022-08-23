using System;
using System.Collections.Generic;


namespace StockBox.Setups
{

    public class SetupList : List<Setup>
    {
        public SetupList()
        {
        }

        public SetupList(Setup setup) : this(new SetupList { setup }) { }

        public SetupList(SetupList source)
        {
            AddRange(source);
        }

        public SetupList(IEnumerable<Setup> source)
        {
            AddRange(source);
        }

        public bool IdentifiesAs(Setup item)
        {

            return false;
        }
    }
}
