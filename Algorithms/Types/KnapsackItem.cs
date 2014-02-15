using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public struct KnapsackItem : IEquatable<KnapsackItem>
    {
        private double value;
        private int size;

        public KnapsackItem(double value = 0, int size = 0)
        {
            this.value = value;
            this.size = size;
        }

        public double Value { get { return this.value; } set { this.value = value; } }
        public int Size { get { return this.size; } set { this.size = value; } }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", this.Value, this.Size);
        }

        public override bool Equals(object obj)
        {
            if (obj is KnapsackItem)
            {
                return this.Equals((KnapsackItem)obj);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + (int)(this.Size * this.Value);
        }

        public bool Equals(KnapsackItem other)
        {
            return other.Value.Equals(this.Value) &&
                   other.Size.Equals(this.Size);
        }
    }
}
