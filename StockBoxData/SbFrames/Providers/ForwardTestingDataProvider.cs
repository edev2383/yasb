using StockBox.Data.SbFrames.Helpers;


namespace StockBox.Data.SbFrames.Providers
{

    /// <summary>
    /// 
    /// </summary>
    public class ForwardTestingDataProvider : BaseDataProvider
    {

        public ForwardTestingDataProvider() { }

        public ForwardTestingDataProvider(DataPointList source) : base(source)
        {
        }

        public override IDataPointListProvider Create()
        {
            return new ForwardTestingDataProvider();
        }

        public override DataPointList GetData()
        {
            return _data;
        }
    }
}

