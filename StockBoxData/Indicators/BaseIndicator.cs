using System.Linq;
using StockBox.Data.Adapters.DataFrame;


namespace StockBox.Data.Indicators
{

    /// <summary>
    /// Class <c>BaseIndicator</c>
    /// </summary>
    public abstract class BaseIndicator : IIndicator
    {
        public BaseIndicator(string column, EIndicatorType type, params int[] indices)
        {
            ColumnKey = column;
            Indices = indices;
            _type = type;
        }

        /// <summary>
        /// The combined value of the provided column and the indicies, i.e.,
        /// SMA(25), SlowSto(14,3)
        /// </summary>
        public string Name
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(ColumnKey))
                    ret += ColumnKey;
                if (Indices != null && Indices.Length > 0)
                    ret += $"({string.Join(",", Indices)})";
                return ret;
            }
        }

        public EIndicatorType Type { get { return _type; } }

        /// <summary>
        /// Column key is the key that creates the indicator in the Factory
        /// class. Essentially the Name prop without the indices. This may be
        /// defunct.
        ///
        /// TODO - Possibly remove this property
        /// </summary>
        public string ColumnKey { get; set; }

        /// <summary>
        /// The additional context of the Indicator, for MovingAverages, this is
        /// the size of the window, i.e., SMA(*25*). For *Stochastics, it's
        /// SlowSto(*14,3*)
        /// </summary>
        public int[] Indices { get; set; }

        /// <summary>
        /// The calculated payload, usually a Dictionary with a KeyValuePair,
        /// usually, but not always, DateTime and double
        /// </summary>
        public object Payload { get { return _payload; } }
        private object _payload;
        private EIndicatorType _type;

        /// <summary>
        /// Perform the actual calculation of the indicator, setting the Payload
        /// property. The Payload is mapped downstream onto the DataPointList,
        /// which has context for the actual structure of the Payload object
        /// </summary>
        /// <param name="adapter"></param>
        public void Calculate(IDataFrameAdapter adapter)
        {
            _payload = CalculateIndicator(adapter);
        }


        /// <summary>
        /// Return true if this object identifies as the provided object.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IdentifiesAs(IIndicator item)
        {
            if (item.Name.ToLower() != Name.ToLower()) return false;
            if (item.Type != Type) return false;
            if (item.ColumnKey.ToLower() != ColumnKey.ToLower()) return false;
            if (item.Indices.Length != Indices.Length) return false;
            if (item.Indices.Intersect(Indices).Count() != Indices.Length) return false;
            return true;
        }

        /// <summary>
        /// All child calculations should be bracketted within this method and
        /// return the final response object to be set to the _payload value.
        /// The return type is `object` because not all indicators will have the
        /// same format/structure. For example, SimpleMovingAverage and RSI have
        /// the same return structure, a simple <DateTime, double> KeyValuePair,
        /// however the *Stochastic indicators have two values for each DateTime
        /// key. The indicator doesn't have to care about it's return type, the
        /// mapping will be handled downstream, specifically by Indicator type
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns></returns>
        protected abstract object CalculateIndicator(IDataFrameAdapter adapter);

    }
}
