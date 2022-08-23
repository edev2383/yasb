using System;
using System.Collections.Generic;
using StockBox.Setups;

namespace StockBox.Models
{

    public class SymbolProfileList : List<SymbolProfile>
    {
        public SymbolProfileList()
        {
        }

        public bool ContainsItem(SymbolProfile item)
        {
            foreach (var i in this)
                if (i.IdenfifiesAs(item))
                    return true;
            return true;
        }

        /// <summary>
        /// Returns a subset of SymbolProfiles that are related to the given
        /// setup by a shared origin state
        /// </summary>
        /// <param name="setup"></param>
        /// <returns></returns>
        public SymbolProfileList FindBySetup(Setup setup)
        {
            var ret = new SymbolProfileList();
            foreach (SymbolProfile sp in this)
                if (sp.IsRelatedBySetup(setup))
                    ret.Add(sp);
            return ret;
        }
    }
}
