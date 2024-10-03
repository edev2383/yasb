using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;
using System;
using System.Collections.Generic;

namespace StockBox.Data.Indicators
{

    /// <summary>
    /// An attempt to turn the Conqueror setup into an indicator
    /// </summary>
    public class Conqueror : BaseIndicator<Dictionary<DateTime, (bool sit1, bool sit2, bool sit3)>>
    {
        public Conqueror(string column, params int[] indices) : base(column, type: EIndicatorType.conqueror, indices)
        {
        }

        protected override Dictionary<DateTime, (bool sit1, bool sit2, bool sit3)> CalculateIndicator(IDataPointListProvider provider)
        {
            var ret = new Dictionary<DateTime, (bool sit1, bool sit2, bool sit3)>();

            /// The Conqueror indicator relies on the SMA(10), so we'll check to make
            /// sure it exists. If not, we'll add it
            var sma = IndicatorFactory.Create("SMA", 10);

            if (provider.IndicatorExists(sma) != true)
                provider.Parent.AddIndicator(sma);

            provider.GetFullDataSource().Reversed.Window(
                40, x => CalculateConqueror(x, ref ret));


            return ret;
        }

        private double CalculateConqueror(DataPointList x, ref Dictionary<DateTime, (bool sit1, bool sit2, bool sit3)> conqueror)
        {
            bool sit1 = false, sit2 = false, sit3 = false;
            
            var last = x.Last();
            var todaySma  = last.Indicators.FindByKey("SMA(10)");

           
            return 0;
        }
    }
}



//1.Today's close is less than the 10-day moving average of the close.

//2.Today's 10-day moving average is less than the 10-day moving average 10 days ago.

//3.Today's close is less than the close of 40 days ago.