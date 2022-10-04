using System;
using System.Collections.Generic;
using StockBox.Positions.Helpers;

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

        public bool HasOpenTransaction()
        {
            return FindBuys().Count > FindSells().Count;
        }

        public TransactionList FindByType(ETransactionType type)
        {
            var ret = new TransactionList();
            foreach (Transaction t in this)
                if (t.Type == type)
                    ret.Add(t);
            return ret;
        }

        public TransactionList FindBuys()
        {
            return FindByType(ETransactionType.eBuy);
        }

        public TransactionList FindSells()
        {
            return FindByType(ETransactionType.eSell);
        }


    }
}
