/**
Copyright 2010 FAV ZCU

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

**/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace SpaceTraffic.Utils.Collections
{
    [DataContract(Name="KeyAccessibleList")]
    public abstract class KeyAccessibleSortedList<K, V> : IKeyAccessibleList<K, V>
    {
        [DataMember]
        protected SortedList<K, V> internalList = new SortedList<K, V>();

        #region IKeyAccessible indexers
        /// <summary>
        /// Gets or sets the <see cref="V"/> value with the specified key.
        /// </summary>
        [DataMember]
        public V this[K key]
        {
            get
            {
                return this.internalList[key];
            }
            set
            {
                this.internalList[key] = value;
            }
        }

        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public virtual V this[int index]
        {
            get
            {
                throw new NotSupportedException("Operation not supported.");
            }
            set
            {
                throw new NotSupportedException("Operation not supported.");
            }
        }

        /// <summary>
        /// Gets the number of star systems in this map.
        /// </summary>
        [DataMember]
        public int Count
        {
            get { return this.internalList.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether this map is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public bool IsReadOnly
        {
            get { return false; }
        }
        #endregion

        #region IKeyAccessibleList implementation
        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public int IndexOf(V item)
        {
            throw new NotSupportedException("Operation not supported.");
        }

        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public void Insert(int index, V item)
        {
            throw new NotSupportedException("Operation not supported.");
        }

        /// <summary>
        /// Operation is not supported in this implementation.
        /// </summary>
        public void RemoveAt(int index)
        {
            throw new NotSupportedException("Operation not supported.");
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        public void Add(V item)
        {
            this.internalList.Add(this.GetKeyForValue(item), item);
        }


        public void AddAll(ICollection<V> values)
        {
            //TODO: Capacity increase
            foreach (V value in values)
            {
                this.Add(value);
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            this.internalList.Clear();
        }

        /// <summary>
        /// Determines whether this list contains the specified item.
        /// </summary>
        /// <param name="item">item instance</param>
        /// <returns>
        ///   <c>true</c> if this list contains the specified item instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(V item)
        {
            return this.ContainsValue(item);
        }

        /// <summary>
        /// Determines whether this list contains the specified item.
        /// </summary>
        /// <param name="item">V instance</param>
        /// <returns>
        ///   <c>true</c> if list contains the specified item instance; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsValue(V item)
        {
            return this.internalList.ContainsValue(item);
        }

        /// <summary>
        /// Determines whether this list contains the item with specified key.
        /// </summary>
        /// <param name="key">Key of the item.</param>
        /// <returns>
        ///   <c>true</c> if this list contains the item specified by key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey(K key)
        {
            return this.internalList.ContainsKey(key);
        }

        /// <summary>
        /// Copies all Vs into the specified array from given index.
        /// </summary>
        /// <param name="array">The destination array.</param>
        /// <param name="arrayIndex">Initial index of the array.</param>
        /// <exception cref="ArgumentNullException">array is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="ArgumentException">The number of elements in this list is greater
        /// than the available space from arrayIndex to the end of the destination array.</exception>
        public void CopyTo(V[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("Array is null.");

            if (arrayIndex < 0) throw new ArgumentOutOfRangeException("Value of index cannot be negative.");

            if (array.Rank > 0)
            {
                throw new ArgumentException("Array is multidimensional");
            }
            else if ((array.Length - arrayIndex) <= this.internalList.Count)
            {
                throw new ArgumentException("Not enough space in array.");
            }

            int i = arrayIndex;
            foreach (V item in this.internalList.Values)
            {
                array[i] = item;
            }
        }
        
        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Remove( V item)
        {
            if (item == null) return false;
            K key = GetKeyForValue(item);
            V existing = this.internalList[key];

            if ((existing != null) && (Object.ReferenceEquals(existing, item)))
            {
                return this.internalList.Remove(key);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="key">key of the item.</param>
        /// <returns>true, if item was successfully removed, otherwise false.</returns>
        public bool Remove(K key)
        {
            return this.internalList.Remove(key);
        }
        #endregion

        #region IEnumerable implementation
        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<V> GetEnumerator()
        {
            return this.internalList.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.internalList.Values.GetEnumerator();
        }
        #endregion

        public abstract K GetKeyForValue(V item);
    }
}
