#undef DEBUG
using System;
using System.Collections.Generic;
using System.Text;
using Algorithms.Types;
using Algorithms.Dynamic_Programming;
using Algorithms.DataStructures;
using Algorithms.Graph_Search;

namespace Algorithms.Programming_Questions
{
    public class Algorithms2_ProgQuest4
    {
        private const string PathToFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ4_Q1_Graph1.txt";
        private const string PathToFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ4_Q1_Graph2.txt";
        private const string PathToFile3 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ4_Q1_Graph3.txt";

        private const string PathToTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ4_Q1_Test1.txt";
        private const string PathToTestFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ4_Q1_Test2.txt";
        private const string PathToDijkstraTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ4_Q1_Test3.txt";
        private const string PathToDijkstraTestFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ4_Q1_Test4.txt";

        public enum Implementation { BellmanFord, FloydWarshall, Johnsons };

        #region BellmanFord Implementation

        private static List<T> FindAllMinPathsToEachVertex<T, U>(IGraph<T, U> inputGraph, T sourceVertex)
            where T : PathVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            BellmanFordMinPath<T, U> bfMinpath = new BellmanFordMinPath<T, U>(inputGraph);
            return bfMinpath.FindMinPathFromSourceToAllVertices(sourceVertex);
        }

        private static double FindLowestCostMinPath<T, U>(IGraph<T, U> inputGraph, T sourceVertex)
            where T : PathVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            double shortestMinPathDistance = double.PositiveInfinity;
            List<T> vertices = Algorithms2_ProgQuest4.FindAllMinPathsToEachVertex<T, U>(inputGraph, sourceVertex);
            T endOfShortestPath = null;
            if (null == vertices)
            {
                Console.WriteLine(string.Format("There was a negative cycle detected in the input graph! Cannot calculate min paths."));
            }
            else
            {
                foreach (T v in vertices)
                {
                    if (v.Distance < shortestMinPathDistance)
                    {
#if DEBUG
                        Console.WriteLine(string.Format("Updating shortest min path value from {0} to {1}", shortestMinPathDistance, v.Distance));                        
#endif
                        shortestMinPathDistance = v.Distance;
                        endOfShortestPath = v;
                    }
                }
            }

