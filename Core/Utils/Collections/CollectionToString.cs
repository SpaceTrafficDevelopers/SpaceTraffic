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
