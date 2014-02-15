using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.ProgrammingQuestions
{
    public class ProgQuest1
    {
        public const string pathToArrayFile = @"C:\Users\nedmurp\Documents\Programming\C#\Code\Algorithms\PQ1_IntegerArray.txt";

        public static uint CountInversions_BruteForce(int[] arrayWithInversions)
        {
            uint count = 0;
            
            //Step through the array and collect 
            for (int i = 0; i < (arrayWithInversions.Length - 1); i++)
            {
                for (int j = i; j < arrayWithInversions.Length; j++)
                {
                    int a = arrayWithInversions[i];
                    int b = arrayWithInversions[j];
                    if (a > b)
                    {
                        //Console.WriteLine(string.Format("{0} at index {1} is greater than {2} at index {3}!", a, i, b, j));
                        count++;
                    }
                }
            }

            return count;
        }

        public static uint CountInversions_MergeSort(int[] arrayWithInversions)
        {
            MergeSort mSort = new MergeSort();
            
            mSort.Sort(arrayWithInversions);
            return mSort.InversionCount;
        }
    }
}
