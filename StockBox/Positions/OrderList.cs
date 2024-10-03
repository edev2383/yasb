using System;
using System.Collections.Generic;
using StockBox.Positions.Helpers;

namespace StockBox.Positions
{


    public class OrderList : List<Order>
    {
        public OrderList()
        {
        }

        public OrderList FindByPositionToken(Guid? token)
        {
            var ret = new OrderList();
            foreach (Order o in this)
                if (o.Position.Token == token)
                    ret.Add(o);
            return ret;
        }

        public bool HasOpenTransaction()
        {
            return FindBuys().Count > FindSells().Count;
        }

        public OrderList FindByType(ETransactionType type)
        {
            var ret = new OrderList();
            foreach (Order t in this)
                if (t.Type == type)
                    ret.Add(t);
            return ret;
        }

        public OrderList FindBuys()
        {
            return FindByType(ETransactionType.Buy);
        }

        public OrderList FindSells()
        {
            return FindByType(ETransactionType.Sell);
        }


    }
}