            if (shortestMinPathDistance < double.PositiveInfinity)
            {
                Console.WriteLine(string.Format("Shortest min path from source vertex with index {0} has distance: {1}", sourceVertex.Index, shortestMinPathDistance));
                Console.WriteLine(Utilities.PrintBellmanFordPath<T, U>(endOfShortestPath));
            }
            return shortestMinPathDistance;
        }

        private static double FindLowestCostMinPath_BF<T, U>(IGraph<T, U> inputGraph)
            where T : PathVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            double lowestCostMinPathOfAll = double.PositiveInfinity;
            foreach (T v in inputGraph.Vertices)
            {
                double shortestMinPathFromSingleSource = Algorithms2_ProgQuest4.FindLowestCostMinPath<T, U>(inputGraph, v);
                if (double.PositiveInfinity == shortestMinPathFromSingleSource)
                {
                    Console.WriteLine(string.Format("Input graph has a negative weight cycle! Terminating search for shortest path."));
                    break;
                }

                if (shortestMinPathFromSingleSource < lowestCostMinPathOfAll)
                {
#if DEBUG
                    Console.WriteLine(string.Format("Updating lowest cost min path value for all sources from {0} to {1}", lowestCostMinPathOfAll, shortestMinPathFromSingleSource));
#endif
                    lowestCostMinPathOfAll = shortestMinPathFromSingleSource;
                }
            }

            if (lowestCostMinPathOfAll < double.PositiveInfinity)
            {
                Console.WriteLine(string.Format("Shortest min path from all sources has distance: {0}", lowestCostMinPathOfAll));
            }
            return lowestCostMinPathOfAll;
        }

        #endregion BellmanFord Implementation

        #region FloydWarshall Implementation

        private static T[,] FindAllMinPathsInGraph<T, U>(IGraph<T, U> inputGraph)
            where T : PathVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            FloydWarshallMinPaths<T, U> fwMinPaths = new FloydWarshallMinPaths<T, U>(inputGraph);
            return fwMinPaths.FindMinPathFromAllSourcesToAllVertices();
        }

        private static double FindLowestCostMinPath_FW<T, U>(IGraph<T, U> inputGraph)
            where T : PathVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            double shortestMinPathDistance = double.PositiveInfinity;
            T endOfShortestPath = null;
            T startOfShortestPath = null;
            T[,] minPaths = Algorithms2_ProgQuest4.FindAllMinPathsInGraph<T, U>(inputGraph);

            if (null == minPaths)
            {
                Console.WriteLine(string.Format("There was a negative cycle detected in the input graph! Cannot calculate min paths."));
            }
            else
            {
                int numVertices = inputGraph.Vertices.Count;
                for (int i = 0; i < numVertices; i++)
                {
                    for (int j = 0; j < numVertices; j++)
                    {
                        double minDistFromItoJ = minPaths[i, j].Distance;
                        if (minDistFromItoJ < shortestMinPathDistance)
                        {
#if DEBUG
                        Console.WriteLine(string.Format("Updating shortest min path value from {0} to {1}", shortestMinPathDistance, v.Distance));                        
#endif
                            shortestMinPathDistance = minDistFromItoJ;
                            endOfShortestPath = minPaths[i, j];
                            startOfShortestPath = inputGraph.GetVertexAt(i);
                        }
                    }
                }
            }

            if (shortestMinPathDistance < double.PositiveInfinity)
            {
                Console.WriteLine(string.Format("Shortest min path in graph is path from index {0} to index {1} and has distance: {2}", startOfShortestPath.Index, endOfShortestPath.Index, shortestMinPathDistance));
                Console.WriteLine(Utilities.PrintBellmanFordPath<T, U>(endOfShortestPath));
            }

            return shortestMinPathDistance;
        }

        #endregion FloydWarshall Implementation

        #region Johnsons Implementation

        private static double FindLowestCostMinPath_J<T, U>(IGraph<T, U> inputGraph)
            where T : PathVertex<U>, IEquatable<T>
            where U : struct, IComparable, IEquatable<U>
        {
            double shortestMinPathDistance = double.PositiveInfinity;
            T endOfShortestPath = null;
            T startOfShortestPath = null;

            JohnsonsMinPaths<T, U> johnson = new JohnsonsMinPaths<T, U>(inputGraph);
            T[,] minPaths = johnson.FindMinPathFromAllSourcesToAllVertices();

            if (null == minPaths)
            {
                Console.WriteLine(string.Format("There was a negative cycle detected in the input graph! Cannot calculate min paths."));
                return shortestMinPathDistance;
            }
            else
            {
                int numVertices = inputGraph.Vertices.Count;
                for (int i = 0; i < numVertices; i++)
                {
                    for (int j = 0; j < numVertices; j++)
                    {
                        double minDistFromItoJ = minPaths[i, j].Distance;
                        if (minDistFromItoJ < shortestMinPathDistance)
                        {
#if DEBUG
                        Console.WriteLine(string.Format("Updating shortest min path value from {0} to {1}", shortestMinPathDistance, v.Distance));                        
#endif
                            shortestMinPathDistance = minDistFromItoJ;
                            endOfShortestPath = minPaths[i, j];
                            startOfShortestPath = inputGraph.GetVertexAt(i);
                        }
                    }
                }
            }

            if (shortestMinPathDistance < double.PositiveInfinity)
            {
                Console.WriteLine(string.Format("Shortest min path in graph is path from index {0} to index {1} and has distance: {2}", startOfShortestPath.Index, endOfShortestPath.Index, shortestMinPathDistance));
                Console.WriteLine(Utilities.PrintBellmanFordPath<T, U>(endOfShortestPath));
            }

            return shortestMinPathDistance;
        }

        #endregion Johnsons Implementation

        private static double DoFindLowestCostMinPath<T, U>(IGraph<T, U> inputGraph, Implementation whichImplementation)
            where T : PathVertex<U>, IEquatable<T>
            where U : struct, IComparable, IEquatable<U>
        {
            double rDouble = double.PositiveInfinity;
            switch (whichImplementation)
            {
                case Implementation.BellmanFord:
                    rDouble = Algorithms2_ProgQuest4.FindLowestCostMinPath_BF<T, U>(inputGraph);
                    break;
                case Implementation.FloydWarshall:
                    rDouble = Algorithms2_ProgQuest4.FindLowestCostMinPath_FW<T, U>(inputGraph);
                    break;
                default:
                    rDouble = Algorithms2_ProgQuest4.FindLowestCostMinPath_J<T, U>(inputGraph);
                    break;
            }

            return rDouble;
        }

        public static void DoFindLowestCostMinPath_Graph1(Implementation whichImplementation = Implementation.Johnsons)
        {
            Algorithms2_ProgQuest4.DoFindLowestCostMinPath<PathVertex<double>, double>(Utilities.ReadEdgeListIntoGraph_BellmanFord(PathToFile1, 1000), whichImplementation);
        }

        public static void DoFindLowestCostMinPath_Graph2(Implementation whichImplementation = Implementation.Johnsons)
        {
            Algorithms2_ProgQuest4.DoFindLowestCostMinPath<PathVertex<double>, double>(Utilities.ReadEdgeListIntoGraph_BellmanFord(PathToFile2, 1000), whichImplementation);
        }

        public static void DoFindLowestCostMinPath_Graph3(Implementation whichImplementation = Implementation.Johnsons)
        {
            Algorithms2_ProgQuest4.DoFindLowestCostMinPath<PathVertex<double>, double>(Utilities.ReadEdgeListIntoGraph_BellmanFord(PathToFile3, 1000), whichImplementation);
        }

        public static void DoFindLowestCostMinPath_Test1(Implementation whichImplementation = Implementation.FloydWarshall)
        {
            Algorithms2_ProgQuest4.DoFindLowestCostMinPath<PathVertex<double>, double>(Utilities.ReadEdgeListIntoGraph_BellmanFord(PathToTestFile1, 20), whichImplementation);
        }

        public static void DoFindLowestCostMinPath_Test2(Implementation whichImplementation = Implementation.FloydWarshall)
        {
            Algorithms2_ProgQuest4.DoFindLowestCostMinPath<PathVertex<double>, double>(Utilities.ReadEdgeListIntoGraph_BellmanFord(PathToTestFile2, 6), whichImplementation);
        }

        public static void DoDijkstraFindShortestPaths_Test1()
        {
            Console.WriteLine("Dijkstra\n");
            PathVertex<double> source = new PathVertex<double>(1);
            IGraph<PathVertex<double>, double> graph = Utilities.ReadEdgeListIntoGraph_BellmanFord(PathToDijkstraTestFile1, 6);
            DijkstrasMinPath<PathVertex<double>, double> dijkstra = new DijkstrasMinPath<PathVertex<double>, double>(graph);
            List<PathVertex<double>> minPaths = dijkstra.FindMinPathFromSourceToAllVertices(source);
            foreach(PathVertex<double> pv in minPaths)
            {
                Console.WriteLine(string.Format("Shortest path from {0} to {1} has distance {2}{3}{4}",
                    source.Index, pv.Index, pv.Distance, Environment.NewLine, Utilities.PrintBellmanFordPath<PathVertex<double>, double>(pv)));
            }

            Console.WriteLine("\nBellman-Ford\n");
            graph = Utilities.ReadEdgeListIntoGraph_BellmanFord(PathToDijkstraTestFile1, 6); //Reinitialize graph
            BellmanFordMinPath<PathVertex<double>, double> bellmanFord = new BellmanFordMinPath<PathVertex<double>, double>(graph);
            minPaths = bellmanFord.FindMinPathFromSourceToAllVertices(source);
            foreach (PathVertex<double> pv in minPaths)
            {
                Console.WriteLine(string.Format("Shortest path from {0} to {1} has distance {2}{3}{4}",
                    source.Index, pv.Index, pv.Distance, Environment.NewLine, Utilities.PrintBellmanFordPath<PathVertex<double>, double>(pv)));
            }
        }
    }
}
