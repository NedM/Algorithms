#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;
using Algorithms.DataStructures;

namespace Algorithms.Programming_Questions
{
    public class Algorithms2_ProgQuest1
    {
        public const string pathToArrayFile = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ1_Q1_JobList.txt";
        public const string pathToArrayTestFile = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ1_Q1_Test.txt";

        public const string pathToGraphFile = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ1_Q3_Graph.txt";
        public const string pathToGraphTestFile0 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\MinSpanTree_Test.txt";
        public const string pathToGraphTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ1_Q3_Test.txt";
        public const string pathToGraphTestFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ1_Q3_Test2.txt";

        public static ulong CalculateTotalWeightedCompletionTime(MaxArrayHeap<WeightedJob> heap)
        {
            ulong sum = 0;
            ulong lastCompletionTime = 0;

            if (!heap.VerifyHeap())
            {
                heap.FixHeap();
                Console.WriteLine("Fixed heap:\n" + heap.ToString());
            }


            while (heap.Size > 0)
            {
                WeightedJob wj = heap.Pop();
                ulong completionTimeForJob = lastCompletionTime + (ulong)wj.Length;
                lastCompletionTime = completionTimeForJob;
                ulong weightedCompletion = (ulong)(completionTimeForJob * (ulong)wj.Weight);
                sum += weightedCompletion;
                if (!heap.VerifyHeap())
                {
                    heap.FixHeap();
#if DEBUG
                Console.WriteLine(heap.ToString());
#endif
                }

                if (heap.Size % 500 == 0)
                {
                    Console.WriteLine(string.Format("Job: {0} Completion time: {1} Weighted: {2} Sum: {3}", wj.ToString(), completionTimeForJob, weightedCompletion, sum)); 
                }
            }

            return sum;
        }

        public static void DoCalculateWeightedJobCompletionTime()
        {
            ulong sum = Programming_Questions.Algorithms2_ProgQuest1.CalculateTotalWeightedCompletionTime(Utilities.ReadJobListIntoHeap(Programming_Questions.Algorithms2_ProgQuest1.pathToArrayFile));

            Console.WriteLine(string.Format("{0}Total weighted completion time as long: {1} as uint {2}", Environment.NewLine, sum, (uint)sum));
        }

        public static void DoTestWeightedJobCompletionTime()
        {
            ulong sum = Programming_Questions.Algorithms2_ProgQuest1.CalculateTotalWeightedCompletionTime(Utilities.ReadJobListIntoHeap(Programming_Questions.Algorithms2_ProgQuest1.pathToArrayTestFile));

            Console.WriteLine(string.Format("{0}Total weighted completion time: {1}", Environment.NewLine, sum));
        }

        public static Graph_AdjacencyList DetermineMinimumSpanningTree_PrimmsAlg(Graph_AdjacencyList inputGraph)
        {
            return new Graph_Search.PrimsMinSpanTree(inputGraph).FindMinimumSpaningTree();
        }

        private static void DoDetermineMinSpanTreeCost_PrimsAlg(string pathToInputFile, int numNodes = 500)
        {
            Graph_AdjacencyList minSpan = DetermineMinimumSpanningTree_PrimmsAlg(Utilities.ReadAdjacencyListIntoGraphOfInts(pathToInputFile, numNodes));
            double totalCost = 0;
            foreach (Edge<int> e in minSpan.Edges)
            {
                totalCost += e.Weight;
            }

            Console.WriteLine(string.Format("Minimum Spanning Tree:{0}{1}{0}Cost: {2}", Environment.NewLine, minSpan.ToString(), totalCost / 2));
        }

        public static void DoDetermineMinSpanTreeCost_PrimsAlg()
        {
            DoDetermineMinSpanTreeCost_PrimsAlg(pathToGraphFile, 500);
        }

        public static void DoTestDetermineMinSpanTree0_PrimsAlg()
        {
            DoDetermineMinSpanTreeCost_PrimsAlg(pathToGraphTestFile0, 7);
        }

        public static void DoTestDetermineMinSpanTree1_PrimsAlg()
        {
            DoDetermineMinSpanTreeCost_PrimsAlg(pathToGraphTestFile1, 5);            
        }

        public static void DoTestDetermineMinSpanTree2_PrimsAlg()
        {
            DoDetermineMinSpanTreeCost_PrimsAlg(pathToGraphTestFile2, 7);            
        }

    }
}
