using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Algorithms
{
    public class MergeSort
    {
        public static void Test()
        {
            MergeSort.Test(25, 0, 500);
        }

        public static void Test(int length, int minimumVal, int maximumVal)
        {
            MergeSort mSort = new MergeSort();
            int[] array = Utilities.GenerateRandomizedArray(length, minimumVal, maximumVal);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            int[] sortedArray = mSort.Sort(array);
            watch.Stop();
            TimeSpan mergeSortTime = watch.Elapsed;

            List<int> comparisonList = array.ToList();
            watch.Restart();
            comparisonList.Sort();
            watch.Stop();
            TimeSpan dotNETTime = watch.Elapsed;

            Console.WriteLine(string.Format("Unsorted: {0}{1}Sorted: {2}{1}Elapsed Time: {3}{1}Compare To: {4}{1}",
                Utilities.FormatPrintArray(array),
                Environment.NewLine,
                Utilities.FormatPrintArray(sortedArray),
                mergeSortTime.ToString(),
                dotNETTime.ToString()));
        }

        public MergeSort()
        {
            this.InversionCount = 0;
        }

        public uint InversionCount
        {
            get;
            private set;
        }

        /// <summary>
        /// ALGORITHM: Merge sort
        /// </summary>
        /// <param name="unsortedArray">The array of integers to be sorted</param>
        /// <returns>The sorted array</returns>
        public int[] Sort(int[] unsortedArray)
        {
            if (unsortedArray.Length < 2)
            {
                return unsortedArray;
            }

            int[] sortedArray = new int[unsortedArray.Length];
            int[] left = new int[] { };
            int[] right = new int[] { };

            //Split the array into two smaller sub arrays at the split point
            this.SplitArray(unsortedArray, out left, out right);

            //Recursively sort the left and right sub arrays
            left = this.Sort(left);
            right = this.Sort(right);

            //Merge the results of those recursive calls back into the result
            sortedArray = this.DoMerge(left, right);

            //return the sorted array
            return sortedArray;
        }

        public void ResetInversionCount()
        {
            this.InversionCount = 0;
        }

        private void SplitArray(int[] inputArray, out int[] leftArray, out int[] rightArray)
        {
            //Random rand = new Random((int)DateTime.Now.Ticks);
            //int splitPoint = (int)rand.Next(1, inputArray.Length - 1);
            int splitPoint = (int)Math.Floor(inputArray.Length / 2.0);
            leftArray = new int[splitPoint];
            rightArray = new int[inputArray.Length - splitPoint];

            for (int i = 0; i < splitPoint; i++)
            {
                leftArray[i] = inputArray[i];
            }

            for (int j = splitPoint; j < inputArray.Length; j++)
            {
                rightArray[j - splitPoint] = inputArray[j];
            }

            //Console.WriteLine(string.Format("Input: {0}{1}Split Point: {2}{1}Left: {3}{1}Right:{4}{1}",
            //    FormatPrintArray(inputArray),
            //    Environment.NewLine,
            //    splitPoint,
            //    FormatPrintArray(leftArray),
            //    FormatPrintArray(rightArray)));
        }

        private int[] DoMerge(int[] leftArray, int[] rightArray)
        {
            int[] output = new int[(leftArray.Length + rightArray.Length)];
            int i = 0; //iterator for left array
            int j = 0; //iterator for right array
            int k = 0; //iterator for output array

            while (k < output.Length)
            {
                if (i == leftArray.Length)
                {
                    output[k] = rightArray[j];
                    j++;
                    k++;
                    continue;
                }

                if (j == rightArray.Length)
                {
                    output[k] = leftArray[i];
                    i++;
                    k++;
                    continue;
                }

                if (leftArray[i] <= rightArray[j])
                {
                    output[k] = leftArray[i];
                    i++;
                }
                else //leftArray[i] > rightArray[j]
                {
                    //Count the inversions
                    //Increase the inversion count by the number of items remaining in the left array
                    uint numItemsRemainingInLeft = (uint)(leftArray.Length - i);
                    this.InversionCount += numItemsRemainingInLeft;
                    //End count the inversions
                    output[k] = rightArray[j];
                    j++;
                }
                k++;
            }

            //Console.WriteLine(string.Format("Left: {0}{1}Right: {2}{1}Merged: {3}{1}",
            //    FormatPrintArray(leftArray),
            //    Environment.NewLine,
            //    FormatPrintArray(rightArray),
            //    FormatPrintArray(output)));

            return output;
        }
    }
}
