using System;
using StockBox.Data.SbFrames;
using StockBox.Data.SbFrames.Helpers;
using StockBox.Data.SbFrames.Providers;


namespace StockBox_UnitTests.Accessors
{

    /// <summary>
    /// 
    /// </summary>
    public class DataProvider_Accessor : BaseDataProvider
    {
        public DataProvider_Accessor()
        {
            _data = new DataPointList();
        }

        public DataProvider_Accessor(DataPointList data) : base(data)
        {
        }

        public override IDataPointListProvider Create()
        {
            throw new NotImplementedException();
        }

        public void CreateAndAddDataPoint(DateTime date, double h, double l, double o, double c, double v, string indicatorKey = null, double? indicatorValue = null)
        {
            _data.Add(CreateDataPoint(date, h, l, o, c, v, indicatorKey, indicatorValue));
        }

        public DataPoint CreateDataPoint(DateTime date, double h, double l, double o, double c, double v, string indicatorKey = null, double? indicatorValue = null)
        {
            var ret = new DataPoint(date, h, l, o, c, c, v);
            if (indicatorKey != null)
            {
                ret.Indicators.Add(new IndicatorDataPoint(indicatorKey, indicatorValue));
            }
            return ret;
        }

        public DataPointList Access_GetData()
        {
            return GetData();
        }


        public override DataPointList GetData()
        {
            return _data;
        }
    }
}
