using System;
using System.IO;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;

namespace StockBox_UnitTests.Accessors
{
    public class DeedleBacktestAdapter_Accessor : DeedleBacktestAdapter
    {
        public DeedleBacktestAdapter_Accessor()
        {
        }

        public DeedleBacktestAdapter_Accessor(MemoryStream data) : base(data)
        {
        }

        public DataPointList Access_GetData()
        {
            return GetData();
        }
    }
}
