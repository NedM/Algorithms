using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.Dynamic_Programming
{
    public class KnapsackClass
    {
        //private Dictionary<Tuple<double, int>, double> subProblemHashTable;

        public KnapsackClass(int capacity = 0)
        {
            this.Capacity = capacity;            
        }

        public int Capacity { get; set; }

        public double Pack(KnapsackItem[] items)
        {
            List<KnapsackItem> dummy = new List<KnapsackItem>();
            return this.Pack(items, false, out dummy);
        }

        public double Pack(KnapsackItem[] items, out List<KnapsackItem> packedItems)
        {
            return this.Pack(items, true, out packedItems);
        }

        protected double Pack(KnapsackItem[] items, bool returnListOfItemsPacked, out List<KnapsackItem> packedItems)
        {
            double[,] subProblemValues = new double[items.Length + 1, this.Capacity + 1];
            packedItems = new List<KnapsackItem>();

            if (this.Capacity <= 0)
            {
                throw new Exception(string.Format("Knapsack capacity is currently {0} where it should be an int value greater than 0. " + 
                                                   "Please set the capacity to a positive non-zero int before retrying", this.Capacity));
            }

            for (int i = 1; i <= items.Length; i++)
            {
                for (int j = 0; j <= this.Capacity; j++)
                {
                    int shiftedIndex = i - 1;
                    double valueAtIndex;

                    if (shiftedIndex < 0)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    double itemValue = items[shiftedIndex].Value;
                    int itemSize = items[shiftedIndex].Size;

                    if (itemSize > j)
                    {
                        //Edge case, item is larger than remaining capacity
                        valueAtIndex = subProblemValues[i - 1, j];
                    }
                    else
                    {
                        //Use pre-computed sub problem answers
                        double case1 = subProblemValues[i - 1, j];                          //Case 1: i-th item is not in the optimal solution
                        double case2 = subProblemValues[i - 1, j - itemSize] + itemValue;   //Case 2: i-th item is in the optimal solution

                        valueAtIndex = Math.Max(case1, case2);
                    }

                    subProblemValues[i, j] = valueAtIndex;
                }
            }

            if (returnListOfItemsPacked)
            {
                packedItems = this.GetItemsPacked(items, subProblemValues);
            }

            return subProblemValues[items.Length, this.Capacity];
        }

        public double Pack_Recursive(KnapsackItem[] items)
        {
            List<KnapsackItem> dummy = new List<KnapsackItem>();
            return this.Pack_Recursive(items, false, out dummy);
        }

        public double Pack_Recursive(KnapsackItem[] items, out List<KnapsackItem> packedItems)
        {
            return this.Pack_Recursive(items, true, out packedItems);
        }

        protected double Pack_Recursive(KnapsackItem[] items, bool returnListOfItemsPacked, out List<KnapsackItem> packedItems)
        {
            Dictionary<Tuple<double, int>, double> subProblemHashTable = new Dictionary<Tuple<double, int>, double>();
            packedItems = new List<KnapsackItem>();
            double dReturn = -1;
            Tuple<double, int> finalKey = new Tuple<double, int>(items.Length, this.Capacity);

            if (this.Capacity <= 0)
            {
                throw new Exception(string.Format("Knapsack capacity is currently {0} where it should be an int value greater than 0. " +
                                                   "Please set the capacity to a positive non-zero int before retrying", this.Capacity));
            }

            if (items.Length <= 0)
            {
                subProblemHashTable.Add(finalKey, 0);
            }
            else if (items.Length == 1)
            {
                if (items[0].Size <= this.Capacity)
                {
                    subProblemHashTable.Add(finalKey, items[0].Value);
                }
                else
                {
                    subProblemHashTable.Add(finalKey, 0);
                }
            }
            else
            {
                int shiftedItemIndex = items.Length - 1;
                KnapsackItem lastItem = items[shiftedItemIndex];
                double case1SubAns = this.GetSubproblemSolution(shiftedItemIndex, this.Capacity, ref items, ref subProblemHashTable);                                   //Case 1: i-th item is not in the optimal solution
                double case2SubAns = this.GetSubproblemSolution(shiftedItemIndex, this.Capacity - lastItem.Size, ref items, ref subProblemHashTable) + lastItem.Value;  //Case 2: i-th item is in the optimal solution
                subProblemHashTable.Add(finalKey, Math.Max(case1SubAns, case2SubAns));
            }

            dReturn = subProblemHashTable[finalKey];

            if (returnListOfItemsPacked)
            {
                packedItems = this.GetItemsPacked(items, subProblemHashTable);
            }

            return dReturn;
        }

        public List<KnapsackItem> GetItemsPacked(KnapsackItem[] allItems, double[,] subProblemValues)
        {
            List<KnapsackItem> packedItems = new List<KnapsackItem>();
            int i = allItems.Length, j = this.Capacity, shiftedIndex = i - 1;

            //Start at end and work backwards
            while (shiftedIndex >= 0 && j >= 0)
            {
                double itemValue = allItems[shiftedIndex].Value;
                int itemSize = allItems[shiftedIndex].Size;

                if (itemSize > j)
                {
                    i = i - 1;
                }
                else
                {
                    double case1 = subProblemValues[i - 1, j];                          //Case 1: i-th item is not in the optimal solution
                    double case2 = subProblemValues[i - 1, j - itemSize] + itemValue;   //Case 2: i-th item is in the optimal solution

                    if (subProblemValues[i, j] == case1)
                    {
                        //Case 1: i-th item is not in the optimal solution
                        i = i - 1;
                    }
                    else
                    {
                        //Case 2: i-th item is in the optimal solution
                        packedItems.Add(allItems[shiftedIndex]);
                        i = i - 1;
                        j = j - itemSize;
                    }
                }
                shiftedIndex = i - 1;
            }

            return packedItems;
        }

        public List<KnapsackItem> GetItemsPacked(KnapsackItem[] allItems, Dictionary<Tuple<double, int>, double> subProblemValues)
        {
            List<KnapsackItem> packedItems = new List<KnapsackItem>();
            int i = allItems.Length, j = this.Capacity, shiftedIndex = i - 1;

            //Start at end and work backwards
            while (i > 0 && j >= 0)
            {
                double itemValue = allItems[shiftedIndex].Value;
                int itemSize = allItems[shiftedIndex].Size;

                if (itemSize > j)
                {
                    i = i - 1;
                }
                else if (subProblemValues.Keys.Count == 1)
                {
                    packedItems.Add(allItems[shiftedIndex]);
                    i = i - 1;
                    j = j - itemSize;
                }
                else
                {
                    Tuple<double, int> case1key = new Tuple<double, int>(i - 1, j);
                    Tuple<double, int> case2Key = new Tuple<double, int>(i - 1, j - itemSize);

                    if (!subProblemValues.ContainsKey(case1key) && !subProblemValues.ContainsKey(case2Key))
                    {
                        throw new Exception("Something has gone wrong! Neither key is in the collection!");
                    }
                    else if (!subProblemValues.ContainsKey(case1key))
                    {
                        //Case 1 key not in collection, must be case 2
                        packedItems.Add(allItems[shiftedIndex]);
                        i = i - 1;
                        j = j - itemSize;
                    }
                    else if (!subProblemValues.ContainsKey(case2Key))
                    {
                        //Case 2 key not in collection, must be case 1
                        i = i - 1;
                    }
                    else  //Both keys present in dictionary
                    {
                        double case1 = subProblemValues[case1key];             //Case 1: i-th item is not in the optimal solution
                        double case2 = subProblemValues[case2Key] + itemValue; //Case 2: i-th item is in the optimal solution

                        if (subProblemValues[new Tuple<double, int>(i, j)] == case1)
                        {
                            //Case 1: i-th item is not in the optimal solution
                            i = i - 1;
                        }
                        else
                        {
                            //Case 2: i-th item is in the optimal solution
                            packedItems.Add(allItems[shiftedIndex]);
                            i = i - 1;
                            j = j - itemSize;
                        }
                    }
                }
                shiftedIndex = i - 1;
            }

            return packedItems;
        }

        private double GetSubproblemSolution(int indexIntoSubProbAns, int weightIndex, ref KnapsackItem[] items, ref Dictionary<Tuple<double, int>, double> subProbs)
        {
            double dReturn = -1;
            Tuple<double, int> key;

            if (indexIntoSubProbAns < 0)
            {
                throw new IndexOutOfRangeException();
            }
            else if (indexIntoSubProbAns == 0)
            {
                key = new Tuple<double, int>(0, weightIndex);
                if (!subProbs.ContainsKey(key))
                {
                    subProbs.Add(key, 0);
                }
            }
            else
            {
                int indexIntoItemsList = indexIntoSubProbAns - 1;
                KnapsackItem item = items[indexIntoItemsList];
                key = new Tuple<double, int>(indexIntoSubProbAns, weightIndex);
                if (!subProbs.ContainsKey(key))
                {
                    if (item.Size > weightIndex)
                    {
                        //Recurse
                        double edgeCaseSubAns = this.GetSubproblemSolution(indexIntoSubProbAns - 1, weightIndex, ref items, ref subProbs);

                        if (!subProbs.ContainsKey(key))
                        {
                            subProbs.Add(key, edgeCaseSubAns);
                        }
                    }
                    else
                    {
                        //Recurse
                        double case1SubAns = this.GetSubproblemSolution(indexIntoSubProbAns - 1, weightIndex, ref items, ref subProbs);                           //Case 1: i-th item is not in the optimal solution
                        double case2SubAns = this.GetSubproblemSolution(indexIntoSubProbAns - 1, weightIndex - item.Size, ref items, ref subProbs) + item.Value;  //Case 2: i-th item is in the optimal solution
                        if (!subProbs.ContainsKey(key))
                        {
                            subProbs.Add(key, Math.Max(case1SubAns, case2SubAns));
                        }
                    }
                }
            }
            dReturn = subProbs[key];
            return dReturn;
        }
    }
}
