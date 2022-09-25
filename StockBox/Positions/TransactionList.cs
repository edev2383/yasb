using System;
using System.Collections.Generic;


namespace StockBox.Positions
{


    public class TransactionList : List<Transaction>
    {
        public TransactionList()
        {
        }

        public TransactionList FindByPositionToken(Guid? token)
        {
            var ret = new TransactionList();
            foreach (Transaction t in this)
                if (t.PositionToken == token)
                    ret.Add(t);
            return ret;
        }
    }
}
