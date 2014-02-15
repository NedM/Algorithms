using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms
{
    public class ProgQuest5
    {
        public static int[] targetSums = { 231552, 234756, 596873, 648219, 726312, 981237, 988331, 1277361, 1283379 };
        public static int[] targetSums_Test = { 25, 54, 66, 81, 90, 100 };
        public static string pathToFile = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ5_Array.txt";
        public static string pathToFile0 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ5_Test.txt";

        public static KeyValuePair<bool, System.Windows.Point> OptimalTwoSumProblem(Dictionary<int, bool> hashmap, int targetSum)
        {
            bool sumExists = false;
            int a = int.MinValue;
            int remainder = int.MinValue;
            System.Windows.Point summands;

            foreach (int key in hashmap.Keys)
            {
                a = key;
                remainder = targetSum - key;

                if (hashmap.ContainsKey(remainder))
                {
                    sumExists = true;
                    hashmap[key] = true;
                    hashmap[remainder] = true;
                    break;
                }
            }

            if (sumExists)
            {
                summands = new System.Windows.Point(a, remainder);
            }
            else
            {
                summands = new System.Windows.Point();
            }

            return new KeyValuePair<bool,System.Windows.Point>(sumExists, summands);
        }

        public static KeyValuePair<bool, System.Windows.Point> SubOptimalTwoSumSolution(int[] array, int targetSum)
        {
            int[] sortedArray = array;
            bool sumExists = false;
            int a = int.MinValue;
            int remainder = int.MinValue;
            System.Windows.Point summands = new System.Windows.Point();

            //sort input array
            QuickSort qs = new QuickSort();
            qs.Sort(ref sortedArray, 0, sortedArray.Length - 1);

            for (int i = 0; i < sortedArray.Length; i++)
            {
                a = sortedArray[i];
                remainder = targetSum - sortedArray[i];
                for (int j = i + 1; j < sortedArray.Length; j++)
                {
                    int current = sortedArray[j];
                    if (current < remainder)
                    {
                        continue;
                    }
                    else if (current == remainder)
                    {
                        sumExists = true;
                        break;
                    }
                    else
                    {
                        break;
                    }
                }

                if (sumExists)
                {
                    summands = new System.Windows.Point(a, remainder);
                    break;
                }
            }

            return new KeyValuePair<bool, System.Windows.Point>(sumExists, summands);
        }

        public static void Test_Do2SumProblem()
        {
            const int numTargets = 6;
            Dictionary<int, KeyValuePair<bool, System.Windows.Point>> sumExistsDictionary = new Dictionary<int, KeyValuePair<bool, System.Windows.Point>>(numTargets);
            Dictionary<int, bool> summands = Utilities.ReadFileIntoDictionary(ProgQuest5.pathToFile0);
            //int[] summands = Utilities.ReadFileIntoArray(ProgQuest5.pathToFile0);

            for (int i = 0; i < numTargets; i++)
            {
                sumExistsDictionary[ProgQuest5.targetSums_Test[i]] = ProgQuest5.OptimalTwoSumProblem(summands, ProgQuest5.targetSums_Test[i]);
                //sumExistsDictionary[ProgQuest5.targetSums_Test[i]] = ProgQuest5.SubOptimalTwoSumSolution(summands, ProgQuest5.targetSums_Test[i]);
            }

            Console.WriteLine(string.Format("Targets: {0}{1}Sums Exist:{1}{2}", Utilities.FormatPrintArray<int>(ProgQuest5.targetSums_Test), Environment.NewLine,
                FormatPrintSumSolution(sumExistsDictionary)));
        }

        public static string FormatPrintSumSolution(Dictionary<int, KeyValuePair<bool, System.Windows.Point>> sumExistsDictionary)
        {
            StringBuilder sb = new StringBuilder();

            foreach (int key in sumExistsDictionary.Keys)
            {
                string summandsString = sumExistsDictionary[key].Key ? string.Format("{0}, Summands: ({1}, {2})", sumExistsDictionary[key].Key, sumExistsDictionary[key].Value.X, sumExistsDictionary[key].Value.Y) : sumExistsDictionary[key].Key.ToString();
                sb.AppendFormat("Target sum: {0}, Exists? {1}{2}", key, summandsString, Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
