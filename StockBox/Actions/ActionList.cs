using System;
using System.Collections.Generic;
using System.Linq;


namespace StockBox.Actions
{

    public class SbActionList : List<ISbAction>
    {

        public SbActionList()
        {
        }

        public SbActionList(SbActionList source)
        {
            foreach (ISbAction item in source)
                Add(item.Clone());
        }

        public ISbAction First { get { return this.First(); } }

        public SbActionList Clone()
        {
            return new SbActionList(this);
        }
    }
}
