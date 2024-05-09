using System;
using System.Collections.Generic;
namespace StockBox.Base.Types
{

    /// <summary>
    /// Class <c>Option</c> is a generic return type that can
    /// wrap any T
    ///
    /// Fully acknowledging that "Option" isn't a great name,
    /// however this whole idea came from Rust, and that's
    /// the name used there, so I just implemented as I saw
    /// fit while yoinking the concept. Rust obviously does
    /// at a language level and this ia syntax sugar for C#.
    /// </summary>
    public abstract class Option
    {
        public bool IsNone
        {
            get { return this is None; }
        }
    }

    /// <summary>
    /// Class <c>Option[T]</c> is the base for Some[T] that
    /// holds a value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Option<T> : Option
    {
        public T Value
        {
            get
            {
                return _value;
            }

            set
            {
                _value = value;
            }
        }

        private T _value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public Option(T value)
        {
            _value = value;

        }
    }

    /// <summary>
    /// Class <c>Some[T]</c>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Some<T> : Option<T>
    {
        public Some(T value) : base(value)
        {
        }
    }

    /// <summary>
    /// Class <c>None</c>
    /// </summary>
    public class None : Option
    {

    }

    /// <summary>
    /// Class <c>SbList[T]</c> will override where possible
    /// any list method that returns an item of type T. Instead
    /// of returning the item T, SbList will return an Option,
    /// either Some[T] if the item is found, or None if the
    /// item is not.
    ///
    /// THis will prevent index out of range errors and
    /// random nulls and ENFORCE the developer to make a
    /// decision on how to handle the no-longer-potentially
    /// null value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OptionList<T> : List<T>
    {

        /// <summary>
        /// The actual Find() method for Lists uses the
        /// Predicate delegate, while the ExList is using
        /// the Func delegate. Still researching the
        /// ramifications of this 
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public Option Find(Func<T, bool> expr)
        {
            foreach (T item in this)
                if (expr(item))
                    return new Some<T>(item);
            return new None();
        }

        /// <summary>
        /// Return None if list has zero elements, otherwise
        /// return Some<T> of the first element
        /// </summary>
        /// <returns></returns>
        public Option First()
        {
            if (Count == 0) return new None();
            return new Some<T>(base[0]);
        }

        /// <summary>
        /// Return the Option for the element at the
        /// given index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Option ElementAt(int index)
        {
            return this[index];
        }

        /// <summary>
        /// Use to set a value at a particular index. Index-range safe, appends
        /// new value to the end of the OptionList if the index is out of range
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="value"></param>
        public void SetAt(int idx, T value)
        {
            if (idx >= Count)
                Add(value);
            else
                base[idx] = value;
        }

        /// <summary>
        /// Override the list index getter. Currently no `set`
        /// as it wants an Option to set the value, which would
        /// mean setting an index would be
        /// 
        /// mylist[5] = new Some<T>(value)
        ///
        /// The problem is the list is not a list of Options,
        /// but a list of T, so this causes some confusion. use SetAt(int, T)
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public new Option this[int i]
        {
            get
            {
                if (Count == 0) return new None();
                if (i >= Count) return new None();
                return new Some<T>(base[i]);
            }
        }
    }
}