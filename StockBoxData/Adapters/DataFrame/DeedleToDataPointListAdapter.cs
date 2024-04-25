using System;
using System.IO;
using System.Linq;
using Deedle;
using StockBox.Data.SbFrames;
using StockBox.Validation;


namespace StockBox.Data.Adapters.DataFrame
{


    public class DeedleToDataPointListAdapter
    {
        private readonly MemoryStream _source;
        public DataPointList _payload;
        public DeedleToDataPointListAdapter(MemoryStream stream)
        {
            _source = stream;
        }

        ///// <summary>
        ///// Add data from a MemoryStream after the object has been created.
        ///// Assumes CSV format. We can add adapters for other formats if needed,
        ///// i.e., JSON
        ///// </summary>
        ///// <param name="data"></param>
        public DataPointList Convert()
        {
            // read the incoming stream as a CSV
            var rawData = Frame.ReadCsv(_source);

            // using the Deedle library functions, index and sort by date
            var tmpSorted = rawData.IndexRows<DateTime>("Date").SortRowsByKey();

            // map the deedle data structure to the domain DataPointList
            _payload = Map(tmpSorted);

            // since our data is time-series, we always want the data in desc
            // order, with the most recent date key at 0-idx.
            if (!_payload.IsDesc)
                _payload = _payload.Reversed;

            return _payload;
        }

        ///// <summary>
        ///// Map a Deedle frame into a StockBox.DataPointList object
        ///// </summary>
        ///// <param name="data"></param>
        ///// <returns></returns>
        private DataPointList Map(Frame<DateTime, string> data)
        {
            // fill missing in with a placeholder value. We can do some
            // post-processing to fill in missing values w/ averages
            data = data.FillMissing(-1);
            DataPointList ret = new DataPointList();
            var dataAsArray = data.RowKeys.ToArray();
            for (var idx = 0; idx < data.RowCount; idx++)
            {
                var key = dataAsArray[idx];
                var curr = data.Rows[key];
                var vr = ValidateObjectSeries(curr);
                // ignore any series that are missing data, the vr contains the
                // series as a ValidationObject so we could further examine for
                // additional data
                if (vr.Success)
                    ret.Add(MapDeedleObjectSeries(key, curr));
            }
            return ret;
        }

        /// <summary>
        /// Map a single row within a Deedle.Frame to a DataPoint object
        /// </summary>
        /// <param name="dateIndex"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private DataPoint MapDeedleObjectSeries(DateTime dateIndex, ObjectSeries<string> obj)
        {
            return new DataPoint
            {
                Date = dateIndex,
                High = obj.GetAs<double>("High"),
                Low = obj.GetAs<double>("Low"),
                Open = obj.GetAs<double>("Open"),
                Close = obj.GetAs<double>("Close"),
                AdjClose = obj.GetAs<double>("Adj Close"),
                Volume = obj.GetAs<double>("Volume")
            };
        }

        protected ValidationResultList ValidateObjectSeries(ObjectSeries<string> obj)
        {
            var ret = new ValidationResultList
            {
                new ValidationResult(obj.ValueCount > 1, "ObjectSeries has more than one value")
            };

            if (ret.HasFailures)
                return ret;
            ret.Add(new ValidationResult(obj.TryGet("High").Value.ToString() != "null", "ObjectSeries(`High`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Low").Value.ToString() != "null", "ObjectSeries(`Low`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Open").Value.ToString() != "null", "ObjectSeries(`Open`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Close").Value.ToString() != "null", "ObjectSeries(`Close`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Adj Close").Value.ToString() != "null", "ObjectSeries(`Adj Close`) is not null"));
            ret.Add(new ValidationResult(obj.TryGet("Volume").Value.ToString() != "null", "ObjectSeries(`Volume`) is not null"));

            if (ret.HasFailures)
                ret.Add(new ValidationResult(EResult.eFail, "ObjectSeries is missing data", obj));
            return ret;
        }
    }
}

