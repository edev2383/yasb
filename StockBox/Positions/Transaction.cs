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
        public ETransactionType Type { get; set; }
        public Guid? PositionToken { get; set; }
        public Guid? Token { get; set; }
        public Transaction()
        {
        }
    }
}
