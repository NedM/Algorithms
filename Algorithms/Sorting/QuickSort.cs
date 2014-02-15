using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Algorithms
{
    public class QuickSort
    {
        public static void Test()
        {
            QuickSort.Test(50, 0, 100);
        }

        public static void Test(int length, int minimumVal, int maximumVal)
        {
            QuickSort qSort = new QuickSort();
            int[] array = Utilities.GenerateRandomizedArray(length, minimumVal, maximumVal);
            int[] copy = new int[array.Length];
            array.CopyTo(copy, 0);

            Stopwatch watch = new Stopwatch();
            watch.Start();
            qSort.Sort(ref array, 0, array.Length - 1);
            watch.Stop();
            TimeSpan quickSortTime = watch.Elapsed;

            List<int> comparisonList = copy.ToList();
            watch.Restart();
            comparisonList.Sort();
            watch.Stop();
            TimeSpan dotNETTime = watch.Elapsed;

            Console.WriteLine(string.Format("Unsorted: {0}{1}Sorted: {2}{1}Elapsed Time: {3}{1}Compare To: {4}{1}",
                Utilities.FormatPrintArray(copy),
                Environment.NewLine,
                Utilities.FormatPrintArray(array),
                quickSortTime.ToString(),
                dotNETTime.ToString()));
        }

        public QuickSort()
        {
            this.ComparisonCount = 0;
        }

        public ulong ComparisonCount
        {
            get;
            private set;
        }

        public void RezeroComparisonCount()
        {
            this.ComparisonCount = 0;
        }

        public void Sort(ref int[] unsortedArray, int leftIndex, int rightIndex)
        {
            if (leftIndex >= rightIndex)
            {
                return;
            }

            int pivotIndex = this.ChoosePivot(leftIndex, rightIndex);
            int partitionPoint = this.PartitionArray(ref unsortedArray, pivotIndex, leftIndex, rightIndex);

            this.Sort(ref unsortedArray, leftIndex, partitionPoint - 1); //Recurse on the left half
            this.Sort(ref unsortedArray, partitionPoint + 1, rightIndex); //and the right half

            //unsortedArray is now sorted
        }

        /// <summary>
        /// Chooses the index of the pivot point
        /// </summary>
        /// <param name="min">the minimum index of the pivot point</param>
        /// <param name="max">the maximum index of the pivot point</param>
        /// <returns>the index of the pivot point</returns>
        private int ChoosePivot(int min, int max)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(min, max);
            //int medianIndex = 0;
            //decimal half = (decimal)((max + min) / 2.0);
            //if (((max + 1) - min) % 2 == 0)
            //{
            //    medianIndex = Convert.ToInt32(Math.Floor(half));
            //}
            //else
            //{
            //    medianIndex = Convert.ToInt32(Math.Ceiling(half));
            //}
            //return medianIndex;
        }

        private int PartitionArray(ref int[] arrayToPartition, int pivotIndex, int leftmostIndex, int rightmostIndex)
        {
            //int pivot = this.FindMedianOfThreeIndicies(arrayToPartition, pivotIndex, leftmostIndex, rightmostIndex);
            int pivot = arrayToPartition[pivotIndex];
            this.Swap(ref arrayToPartition, pivotIndex, leftmostIndex);
            int j = leftmostIndex + 1;
            bool triggered = false;  //Flag to indicate that element bigger than the pivot has been encountered. Prevents unnecessary swaps

            for (int i = leftmostIndex + 1; i <= rightmostIndex; i++)
            {
                this.ComparisonCount++;
                if (arrayToPartition[i] > pivot)
                {
                    triggered = true;
                }
                else if (arrayToPartition[i] <= pivot)
                {
                    if (triggered)
                    {
                        //Swap with leftmost element that is bigger than the pivot
                        this.Swap(ref arrayToPartition, j, i);
                    }
                    //increment j
                    j++;
                }
            }

            if ((j - 1) > leftmostIndex)
            {
                //finally swap index of the pivot with it's rightful position at j
                this.Swap(ref arrayToPartition, leftmostIndex, j - 1);
            }
            return j - 1;
        }

        private void Swap(ref int[] array, int indexA, int indexB)
        {
            int temp = array[indexA];
            array[indexA] = array[indexB];
            array[indexB] = temp;
        }

        //private int FindMedianOfThreeIndicies(int[] array, int i, int j, int k)
        //{
        //    int[] values = new int[] { array[i], array[j], array[k] };
        //    MergeSort mSort = new MergeSort();
        //    return mSort.Sort(values)[1];            
        //}
    }
}
