using System;
using System.Collections.Generic;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;
using Algorithms.Graph_Search;

namespace Algorithms.Programming_Questions
{
    public class Algorithms2_ProgQuest6
    {
        private static Dictionary<string, int> pathsToFiles = new Dictionary<string, int>() {
            {@"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_2SAT1.txt", 100000},
            {@"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_2SAT2.txt", 200000},
            {@"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_2SAT3.txt", 400000},
            {@"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_2SAT4.txt", 600000},
            {@"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_2SAT5.txt", 800000},
            {@"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_2SAT6.txt", 1000000},
        };

        private static string pathToTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_Test1.txt";
        private static string pathToTestFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ6_Q1_Test2.txt";

        private static bool TwoSATCanBeSatisfied<T, U>(IGraph<T, U> graph)
            where T : LogicVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            bool canBeSatisfied = true;

            StronglyConnectedComponents<T, U> alg = new StronglyConnectedComponents<T, U>(graph);

            Dictionary<int, List<T>> sccs = alg.FindSCCs_KosarajusAlg();

#if DEBUG
            Console.WriteLine(string.Format("Found {0} strongly connected components in the graph", sccs.Keys.Count));
#endif
            foreach (int key in sccs.Keys)
            {
                List<T> scc = sccs[key];
                int sccSize = scc.Count;

#if DEBUG
                if (sccSize > 1)
                {
                    Console.WriteLine(string.Format("Component with leader {0} has {1} connected components", key, sccSize));
                }
#endif

                if (sccSize > 1 && LogicVertex<U>.CollectionContainsContradiction(new List<LogicVertex<U>>(scc)))
                {
#if DEBUG
                    Console.WriteLine(string.Format("Contradiction detected in SCC with leader = {0}! This set of logical clauses cannot be satisfied!", key));
                    Console.WriteLine(string.Format("Component members:{1}{0}", Utilities.FormatPrintList<T>(sccs[key]), Environment.NewLine));
#endif

                    canBeSatisfied = false;
                    break;
                }
            }

            return canBeSatisfied;
        }

