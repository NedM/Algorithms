using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public struct WeightedJob : IComparable, IEquatable<WeightedJob>
    {
        private int weight;
        private int length;

        public WeightedJob(int weight = 0, int length = 0)
        {
            this.weight = weight;
            this.length = length;
        }

        public int Weight { get { return this.weight; } }
        public int Length { get { return this.length; } }

        /// <summary>
        /// This calculation of Job priority takes the difference of the Job weight - Job length
        /// </summary>
        //public int JobPriority { get { return this.Weight - this.Length; } }

        /// <summary>
        /// This calculation of Job priority takes the ratio of the ((Job weight)/(Job length))
        /// </summary>
        public double JobPriority { get { return ((double)this.Weight / (double)this.Length); } }

        /// <summary>
        /// Compares the current Job to another Job object and returns a value to indicate whether the current Job is 
        /// "before" or "after" the other job based on the priority and tie breaking logic.
        /// </summary>
        /// <param name="obj">another Job object</param>
        /// <returns>a negative number indicates the current job is "before" the other job
        /// a positive number indicates the current job is "after" the other job
        /// zero indicates the current job is concurrent with the other job</returns>
        public int CompareTo(object obj)
        {
            int rInt;
            if (!(obj is WeightedJob))
            {
                throw new ArgumentException("Comparison to weighted job is not defined for type " + obj.GetType().ToString());
            }

            WeightedJob wj = (WeightedJob)obj;
            if (this.JobPriority != wj.JobPriority)
            {
                rInt = this.JobPriority.CompareTo(wj.JobPriority);
            }
            else
            {
                rInt = this.Weight.CompareTo(wj.Weight);               
            }

            if (0 == rInt)
            {
                rInt = 1;
            }

            return rInt;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}]", this.Weight, this.Length);
        }

        public bool Equals(WeightedJob other)
        {
            return other.Length.Equals(this.Length) && other.Weight.Equals(this.Weight);
        }
    }
}
