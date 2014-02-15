using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Algorithms
{
    public class Algorithms
    {
        //++ Resources ++
        //Book: "Algorithms" (2006) by Dasgupta/Papadimitriou/Vazirani - Free on web
        //Book: "Introductions to algorithms (3rd edition)" (2009) by Cormen/Leiserson/Rivest/Stein 
        //Book: "Data structures and algorithms: the basic toolbox" (2008) by Mehlhorn/Sanders - Free on web

        private static int[] test = new int[] { 1, 3, 5, 2, 4, 6 };

        public static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            //int testA = new Random((int)DateTime.Now.Ticks).Next(0, int.MaxValue);
            //Console.WriteLine(string.Format("There are {0} digits in {1}.", GetNumDigits(testA), testA));

            watch.Start();

            #region PQ3

            //DoTestFindMinCut_RandomizedContraction(100);
            //DoFindMinCut_RandomizedContraction(1000);
            //ProgrammingQuestions.ProgQuest3.TestFindMinCutWithRandomizedContractions(10);

            #endregion PQ3

            #region PQ4

            //System.Threading.Thread T = new System.Threading.Thread(DoFindSCCs_Kosaraju, 150000000);
            //T.Start();
            //ProgrammingQuestions.ProgQuest4.Test1();

            #endregion PQ4

            #region PQ5

            //Test_Do2SumProblem();
            //Do2SumProblem();

            #endregion PQ5

            #region Master Method

            //MasterMethod.PrintAssumptions();
            //MasterMethod.Evaluate();

            #endregion Master Method

            #region BinaryTree & Heap

            //DataStructures.BinayTree<int>.TestBinaryTree();
            //DataStructures.MinHeap<int>.TestMinHeap();
            //DataStructures.MaxHeap<int>.TestMaxHeap();
            //DataStructures.MinArrayHeap<int>.Test();
            //DataStructures.MaxArrayHeap<int>.Test();

            #endregion BinaryTree & Heap

            #region Algorithms course part II

            #region Algorithms 2 PQ1

            //Programming_Questions.Algorithms2_ProgQuest1.DoTestWeightedJobCompletionTime();
            //Programming_Questions.Algorithms2_ProgQuest1.DoCalculateWeightedJobCompletionTime();            
            //Programming_Questions.Algorithms2_ProgQuest1.DoTestDetermineMinSpanTree0_PrimsAlg();
            //Programming_Questions.Algorithms2_ProgQuest1.DoTestDetermineMinSpanTree1_PrimsAlg();
            //Programming_Questions.Algorithms2_ProgQuest1.DoTestDetermineMinSpanTree2_PrimsAlg();
            //Programming_Questions.Algorithms2_ProgQuest1.DoDetermineMinSpanTreeCost_PrimsAlg();

            #endregion Algorithms 2 PQ1

            #region Algorithms 2 PQ2

            //Programming_Questions.Algorithms2_ProgQuest2.DoTestDetermineMinSpanTree1_KruskalsAlg();
            //Programming_Questions.Algorithms2_ProgQuest2.DoTestDetermineMinSpanTree0_KruskalsAlg();
            //Programming_Questions.Algorithms2_ProgQuest2.DoDetermineMinSpanTreeCost_KruskalsAlg();
            //Programming_Questions.Algorithms2_ProgQuest2.DoTestMaximizeMinDistanceBetweenClusters0_KruskalsAlg();
            //Programming_Questions.Algorithms2_ProgQuest2.DoTestMaximizeMinDistanceBetweenClusters1_KruskalsAlg();
            //Programming_Questions.Algorithms2_ProgQuest2.DoTestMaximizeMinDistanceBetweenClusters2_KruskalsAlg();
            //Programming_Questions.Algorithms2_ProgQuest2.DoMaximizeMinDistanceBetweenClusters_KruskalsAlg();

            #endregion Algorithms 2 PQ2

            #region Algorithms 2 PQ3

            //Programming_Questions.Algorithms2_ProgQuest3.MaximizeValueInKnapsack_Test2();
            //Programming_Questions.Algorithms2_ProgQuest3.MaximizeValueInKnapsack_Test0();
            //Programming_Questions.Algorithms2_ProgQuest3.MaximizeValueInKnapsack_Q1();
            //Programming_Questions.Algorithms2_ProgQuest3.MaximizeValueInKnapsack_Q2();

            #endregion Algorithms 2 PQ3

            #region Algorithms 2 PQ4

            //Programming_Questions.Algorithms2_ProgQuest4.DoDijkstraFindShortestPaths_Test1();
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Test1(Programming_Questions.Algorithms2_ProgQuest4.Implementation.BellmanFord);
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Test1(Programming_Questions.Algorithms2_ProgQuest4.Implementation.FloydWarshall);
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Test1(Programming_Questions.Algorithms2_ProgQuest4.Implementation.Johnsons);
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Test2(Programming_Questions.Algorithms2_ProgQuest4.Implementation.BellmanFord);
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Test2(Programming_Questions.Algorithms2_ProgQuest4.Implementation.FloydWarshall);
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Test2(Programming_Questions.Algorithms2_ProgQuest4.Implementation.Johnsons);
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Graph1();
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Graph2();
            //Programming_Questions.Algorithms2_ProgQuest4.DoFindLowestCostMinPath_Graph3();
            
            #endregion Algorithms 2 PQ 4

            #region Algorithms 2 PQ5

            //Programming_Questions.Algorithms2_ProgQuest5.DoTravellingSalesmanProblem_Test1();
            //Programming_Questions.Algorithms2_ProgQuest5.DoTravellingSalesmanProblem_Graph1();

            #endregion Algorithms 2 PQ5

            #region Algorithms 2 PQ6

            //Programming_Questions.Algorithms2_ProgQuest6.FindSCCsWithKosarajus_Test0();
            //Programming_Questions.Algorithms2_ProgQuest6.FindSCCsWithKosarajus_Test1();
            //Programming_Questions.Algorithms2_ProgQuest6.DoDetermineSatisfiability_Test1();
            //Programming_Questions.Algorithms2_ProgQuest6.DoDetermineSatisfiability_Test2();
            Programming_Questions.Algorithms2_ProgQuest6.DoDetermineSatisfiability();

            #endregion Algorithms 2 PQ6

            #endregion Algorithms course part II

            watch.Stop();
            Console.WriteLine("Running time: " + watch.Elapsed.TotalSeconds + " seconds");
        }

        private static void DoMergeSort()
        {
            int[] array = new int[] { 5, 3, 8, 9, 1, 7, 0, 2, 6, 4 };
            MergeSort mSort = new MergeSort();
            mSort.Sort(array);
        }

        private static void DoCountInversions_BruteForce()
        {
            Stopwatch watch = new Stopwatch();

            //Brute force method
            watch.Start();
            uint numInversions = ProgrammingQuestions.ProgQuest1.CountInversions_BruteForce(Utilities.ReadFileIntoArray(ProgrammingQuestions.ProgQuest1.pathToArrayFile));
            watch.Stop();
            Console.WriteLine(string.Format("There were {0} inversions. Running time was {1}.", numInversions, watch.Elapsed));
            watch.Restart();
            numInversions = ProgrammingQuestions.ProgQuest1.CountInversions_BruteForce(test);
            watch.Stop();
            Console.WriteLine(string.Format("There were {0} inversions. Running time was {1}.", numInversions, watch.Elapsed));
            watch.Reset();
        }

        private static void DoCountInversions_MergeSort()
        {
            Stopwatch watch = new Stopwatch();

            //Merge sort method
            watch.Start();
            uint numInversions = ProgrammingQuestions.ProgQuest1.CountInversions_MergeSort(Utilities.ReadFileIntoArray(ProgrammingQuestions.ProgQuest1.pathToArrayFile));
            watch.Stop();
            Console.WriteLine(string.Format("There were {0} inversions. Running time was {1}.", numInversions, watch.Elapsed));
            watch.Restart();
            numInversions = ProgrammingQuestions.ProgQuest1.CountInversions_BruteForce(test);
            watch.Stop();
            Console.WriteLine(string.Format("There were {0} inversions. Running time was {1}.", numInversions, watch.Elapsed));
            watch.Reset();
        }

        private static void DoQuickSort()
        {
            QuickSort.Test(100, 0, 500);
        }

        private static void DoCountComparisons_QuickSort()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ulong numComparisons = ProgrammingQuestions.ProgQuest2.QuickSortAndCountComparisons(Utilities.ReadFileIntoArray(ProgrammingQuestions.ProgQuest2.pathToArrayFile));
            watch.Stop();
            Console.WriteLine("Running time was " + watch.Elapsed);
        }

        private static void DoFindMinCut_RandomizedContraction(int numberOfIterations)
        {
            int minCut = int.MaxValue;
            for (int i = 0; i < numberOfIterations; i++)
            {
                int temp = ProgrammingQuestions.ProgQuest3.FindMinCutWithRandomizedContractions(Utilities.ReadAdjacencyListIntoGraph(ProgrammingQuestions.ProgQuest3.pathToFile));
                Console.WriteLine(string.Format("Iteration {2} of {3}, Cut: {0}, Current min: {1}", temp/2, minCut/2, i, numberOfIterations));
                if (temp < minCut)
                {
                    minCut = temp;
                    Console.WriteLine("New min: " + minCut/2);
                }
            }
            Console.WriteLine("Best minimum cut: " + minCut/2);
            Console.WriteLine("==================================\n");
        }

        private static void DoTestFindMinCut_RandomizedContraction(int numberOfIterations)
        {
            int minCut = int.MaxValue;
            for (int i = 0; i < numberOfIterations; i++)
            {
                int temp = ProgrammingQuestions.ProgQuest3.FindMinCutWithRandomizedContractions(Utilities.ReadAdjacencyListIntoGraph(ProgrammingQuestions.ProgQuest3.pathToTestFile2));
                Console.WriteLine(string.Format("Iteration {2} of {3}, Cut: {0}, Current min: {1}", temp / 2, minCut / 2, i, numberOfIterations));
                if (temp < minCut)
                {
                    minCut = temp;
                    Console.WriteLine("New min: " + minCut / 2);
                }
            }
            Console.WriteLine("Best minimum cut: " + minCut / 2);
            Console.WriteLine("==================================\n");
        }

        private static void DoFindSCCs_Kosaraju()
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            ProgrammingQuestions.ProgQuest4.FindSCCs(Utilities.ReadEdgeListIntoGraph_Lite(ProgrammingQuestions.ProgQuest4.pathToFile));
            watch.Stop();
            Console.WriteLine("Running time: " + watch.Elapsed);
        }

        private static void Do2SumProblem()
        {
            const int numTargets = 9;
            Dictionary<int, KeyValuePair<bool, Point>> sumExistsDictionary = new Dictionary<int, KeyValuePair<bool, Point>>(numTargets);
            Dictionary<int, bool> summands = Utilities.ReadFileIntoDictionary(ProgQuest5.pathToFile);
            //int[] summands = Utilities.ReadFileIntoArray(ProgQuest5.pathToFile);

            for (int i = 0; i < numTargets; i++)
            {
                sumExistsDictionary[ProgQuest5.targetSums[i]] = ProgQuest5.OptimalTwoSumProblem(summands, ProgQuest5.targetSums[i]);
                //sumExistsDictionary[ProgQuest5.targetSums[i]] = ProgQuest5.SubOptimalTwoSumSolution(summands, ProgQuest5.targetSums[i]);
            }

            Console.WriteLine(string.Format("{0}Sums Exist:{0}{1}", Environment.NewLine, ProgQuest5.FormatPrintSumSolution(sumExistsDictionary)));
        }

        #region Helper Methods

        private static int GetNumDigits(int x)
        {
            double temp = x;
            int countDigits = 0;
            while (temp >= 1 || temp <= -1)
            {
                temp = temp * 0.1;
                countDigits++;
            }
            return countDigits;
        }

        #endregion Helper Methods
    }
}
