using System;
namespace StockBox.Data.Indicators
{
    public static class IndicatorFactory
    {
        public static IIndicator Create(string column, params int[] indices)
        {
            switch(column.ToLower())
            {
                case "sma":
                    return new SimpleMovingAverage(column, indices);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
