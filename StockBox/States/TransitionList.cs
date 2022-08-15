using System;
using System.Collections.Generic;

namespace StockBox.States
{
    public class TransitionList : List<Transition>
    {
        public TransitionList()
        {
        }

        /// <summary>
        /// Return true if provided item, or equivalent, already exists
        /// within the list class
        /// </summary>
        /// <param name="tryTransition"></param>
        /// <returns></returns>
        public bool ContainsItem(Transition tryTransition)
        {
            foreach (Transition item in this)
                if (item.IdentifiesAs(tryTransition)) return true;
            return false;
        }
    }
}
