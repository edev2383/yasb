using System;
using StockBox.Associations;
using StockBox.Positions.Helpers;


namespace StockBox.Positions
{

    /// <summary>
    /// Class <c>Order</c> will model interactions for a given Position.
    /// Most Positions will consist of only (2) Orders, one to enter/buy
    /// and another to exit/sell. 
    /// </summary>
    public class Order
    {

        /**
         * Need to reconcile multi-buy/multi-sell positions somehow
         * 
         * */
        public ETransactionType Type { get; set; }
        public Position Position { get; set; }
        public Guid Token { get { return _token ?? (_token = Guid.NewGuid()).Value; } }
        private Guid? _token;
        public int? ShareCount { get; set; }
        public double SharePrice { get; set; }
        public DateTime Timestamp { get; set; }
        public ISymbolProvider Symbol { get; set; }


        private Order(
            int shareCount, 
            double sharePrice,
            ISymbolProvider symbol,
            ETransactionType type,
            Position position,
            DateTime? timestamp,
            Guid? token)
        {
            ShareCount = shareCount;
            SharePrice = sharePrice;
            Symbol = symbol;
            Type = type;
            Position = position;
            Timestamp = timestamp ?? DateTime.Now;
            _token = token ?? Guid.NewGuid();
        }

        public static Order Buy(ISymbolProvider symbol, int shareCount, double sharePrice, Position position, DateTime? timestamp = null, Guid? token = null)
        {
            return new Order(shareCount, sharePrice, symbol, ETransactionType.Buy, position, timestamp, token);
        }

        public static Order BuyStop(ISymbolProvider symbol, int shareCount, double sharePrice, Position position, DateTime? timestamp = null, Guid? token = null)
        {
            return new Order(shareCount, sharePrice, symbol, ETransactionType.BuyStop, position, timestamp, token);
        }

        public static Order BuyLimit(ISymbolProvider symbol, int shareCount, double sharePrice, Position position, DateTime? timestamp = null, Guid? token = null)
        {
            return new Order(shareCount, sharePrice, symbol, ETransactionType.BuyLimit, position, timestamp, token);
        }

        public static Order Sell(ISymbolProvider symbol, int shareCount, double sharePrice, Position position, DateTime? timestamp = null, Guid? token = null)
        {
            return new Order(shareCount, sharePrice, symbol, ETransactionType.Sell, position, timestamp, token);
        }

        public static Order SellStop(ISymbolProvider symbol, int shareCount, double sharePrice, Position position, DateTime? timestamp = null, Guid? token = null)
        {
            return new Order(shareCount, sharePrice, symbol, ETransactionType.SellStop, position, timestamp, token);
        }

        public static Order SellLimit(ISymbolProvider symbol, int shareCount, double sharePrice, Position position, DateTime? timestamp = null, Guid? token = null)
        {
            return new Order(shareCount, sharePrice, symbol, ETransactionType.SellLimit, position, timestamp, token);
        }

    }
}
