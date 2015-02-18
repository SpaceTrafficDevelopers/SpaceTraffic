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
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Collections;
using SpaceTraffic.Utils.Collections;

namespace SpaceTraffic.GameUi.Models.Ui
{
    /// <summary>
    /// List of tabs for jQuery UI Tab control
    /// </summary>
    public class Tabs
    {
        #region Fields
        private IKeyAccessibleList<string, TabItem> _Items;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the currently selected tab.
        /// </summary>
        /// <value>
        /// The currently selected tab.
        /// </value>
        public TabItem CurrentTab { get; set; }

        public IKeyAccessibleList<string, TabItem> Items
        {
            get { return this._Items; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Tabs"/> class.
        /// </summary>
        public Tabs()
        {
            this._Items = new TabList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tabs"/> class.
        /// </summary>
        /// <param name="count">The number of tabs.</param>
        public Tabs(int count)
        {
            this._Items = new TabList(count);
        }
        #endregion


        /// <summary>
        /// Adds the tab.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="text">The text.</param>
        /// <param name="title">The title.</param>
        /// <param name="partialViewName">PartialView name.</param>
        /// <param name="partialViewModel">PartialView model.</param>
        public void AddTab(string action, string text = "", string title = "", string partialViewName = "", object partialViewModel = null)
        {
            this.Items.Add(new TabItem(action, text, title, partialViewName, partialViewModel));
        }

        /// <summary>
        /// Tab list, used in Tabs class.
        /// </summary>
        private class TabList : IKeyAccessibleList<string, TabItem>
        {
            #region Fields
            private OrderedDictionary innerList;
            #endregion

            #region Properties
            public int Count
            {
                get { return this.innerList.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            #endregion

            #region Indexers
            public TabItem this[int index]
            {
                get
                {
                    return (TabItem)this.innerList[index];
                }
                set
                {
                    this.innerList[index] = value;
                }
            }

            public TabItem this[string key]
            {
                get
                {
                    return (TabItem)this.innerList[key];
                }
                set
                {
                    this.innerList[key] = value;
                }
            }

            #endregion

            #region Constructors

            public TabList()
            {
                this.innerList = new OrderedDictionary();
            }

            public TabList(int count)
            {
                this.innerList = new OrderedDictionary(count);
            }

            #endregion

            #region IList implementation
            public int IndexOf(TabItem item)
            {
                throw new NotSupportedException("Operation is not supported");
            }

            public void Insert(int index, TabItem item)
            {
                this.innerList.Insert(index, item.Action, item);
            }

            public void RemoveAt(int index)
            {
                this.innerList.RemoveAt(index);
            }

            public void Add(TabItem item)
            {
                this.innerList.Add(item.Action, item);
            }

            public void Clear()
            {
                this.innerList.Clear();
            }

            public bool Contains(TabItem item)
            {
                TabItem existingItem = (TabItem)this.innerList[item.Action];
                return (existingItem != null) ? item.Equals(existingItem) : false;
            }

            public void CopyTo(TabItem[] array, int arrayIndex)
            {
                this.innerList.Values.CopyTo(array, arrayIndex);
            }

            public bool Remove(TabItem item)
            {
                TabItem existingItem = (TabItem)this.innerList[item.Action];
                if (existingItem != null)
                {
                    this.innerList.Remove(item.Action);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            #endregion
            
            #region IEnumerable implementation

            public IEnumerator<TabItem> GetEnumerator()
            {
                foreach(DictionaryEntry item in this.innerList)
                {
                    yield return (TabItem)item.Value;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }
            #endregion


            public bool ContainsKey(string key)
            {
                throw new NotImplementedException();
            }

            public bool ContainsValue(TabItem value)
            {
                throw new NotImplementedException();
            }

            public bool Remove(string key)
            {
                throw new NotImplementedException();
            }

            public void AddAll(ICollection<TabItem> values)
            {
                throw new NotImplementedException();
            }
        }
        
    }
}