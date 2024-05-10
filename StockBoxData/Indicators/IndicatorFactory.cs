using System;


namespace StockBox.Data.Indicators
{
    /// <summary>
    /// Straightforward abstract factory class, creating an indicator
    /// object based on provided column string and indices
    /// 
    /// Note: When adding an indicator, it is necessary to update
    /// the following:
    ///
    /// DataColumn.MapToColumString
    /// DataColumn.SanitizeRawStringColumn
    /// DataColumn.EColumns
    /// DataPointList.MapIndicator (specific to provided indicator)
    /// </summary>
    public static class IndicatorFactory
    {

        /// <summary>
        /// Return the request indicator
        /// </summary>
        /// <param name="column">This is the code/abbreviation for the indicator, i.e., 
        /// SMA = SimpleMovingAverage, ATR = AverageTrueRange, etc</param>
        /// <param name="indices">Relevant indices, if any, for the indicator, i.e., SMA20 = 20</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static IIndicator Create(string column, params int[] indices)
        {
            return column.ToLower() switch
            {
                "sma" => new SimpleMovingAverage(column, indices),
                "avgvolume" => new AverageVolume(column, indices),
                "rsi" => new RelativeStrengthIndex(column, indices),
                "slowsto" => new SlowStochastic(column, indices),
                "faststo" => new FastStochastic(column, indices),
                "atr" => new AverageTrueRange(column, indices),
                "pc" => new PriceChannel(column, indices),
                "chan" => new PriceChannel(column, indices),
                _ => throw new ArgumentOutOfRangeException(),
            }; ;
        }
    }
}
