﻿using System;


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
        public static IIndicator Create(string column, params int[] indices)
        {
            return column.ToLower() switch
            {
                "sma" => new SimpleMovingAverage(column, indices),
                "volume" => new Volume(column, indices),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
