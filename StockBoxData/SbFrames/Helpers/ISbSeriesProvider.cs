using System;
namespace StockBox.Data.SbFrames.Helpers
{
    public interface ISbSeriesProvider
    {

        /// <summary>
        /// Return an SbSeries for a specific column header
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        SbSeries GetSeries(string column);
    }
}

