using System;
using StockBox.Positions.Helpers;


namespace StockBox.Positions
{

    /// <summary>
    /// Class <c>Transaction</c> will model interactions for a given Position.
    /// Most Positions will consist of only (2) Transactions, one to enter/buy
    /// and another to exit/sell. 
    /// </summary>
    public class Transaction
    {

        /**
         * Need to reconcile multi-buy/multi-sell positions somehow
         * 
         * */
        public ETransactionType Type { get; set; }
        public Guid? PositionToken { get; set; }
        public Guid? Token { get; set; }
        public int? ShareCount { get; set; }
        public double SharePrice { get; set; }

        public Transaction()
        {
        }

        public Transaction(int shareCount, double sharePrice)
        {
            ShareCount = shareCount;
            SharePrice = sharePrice;
        }
    }
}
