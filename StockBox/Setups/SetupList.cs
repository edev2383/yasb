using System.Collections.Generic;
using StockBox.States;


namespace StockBox.Setups
{

    /// <summary>
    /// 
    /// </summary>
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

        public SetupList FindAllByState(StateBase state)
        {
            var ret = new SetupList();
            foreach (var item in this)
                if (item.OriginState.Equals(state))
                    ret.Add(item);
            return ret;
        }

        public bool ContainsItem(Setup item)
        {
            foreach (var setup in this)
                if (setup.IdentifiesAs(item))
                    return true;
            return false;
        }
    }
}
