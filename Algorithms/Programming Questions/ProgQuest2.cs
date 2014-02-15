using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.ProgrammingQuestions
{
    public class ProgQuest2
    {
        public const string pathToArrayFile = @"C:\Users\nedmurp\Documents\Programming\C#\Code\Algorithms\PQ2_IntegerArray.txt";

        public static ulong QuickSortAndCountComparisons(int[] array)
        {
            QuickSort qs = new QuickSort();
            qs.Sort(ref array, 0, array.Length - 1);
            Console.WriteLine(string.Format("ComparisonCount: {0}", qs.ComparisonCount));
            return qs.ComparisonCount;
        }
    }
}
