using System;
using System.Collections.Generic;
using System.Linq;
using StockBox.Associations;
using StockBox.Associations.Enums;

namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// 
    /// </summary>
    public class SbFrameList : List<ISbFrame>
    {

        public DataPoint Current
        {
            get
            {
                var daily = FindByFrequency(EFrequency.eWeekly);
                if (daily == null) return new DataPoint();
                return daily.FirstDataPoint();
            }
        }

        public bool HasMonthly { get { return GetMonthly() != null; } }
        public bool HasWeekly { get { return GetWeekly() != null; } }
        public bool HasDaily { get { return GetDaily() != null; } }

        public double AllTimeHigh
        {
            get
            {
                // found *should* always be monthly given that when this token
                // is present the Analyzer and FrameListFactory account for the
                // required domain tokens, however, in an abundance of caution,
                // we are going to query for the largest interval, just in case
                var found = FindGreatestAvailableFrequency();
                if (found == null) throw new Exception("SbFrame not found. Have they been added to the SbFrameList parent?");
                var series = found.ToSeries("High");
                return series.Max();
            }
        }

        public double AllTimeLow
        {
            get
            {
                // found *should* always be monthly given that when this token
                // is present the Analyzer and FrameListFactory account for the
                // required domain tokens, however, in an abundance of caution,
                // we are going to query for the largest interval, just in case
                var found = FindGreatestAvailableFrequency();
                if (found == null) throw new Exception("SbFrame not found. Have they been added to the SbFrameList parent?");
                var series = found.ToSeries("Low");
                return series.Min();
            }
        }

        public double FiftyTwoWeekHigh
        {
            get
            {
                // found *should* always be monthly given that when this token
                // is present the Analyzer and FrameListFactory account for the
                // required domain tokens, however, in an abundance of caution,
                // we are going to query for the largest interval, just in case
                var found = FindGreatestAvailableFrequency();
                if (found == null) throw new Exception("SbFrame not found. Have they been added to the SbFrameList parent?");

                // we want to get the length of time for a year based on the
                // interval of the found frame
                var length = MapFrequencyToRowInteger(found);
                var valueList = found.ToSeries("High").ToValueList();
                if (valueList.Count < length)
                    return valueList.Max();
                var range = valueList.GetRange(valueList.Count - length, length - 1);
                return range.Max();
            }
        }

        public double FiftyTwoWeekLow
        {
            get
            {
                // found *should* always be monthly given that when this token
                // is present the Analyzer and FrameListFactory account for the
                // required domain tokens, however, in an abundance of caution,
                // we are going to query for the largest interval, just in case
                var found = FindGreatestAvailableFrequency();
                if (found == null) throw new Exception("SbFrame not found. Have they been added to the SbFrameList parent?");

                // we want to get the length of time for a year based on the
                // interval of the found frame
                var length = MapFrequencyToRowInteger(found);
                var valueList = found.ToSeries("Low").ToValueList();
                if (valueList.Count < length)
                    return valueList.Min();
                var range = valueList.GetRange(valueList.Count - length, length - 1);
                return range.Min();
            }
        }


        public SbFrameList()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        public SbFrame FindByFrequency(EFrequency frequency)
        {
            SbFrame ret = null;
            foreach (var frame in this)
            {
                if (frame.Frequency == frequency)
                    ret = (SbFrame)frame;
            }
            return ret;
        }

        public SbFrameList FindAllBySymbolProvider(ISymbolProvider symbol)
        {
            var ret = new SbFrameList();
            foreach (SbFrame item in this)
                if (item.Symbol.Equals(symbol))
                    ret.Add(item);
            return ret;
        }

        public SbFrame GetMonthly()
        {
            return FindByFrequency(EFrequency.eMonthly);
        }

        public SbFrame GetWeekly()
        {
            return FindByFrequency(EFrequency.eWeekly);
        }

        public SbFrame GetDaily()
        {
            return FindByFrequency(EFrequency.eDaily);
        }

        public SbFrame FindGreatestAvailableFrequency()
        {
            if (HasMonthly) return GetMonthly();
            if (HasWeekly) return GetWeekly();
            if (HasDaily) return GetDaily();
            return null;
        }

        private int MapFrequencyToRowInteger(SbFrame frame)
        {
            switch (frame.Frequency)
            {
                case EFrequency.eMonthly:
                    return 12;
                case EFrequency.eWeekly:
                    return 52;
                case EFrequency.eDaily:
                    return 252;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Redraw the Weekly and Monthly windows based on the iterations of the
        /// Daily. 
        /// </summary>
        public void Normalize()
        {

        }
    }
}
