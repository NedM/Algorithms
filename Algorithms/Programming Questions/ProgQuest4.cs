using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Graph_Search;

namespace Algorithms.ProgrammingQuestions
{
    public class ProgQuest4
    {
        public const string pathToFile = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ4_Adjacency.txt";
        private const string pathToFile0 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ4_TC0.txt";
        private const string pathToFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ4_TC1.txt";

        public static void Test0()
        {
            Graph_AdjacencyList_Lite myGraph = Utilities.ReadEdgeListIntoGraph_Lite(pathToFile0);
            Graph_AdjacencyList_Lite revGraph = Graph_AdjacencyList_Lite.ReverseGraph(myGraph);
            Console.WriteLine(string.Format("Reversed Graph:{0}{1}", Environment.NewLine, revGraph.ToString()));
            Graph_AdjacencyList_Lite reRevGraph = Graph_AdjacencyList_Lite.ReverseGraph(revGraph);
            Console.WriteLine(string.Format("Re-reversed Graph:{0}{1}", Environment.NewLine, reRevGraph.ToString()));

            StronglyConnectedComponents_Kosarajus kDFS = new StronglyConnectedComponents_Kosarajus();
            Dictionary<int, int> result = kDFS.FindSCCsInGraph(myGraph);
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}", result.Count, Utilities.FormatPrintDictionary<int, int>(result)));
        }

        public static void Test1()
        {
            Graph_AdjacencyList_Lite myGraph = Utilities.ReadEdgeListIntoGraph_Lite(pathToFile1);
            Console.WriteLine(string.Format("Original Graph:{0}{1}", Environment.NewLine, myGraph.ToString()));
            Graph_AdjacencyList_Lite revGraph = Graph_AdjacencyList_Lite.ReverseGraph(myGraph);
            Console.WriteLine(string.Format("Reversed Graph:{0}{1}", Environment.NewLine, revGraph.ToString()));
            Graph_AdjacencyList_Lite reRevGraph = Graph_AdjacencyList_Lite.ReverseGraph(revGraph);
            Console.WriteLine(string.Format("Re-reversed Graph:{0}{1}", Environment.NewLine, reRevGraph.ToString()));

            StronglyConnectedComponents_Kosarajus kDFS = new StronglyConnectedComponents_Kosarajus();
            Dictionary<int, int> result = kDFS.FindSCCsInGraph(myGraph);
            List<int> fiveTop = FindMaxValuesInDictionary(result, 5);
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}{3}5 largest SCCs were {2}",
                result.Count, Utilities.FormatPrintDictionary<int, int>(result), Utilities.FormatPrintList<int>(fiveTop), Environment.NewLine));
        }

        public static void FindSCCs(Graph_AdjacencyList_Lite graph)
        {
            StronglyConnectedComponents_Kosarajus kDFS = new StronglyConnectedComponents_Kosarajus();
            Dictionary<int, int> result = kDFS.FindSCCsInGraph(graph);
            List<int> fiveTop = FindMaxValuesInDictionary(result, 5);
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}, 5 largest SCCs were {2}", 
                result.Count, Utilities.FormatPrintDictionary<int, int>(result), Utilities.FormatPrintList<int>(fiveTop)));
        }

        private static List<int> FindMaxValuesInDictionary(Dictionary<int, int> input, int numValuesToFind)
        {
            List<int> maxValues = new List<int>(numValuesToFind);
            bool valueAdded = false; ;

            foreach (int key in input.Keys)
            {
                valueAdded = false;
                if (maxValues.Count < numValuesToFind)
                {
                    valueAdded = true;
                    maxValues.Add(input[key]);
                }
                else
                {
                    if (input[key] > maxValues[0])
                    {
                        valueAdded = true;
                        maxValues[0] = input[key];
                    }
                }

                if (valueAdded)
                {
                    maxValues.Sort();
                }
            }

            return maxValues;
        }
    }
}
