using System;
using System.Collections.Generic;
using System.Linq;


namespace StockBox.Data.SbFrames
{

    /// <summary>
    /// Class <c>SbSeries</c> is a wrap around a dictionary, which can have
    /// expressions applied to a window-framed subset of data.
    /// </summary>
    public class SbSeries : Dictionary<DateTime, double>
    {

        /// <summary>
        /// A user-readable string name context for the SbSeries object
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The number of keys in the dictionary
        /// </summary>
        public int KeyCount { get { return Keys.Count; } }

        /// <summary>
        /// The first and last DateTime keys within the entire dict
        /// </summary>
        public (DateTime? FirstKey, DateTime? LastKey) KeyRange
        {
            get
            {
                if (Count == 0) return (null, null);
                return (this.First().Key, this.Last().Key);
            }
        }

        /// <summary>
        /// Cloning constructor
        /// </summary>
        /// <param name="source"></param>
        public SbSeries(SbSeries source)
        {
            Name = source.Name;
            foreach (var kvp in source)
                Add(kvp.Key, kvp.Value);
        }

        /// <summary>
        /// Name-only constructor
        /// </summary>
        /// <param name="name"></param>
        public SbSeries(string name = null)
        {
            Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="name"></param>
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


        /// <summary>
        /// Return a new SbSeries object that is stored by DateTime [key] value
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Return the last entrie's [value] 
        /// </summary>
        /// <returns></returns>
        public double LastValue()
        {
            return this.Last().Value;
        }

        /// <summary>
        /// Return the first entrie's [valuel]
        /// </summary>
        /// <returns></returns>
        public double FirstValue()
        {
            return this.First().Value;
        }

        /// <summary>
        /// Return the maximum value
        /// </summary>
        /// <returns></returns>
        public double Max()
        {
            return Values.Max();
        }

        /// <summary>
        /// Return the minimum value
        /// </summary>
        /// <returns></returns>
        public double Min()
        {
            return Values.Min();
        }

        /// <summary>
        /// Return the sum of values within the entire range
        ///
        /// TODO: Test if null breaks that
        /// </summary>
        /// <returns></returns>
        public double Sum()
        {
            return Values.Sum();
        }

        /// <summary>
        /// Return the average value within the entire range
        /// </summary>
        /// <returns></returns>
        public double Mean()
        {
            return this.Sum() / this.Count;
        }
    }
}
