using System;
using System.Collections.Generic;


namespace StockBox.Data.SbFrames
{
    public class Row : List<DataPoint>
    {

        public DateTime FirstKey { get
            {
                return this[0].Date;
            } }

        public Row()
        {
        }

        public DataPoint FindByIndex(int index)
        {
            return FindByDate(GetTargetDate(index));
        }

        public DataPoint FindByDate(DateTime date)
        {
            DataPoint ret = null;

            foreach (DataPoint item in this)
                if (item.Date == date)
                    ret = item;

            return ret;
        }

        private DateTime GetTargetDate(int index)
        {
            return FirstKey.AddDays(-index);
        }
    }
}
