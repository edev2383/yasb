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

        public DeedleBacktestAdapter_Accessor(DataPointList data) : base(data)
        {
        }

        public DeedleBacktestAdapter_Accessor(MemoryStream data) : base(data)
        {
        }

        public DataPointList Access_GetData()
        {
            return GetData();
        }

        public DeedleBacktestAdapter_Accessor CloneWithSubsetOfData(DateTime startDate, DateTime? endDate = null)
        {
            var datapoints = GetData();
            var startIndex = datapoints.FindIndex(x => x.Date == startDate);
            var count = datapoints.Count - startIndex;
            if (endDate != null)
            {
                var endIndex = datapoints.FindIndex(x => x.Date == (DateTime)endDate);
                count = datapoints.Count - endIndex;
            }
            return new DeedleBacktestAdapter_Accessor(new DataPointList(datapoints.GetRange(startIndex, count)));
        }
    }
}
