using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBox.Data.Scraper.Helpers
{
    public class AlphaVantageApiResources
    {


        public readonly string UrlSecurityHistory = "https://www.alphavantage.co/query?function={{Frequency}}&symbol={{Symbol}}&apikey={{ApiKey}}&outputsize={{OutputSize}}&datatype={{DataType}}";
        public readonly string FrequencyDaily = "TIME_SERIES_DAILY";
        public readonly string FrequencyWeekly = "TIME_SERIES_WEEKLY";
        public readonly string FrequencyMonthly = "TIME_SERIES_MONTHLY";


        public readonly string UrlForexHistory = "https://www.alphavantage.co/query?function={{Frequency}}&from_symbol={{FromSymbol}}&to_symbol={{ToSymbol}}&apikey={{ApiKey}}&outputsize={{OutputSize}}&datatype={{DataType}}";
        public readonly string ForexDaily = "FX_DAILY";
        public readonly string ForexWeekly = "FX_WEEKLY";
        public readonly string ForexMonthly = "FX_MONTHLY";


        public readonly string OutputSizeCompact = "compact";
        public readonly string OutputSizeFull = "full";

        public readonly string DataTypeJson = "json";
        public readonly string DataTypeCsv = "csv";

        public readonly string ApiKey = "LRNCQ2VNKUGOY7G6";
    }
}
