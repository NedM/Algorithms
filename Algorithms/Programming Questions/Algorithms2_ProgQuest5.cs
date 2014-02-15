using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;
using Algorithms.Dynamic_Programming;

namespace Algorithms.Programming_Questions
{
    public class Algorithms2_ProgQuest5
    {
        private const string PathToFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ5_Q1_TSP1.txt";

        private const string PathToTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ5_Q1_Test1.txt";

        private static double CalculateShortesTSPRoute<T, U>(IGraph<T, U> inputGraph, ref T source, bool printRoute = false)
            where T : PathVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            TravellingSalesmanProblem<T, U> tsp = new TravellingSalesmanProblem<T, U>(inputGraph);
            double shortestRouteDistance = tsp.FindShortestRoute(ref source);

            Console.WriteLine(string.Format("Shortest route distance = {0}.", shortestRouteDistance));
            if(printRoute)
            {
                Console.WriteLine(string.Format("Route:{1}{0}", Utilities.PrintBellmanFordPath<T, U>(source), Environment.NewLine));
            }
            return shortestRouteDistance;            
        }

        public static void DoTravellingSalesmanProblem_Graph1()
        {
            Graph_AdjacencyList<PathVertex<Point>, Point> graph = Utilities.ReadPointListIntoGraph(PathToFile1, 25);
            PathVertex<Point> source = graph.Vertices[0];
            Algorithms2_ProgQuest5.CalculateShortesTSPRoute<PathVertex<Point>, Point>(graph, ref source);
        }

        public static void DoTravellingSalesmanProblem_Test1()
        {
            Graph_AdjacencyList<PathVertex<Point>, Point> graph = Utilities.ReadPointListIntoGraph(PathToTestFile1, 4);
            PathVertex<Point> source = graph.Vertices[0];
            Algorithms2_ProgQuest5.CalculateShortesTSPRoute<PathVertex<Point>, Point>(graph, ref source);
        }
    }
}
