using System;
using System.IO;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Providers;

namespace StockBox_UnitTests.Accessors
{
    public class BackwardTestingDataProvider_Accessor : BackwardTestingDataProvider
    {
        public BackwardTestingDataProvider_Accessor()
        {
        }

        public BackwardTestingDataProvider_Accessor(DataPointList data) : base(data)
        {
        }

        public DataPointList Access_GetData()
        {
            return GetData();
        }

        public BackwardTestingDataProvider_Accessor CloneWithSubsetOfData(DateTime startDate, DateTime? endDate = null)
        {
            var datapoints = GetData();
            var startIndex = datapoints.FindIndex(x => x.Date == startDate);
            var count = datapoints.Count - startIndex;
            if (endDate != null)
            {
                var endIndex = datapoints.FindIndex(x => x.Date == (DateTime)endDate);
                count = datapoints.Count - endIndex;
            }
            return new BackwardTestingDataProvider_Accessor(new DataPointList(datapoints.GetRange(startIndex, count)));
        }
    }
}
