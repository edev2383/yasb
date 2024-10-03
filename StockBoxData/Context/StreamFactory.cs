using StockBox.Associations;
using StockBox.Associations.Enums;
using System;


namespace StockBox.Data.Context
{

    /// <summary>
    /// Create and return an IStreamProvider object. Currently, it's just the
    /// one parser, but this will expand as we add some redundancies.
    /// </summary>
    [Obsolete]
    public class StreamFactory
    {
        public static IStreamProvider Create(string symbol, EFrequency frequency, DateTime startDate, DateTime? endDate = null)
        {
            throw new NotImplementedException();
            //return CreateNasdaqStream(symbol, frequency, startDate, endDate);
        }

    }
}
