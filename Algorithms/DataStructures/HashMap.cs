using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class HashMap<TKey, TValue>
    {
        int numberOfBuckets;
        TKey[] keys;
        TValue[] values;

        public HashMap() : this(1000) { }

        public HashMap(int numberOfKeys)
        {
            //Set numberOfBuckets to a prime that is not too close to a power of 2 or a power of 10
            //Search internet for a good solution for the specified input size

            this.keys = new TKey[this.numberOfBuckets];
            this.values = new TValue[this.numberOfBuckets];
        }

        public ICollection<TKey> Keys
        {
            get { return this.keys; }
        }

        public void Add()
        {
            throw new NotImplementedException();
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }

        private int Hash(TKey key)
        {
            return this.Compress(this.GetHashCode(key));
        }

        private int GetHashCode(TKey key)
        {
            return 0;
        }

        private int Compress(int hashCode)
        {
            return hashCode % this.numberOfBuckets;
        }

        public void DoubleNumberOfBuckets()
        {

        }
    }
}
