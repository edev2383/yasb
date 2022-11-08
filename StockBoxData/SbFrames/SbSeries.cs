using System;
using System.Collections.Generic;
using System.Linq;

namespace StockBox.Data.SbFrames
{

    public class SbSeries : Dictionary<DateTime, double>
    {

        public string Name { get; set; }
        public int KeyCount { get { return Keys.Count; } }


        public (DateTime? FirstKey, DateTime? LastKey) KeyRange
        {
            get
            {
                if (Count == 0) return (null, null);
                return (this.First().Key, this.Last().Key);
            }
        }

        public SbSeries(SbSeries source)
        {
            Name = source.Name;
            foreach (var kvp in source)
                Add(kvp.Key, kvp.Value);
        }

        public SbSeries(string name = null)
        {
            Name = name;
        }

        public SbSeries(Dictionary<DateTime, double> source, string name = null)
        {
            Name = name;
            foreach (var kvp in source)
                Add(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Apply [expression] across a floating subset of SbSeries with length
        /// [frame]
        /// </summary>
        /// <param name="frame"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SbSeries Window(int frame, Func<SbSeries, double> expression)
        {
            var ret = new SbSeries();
            for (var idx = 0; idx < Count - frame + 1; idx++)
            {
                var window = new SbSeries(this.Skip(idx).Take(frame).ToDictionary(k => k.Key, v => v.Value));
                ret.Add(window.Last().Key, expression(window));
            }
            return ret;
        }

        public SbSeries SortByKey()
        {
            var ret = new SbSeries();
            foreach (var item in this.OrderBy(k => k.Key))
            {
                ret.Add(item.Key, item.Value);
            }
            return ret;
        }

        /// <summary>
        /// Return a SbSeries of comparisons based on the offset
        /// </summary>
        /// <param name="offest"></param>
        /// <returns></returns>
        public SbSeries Diff(int offest)
        {
            var ret = new SbSeries();
            if (Count < offest) return ret;
            for (var idx = offest; idx < Count; idx++)
            {
                var origin = this.ElementAt(idx);
                var compare = this.ElementAt(idx - offest);
                ret.Add(origin.Key, origin.Value - compare.Value);
            }
            return ret;
        }

        public double LastValue()
        {
            return this.Last().Value;
        }

        public double Max()
        {
            return Values.Max();
        }

        public double Min()
        {
            return Values.Min();
        }

        public double Sum()
        {
            return Values.Sum();
        }

        public double Mean()
        {
            return this.Sum() / this.Count;
        }
    }
}
