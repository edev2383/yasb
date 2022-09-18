using System;
using System.Collections.Generic;
using System.Linq;

namespace StockBox.Data.SbFrames
{
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
            eVolumeSma,
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
                case EColumns.eVolumeSma:
                    ret = $"Volume({string.Join(",", _indices)})";
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown column value provided {_column}");
            }

            return ret;
        }

        public EColumns GetColumnKey()
        {
            return _column;
        }

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
                    return EColumns.eVolumeSma;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
