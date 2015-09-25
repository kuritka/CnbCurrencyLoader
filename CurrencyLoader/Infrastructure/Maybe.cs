using System.Collections;
using System.Collections.Generic;


namespace CurrencyLoader.Infrastructure
{
    /// <summary>
    /// Return type. User of class decides what to return 
    /// i.e. 
    /// .DefaultIfEmpty(string.Empty).Single()
    /// .DefaultIfEmpty("none").Single()
    /// .DefaultIfEmpty(null).Single()
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _values;

        public Maybe()
        {
            _values = new T[0];
        }

        public Maybe(T value)
        {
            _values = new[] { value };
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
