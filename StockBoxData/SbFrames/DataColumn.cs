using System;
using System.Collections.Generic;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// Class <c>DataColumn</c> encapsulates and describes a column and
    /// potential indices 
    /// </summary>
    public class DataColumn
    {

        public enum EColumns
        {
            // Core columns
            eDate = 0, // index
            eOpen,
            eHigh,
            eLow,
            eClose,
            eAdjClose,
            eVolume,

            // indicator columns
            eSma,
            eAvgVolume,
            eRsi,
            eEma,
            eSloSto,
            eBBandsLower,
            eBBandsHigher,
            eBBandsCenter,
        }

        public string Column { get { return MapToColumnString(); } }
        private EColumns _column;
        private List<int> _indices = new List<int>();


        public DataColumn(string column, params int[] indices)
        {
            _column = SanitizeRawStringColumn(column);
            if (indices != null)
            {
                foreach (var item in indices)
                    _indices.Add(item);
            }
        }


        public DataColumn(EColumns column, params int[] indices)
        {
            _column = column;
            foreach (var item in indices)
                _indices.Add(item);
        }

        /// <summary>
        /// Return the enum for the targeted column
        /// </summary>
        /// <returns></returns>
        public EColumns GetColumnKey()
        {
            return _column;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string MapToColumnString()
        {
            var ret = string.Empty;

            switch (_column)
            {
                case EColumns.eDate:
                    ret = "Date";
                    break;
                case EColumns.eOpen:
                    ret = "Open";
                    break;
                case EColumns.eHigh:
                    ret = "High";
                    break;
                case EColumns.eLow:
                    ret = "Low";
                    break;
                case EColumns.eClose:
                    ret = "Close";
                    break;
                case EColumns.eAdjClose:
                    ret = "AdjClose";
                    break;
                case EColumns.eSma:
                    ret = $"SMA({string.Join(",", _indices)})";
                    break;
                case EColumns.eSloSto:
                    ret = $"SlowSto({string.Join(",", _indices)})";
                    break;
                case EColumns.eAvgVolume:
                    ret = $"AvgVolume({string.Join(",", _indices)})";
                    break;
                case EColumns.eVolume:
                    ret = "Volume";
                    break;
                case EColumns.eRsi:
                    ret = $"RSI({string.Join(",", _indices)})";
                    break;
                case EColumns.eEma:
                    break;
                case EColumns.eBBandsLower:
                    break;
                case EColumns.eBBandsHigher:
                    break;
                case EColumns.eBBandsCenter:
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown column value provided {_column}");
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        private EColumns SanitizeRawStringColumn(string column)
        {
            switch (column.ToLower())
            {
                case "open":
                    return EColumns.eOpen;
                case "low":
                    return EColumns.eLow;
                case "high":
                    return EColumns.eHigh;
                case "close":
                    return EColumns.eClose;
                case "sma":
                    return EColumns.eSma;
                case "volume":
                    return EColumns.eVolume;
                case "avgvolume":
                    return EColumns.eAvgVolume;
                case "rsi":
                    return EColumns.eRsi;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
