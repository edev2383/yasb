using System;
namespace StockBox.Data.Indicators
{
    public static class IndicatorFactory
    {
        public static IIndicator Create(string column, params int[] indices)
        {
            return column.ToLower() switch
            {
                "sma" => new SimpleMovingAverage(column, indices),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
