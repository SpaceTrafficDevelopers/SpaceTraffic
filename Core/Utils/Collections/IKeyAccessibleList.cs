using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceTraffic.Utils.Collections
{
    public interface IKeyAccessibleList<K, T> : IList<T>
    {
        T this[K key]
        {
            get;
            set;
        }

        bool ContainsKey(K key);
        bool ContainsValue(T value);

        bool Remove(K key);

        void AddAll(ICollection<T> values);
    }
}
