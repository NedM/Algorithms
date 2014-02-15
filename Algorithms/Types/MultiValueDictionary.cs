using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public class MultiValueDictionary<TKey, TValue> : Dictionary<TKey, List<TValue>>, System.Collections.IDictionary
    {
        public MultiValueDictionary() : base() { }
        public MultiValueDictionary(int capacity) : base(capacity) { }

        public void Add(TKey key, TValue value)
        {
            if (base.ContainsKey(key))
            {
                base[key].Add(value);
            }
            else
            {
                base.Add(key, new List<TValue>() { value });
            }
        }

        public void RemoveValue(TKey key, TValue value)
        {
            if (base[key].Contains(value))
            {
                base[key].Remove(value);
            }
        }

        public TKey GetKeyAtIndex(int index)
        {
            return this.Keys.ElementAt(index);
        }

        public TKey GetKey(TKey key)
        {
            return (from k in this.Keys
                    where k.Equals(key)
                    select k).FirstOrDefault();
        }

        public ICollection<TValue> ValuesAsFlatList
        {
            get
            {
                ICollection<TValue> collection = new List<TValue>();

                foreach (List<TValue> list in base.Values)
                {
                    foreach(TValue val in list)
                    {
                        collection.Add(val);
                    }
                }
                return collection;
            }
        }

        public void Add(KeyValuePair<TKey, List<TValue>> item)
        {
            this.Add(item.Key, item.Value);
        }

        public void CopyTo(KeyValuePair<TKey, List<TValue>>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool IsReadOnly
        {
            get { throw new NotImplementedException(); }
        }

        public bool Remove(KeyValuePair<TKey, List<TValue>> item)
        {
            return base.Remove(item.Key);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.Keys.Count);
            sb.Append("Dictionary:" + Environment.NewLine + "{" + Environment.NewLine);
            foreach (TKey k in this.Keys)
            {
                sb.AppendFormat(" {0}, [ ", k.ToString());
                int valueCount = this[k].Count;
                for(int i = 0; i < valueCount; i++)
                {
                    sb.Append(this[k][i].ToString());
                    if (i != (valueCount - 1))
                    {
                        sb.Append(", ");
                    }
                }

                sb.Append(" ]" + Environment.NewLine);
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}
