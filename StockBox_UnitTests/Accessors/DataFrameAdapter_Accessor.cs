using System;
using StockBox.Data.Adapters.DataFrame;
using StockBox.Data.SbFrames;

namespace StockBox_UnitTests.Accessors
{
    public class DataFrameAdapter_Accessor : BaseDataFrameAdapter
    {
        public DataFrameAdapter_Accessor()
        {
            _data = new DataPointList();
        }

        public override IDataFrameAdapter Create()
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
    }
}