        public static void DoDetermineSatisfiability()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string path in pathsToFiles.Keys)
            {
                bool satisfiable = Algorithms2_ProgQuest6.TwoSATCanBeSatisfied<LogicVertex<int>, int>(Utilities.Read2SATDataIntoImplicationGraph(path, pathsToFiles[path]));
                Console.WriteLine(string.Format("2SAT problem described by {0} can{1} be satisfied", path, satisfiable ? string.Empty : " NOT"));
                sb.Append(satisfiable ? "1" : "0");
            }
            Console.WriteLine(string.Format("Result: {0}", sb.ToString()));
        }

        public static void DoDetermineSatisfiability_Test1()
        {
            Graph_AdjacencyList<LogicVertex<int>, int> inputGraph = Utilities.Read2SATDataIntoImplicationGraph(pathToTestFile1, 8);
            bool canBeSatisfied = Algorithms2_ProgQuest6.TwoSATCanBeSatisfied<LogicVertex<int>, int>(inputGraph);
            Console.WriteLine(string.Format("2SAT problem described by {0} can{1} be satisfied", pathToTestFile1, canBeSatisfied ? string.Empty : " NOT"));
        }

        public static void DoDetermineSatisfiability_Test2()
        {
            Graph_AdjacencyList<LogicVertex<int>, int> inputGraph = Utilities.Read2SATDataIntoImplicationGraph(pathToTestFile2, 14);
            bool canBeSatisfied = Algorithms2_ProgQuest6.TwoSATCanBeSatisfied<LogicVertex<int>, int>(inputGraph);
            Console.WriteLine(string.Format("2SAT problem described by {0} can{1} be satisfied", pathToTestFile1, canBeSatisfied ? string.Empty : " NOT"));
        }

        public static void FindSCCsWithKosarajus_Test0()
        {
            Algorithms2_ProgQuest6.Test0();

            Graph_AdjacencyList<Vertex<int>, int> testGraph = Utilities.ReadEdgeListIntoGraph(pathToFile0);
            Console.WriteLine(string.Format("Original Graph:{0}{1}", Environment.NewLine, testGraph.ToString()));
            Graph_AdjacencyList<Vertex<int>, int> revGraph = Graph_AdjacencyList<Vertex<int>, int>.ReverseGraph(testGraph);
            Console.WriteLine(string.Format("Reversed Graph:{0}{1}", Environment.NewLine, revGraph.ToString()));
            Graph_AdjacencyList<Vertex<int>, int> reRevGraph = Graph_AdjacencyList<Vertex<int>, int>.ReverseGraph(revGraph);
            Console.WriteLine(string.Format("Re-reversed Graph:{0}{1}", Environment.NewLine, reRevGraph.ToString()));

            StronglyConnectedComponents<Vertex<int>, int> alg = new StronglyConnectedComponents<Vertex<int>, int>(testGraph);
            Dictionary<int, List<Vertex<int>>> sccs = alg.FindSCCs_KosarajusAlg();
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}.", sccs.Keys.Count, Utilities.FormatPrintCollection<int>(sccs.Keys)));
            foreach (int leader in sccs.Keys)
            {
                Console.WriteLine(string.Format("Components for leader {0}:{2}{1}", leader, Utilities.FormatPrintList<Vertex<int>>(sccs[leader]), Environment.NewLine));
            }
        }

        public static void FindSCCsWithKosarajus_Test1()
        {
            Algorithms2_ProgQuest6.Test1();

            Graph_AdjacencyList<Vertex<int>, int> testGraph = Utilities.ReadEdgeListIntoGraph(pathToFile1);
            Console.WriteLine(string.Format("Original Graph:{0}{1}", Environment.NewLine, testGraph.ToString()));
            Graph_AdjacencyList<Vertex<int>, int> revGraph = Graph_AdjacencyList<Vertex<int>, int>.ReverseGraph(testGraph);
            Console.WriteLine(string.Format("Reversed Graph:{0}{1}", Environment.NewLine, revGraph.ToString()));
            Graph_AdjacencyList<Vertex<int>, int> reRevGraph = Graph_AdjacencyList<Vertex<int>, int>.ReverseGraph(revGraph);
            Console.WriteLine(string.Format("Re-reversed Graph:{0}{1}", Environment.NewLine, reRevGraph.ToString()));

            StronglyConnectedComponents<Vertex<int>, int> alg = new StronglyConnectedComponents<Vertex<int>, int>(testGraph);
            Dictionary<int, List<Vertex<int>>> sccs = alg.FindSCCs_KosarajusAlg();
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}.", sccs.Keys.Count, Utilities.FormatPrintCollection<int>(sccs.Keys)));
            foreach (int leader in sccs.Keys)
            {
                Console.WriteLine(string.Format("Components for leader {0}:{2}{1}", leader, Utilities.FormatPrintList<Vertex<int>>(sccs[leader]), Environment.NewLine));
            }
        }

        private const string pathToFile0 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ4_TC0.txt";
        private const string pathToFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ4_TC1.txt";

        private static void Test0()
        {
            Graph_AdjacencyList_Lite myGraph = Utilities.ReadEdgeListIntoGraph_Lite(pathToFile0);
            Console.WriteLine(string.Format("Original Graph:{0}{1}", Environment.NewLine, myGraph.ToString()));
            Graph_AdjacencyList_Lite revGraph = Graph_AdjacencyList_Lite.ReverseGraph(myGraph);
            Console.WriteLine(string.Format("Reversed Graph:{0}{1}", Environment.NewLine, revGraph.ToString()));
            Graph_AdjacencyList_Lite reRevGraph = Graph_AdjacencyList_Lite.ReverseGraph(revGraph);
            Console.WriteLine(string.Format("Re-reversed Graph:{0}{1}", Environment.NewLine, reRevGraph.ToString()));

            StronglyConnectedComponents_Kosarajus kDFS = new StronglyConnectedComponents_Kosarajus();
            Dictionary<int, int> result = kDFS.FindSCCsInGraph(myGraph);
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}", result.Count, Utilities.FormatPrintDictionary<int, int>(result)));
        }

        private static void Test1()
        {
            Graph_AdjacencyList_Lite myGraph = Utilities.ReadEdgeListIntoGraph_Lite(pathToFile1);
            Console.WriteLine(string.Format("Original Graph:{0}{1}", Environment.NewLine, myGraph.ToString()));
            Graph_AdjacencyList_Lite revGraph = Graph_AdjacencyList_Lite.ReverseGraph(myGraph);
            Console.WriteLine(string.Format("Reversed Graph:{0}{1}", Environment.NewLine, revGraph.ToString()));
            Graph_AdjacencyList_Lite reRevGraph = Graph_AdjacencyList_Lite.ReverseGraph(revGraph);
            Console.WriteLine(string.Format("Re-reversed Graph:{0}{1}", Environment.NewLine, reRevGraph.ToString()));

            StronglyConnectedComponents_Kosarajus kDFS = new StronglyConnectedComponents_Kosarajus();
            Dictionary<int, int> result = kDFS.FindSCCsInGraph(myGraph);
            
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}",
                result.Count, Utilities.FormatPrintDictionary<int, int>(result)));
        }

        private static void FindSCCs(Graph_AdjacencyList_Lite graph)
        {
            StronglyConnectedComponents_Kosarajus kDFS = new StronglyConnectedComponents_Kosarajus();
            Dictionary<int, int> result = kDFS.FindSCCsInGraph(graph);
            Console.WriteLine(string.Format("There were {0} SCCs in the graph. The leaders were {1}.", 
                result.Count, Utilities.FormatPrintDictionary<int, int>(result)));
        }
    }
}
