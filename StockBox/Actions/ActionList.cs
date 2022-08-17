using System;
using System.Collections.Generic;
using System.Linq;


namespace StockBox.Actions
{

    public class SbActionList : List<SbActionBase>
    {

        public SbActionList()
        {
        }

        public SbActionBase First { get { return this.First(); } }
    }
}
