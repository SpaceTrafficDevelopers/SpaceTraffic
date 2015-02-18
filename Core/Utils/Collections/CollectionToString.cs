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
using System.Collections;

namespace SpaceTraffic.Utils.Collections
{
    public class CollectionToString
    {
        private IEnumerable collection;

        public CollectionToString(IEnumerable collection)
        {
            this.collection = collection;
        }

        public override string ToString()
        {
            if (this.collection == null)
                return "null";

            StringBuilder sb = new StringBuilder();

            sb.Append('[');

            IEnumerator it = this.collection.GetEnumerator();
            if (it.MoveNext())
            {
                sb.Append(it.Current);
                
                while (it.MoveNext())
                {
                    sb.Append(", ");
                    sb.Append(it.Current);
                }
            }

            sb.Append(']');

            return sb.ToString();
        }
    }
}
