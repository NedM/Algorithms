using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;
using Algorithms.Dynamic_Programming;

namespace Algorithms.Programming_Questions
{
    public class Algorithms2_ProgQuest3
    {
        private const string PathToFileQ1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ3_Q1_ItemArray.txt";
        private const string PathToFileQ2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ3_Q2_ItemArray.txt";

        private const string PathToTestFile0 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ3_Q1_Test0.txt";
        private const string PathToTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ3_Q1_Test1.txt";
        private const string PathToTestFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ3_Q1_Test2.txt";

        private static int DoKnapsackProblem(int knapsackCap, KnapsackItem[] itemsToPack, bool getListOfItemsPacked = true)
        {
            double dReturn = -1;
            List<KnapsackItem> packedItems = new List<KnapsackItem>();
            KnapsackClass sack = new KnapsackClass(knapsackCap);

            if (getListOfItemsPacked)
            {
                dReturn = sack.Pack(itemsToPack, out packedItems);
                Console.WriteLine(string.Format("Total value of packed items: {0}{1}Packed {3} items:{1}{2}", dReturn, Environment.NewLine, Utilities.FormatPrintList<KnapsackItem>(packedItems, true), packedItems.Count));
            }
            else
            {
                dReturn = sack.Pack(itemsToPack);
                Console.WriteLine(string.Format("Total value of packed items: {0}{1}", dReturn, Environment.NewLine));
            }
            return (int)dReturn;
        }

        private static int DoKnapsackProblem_Recursive(int knapsackCap, KnapsackItem[] itemsToPack, bool getListOfItemsPacked = true)
        {
            double dReturn = -1;
            List<KnapsackItem> packedItems = new List<KnapsackItem>();
            KnapsackClass sack = new KnapsackClass(knapsackCap);

            if (getListOfItemsPacked)
            {
                dReturn = sack.Pack_Recursive(itemsToPack, out packedItems);
                Console.WriteLine(string.Format("Total value of packed items: {0}{1}Packed {3} items:{1}{2}", dReturn, Environment.NewLine, Utilities.FormatPrintList<KnapsackItem>(packedItems, true), packedItems.Count));
            }
            else
            {
                dReturn = sack.Pack_Recursive(itemsToPack);
                Console.WriteLine(string.Format("Total value of packed items: {0}{1}", dReturn, Environment.NewLine));
            }
            return (int)dReturn;
        }

        public static int MaximizeValueInKnapsack_Q1()
        {
            //Should pack 40 items for a total value of 2493893
            //return DoKnapsackProblem(10000, Utilities.ReadFileIntoKnapsackItemArray(PathToFileQ1));
            return DoKnapsackProblem_Recursive(10000, Utilities.ReadFileIntoKnapsackItemArray(PathToFileQ1));
        }

        public static int MaximizeValueInKnapsack_Q2()
        {
            //This must be done recursively since the problem size is so large
            //Should pack 37 items for a total value of 2595819
            return DoKnapsackProblem_Recursive(2000000, Utilities.ReadFileIntoKnapsackItemArray(PathToFileQ2), true);
        }

        public static int MaximizeValueInKnapsack_Test0()
        {
            //Should pack these items for a total value of 309:
            //[43, 38][68, 44][49, 29][57, 31][92, 23]
            //return DoKnapsackProblem(165, Utilities.ReadFileIntoKnapsackItemArray(PathToTestFile0));
            return DoKnapsackProblem_Recursive(165, Utilities.ReadFileIntoKnapsackItemArray(PathToTestFile0));
        }

        public static int MaximizeValueInKnapsack_Test1()
        {
            return DoKnapsackProblem(750, Utilities.ReadFileIntoKnapsackItemArray(PathToTestFile1));
        }

        public static int MaximizeValueInKnapsack_Test2()
        {
            //return DoKnapsackProblem(50, Utilities.ReadFileIntoKnapsackItemArray(PathToTestFile2));
            return DoKnapsackProblem_Recursive(50, Utilities.ReadFileIntoKnapsackItemArray(PathToTestFile2));
        }
    }
}
