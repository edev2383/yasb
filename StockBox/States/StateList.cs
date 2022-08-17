using System;
using System.Collections.Generic;


namespace StockBox.States
{

    public class StateList : List<StateBase>
    {

        public StateList()
        {
        }

        /// <summary>
        /// Return true if provided item, or equivalent, already exists
        /// within the list class
        /// </summary>
        /// <param name="tryState"></param>
        /// <returns></returns>
        public bool ContainsItem(StateBase tryState)
        {
            foreach (StateBase item in this)
                if (item.IdentifiesAs(tryState)) return true;
            return false;
        }
    }
}
