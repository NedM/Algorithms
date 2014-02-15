using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.ProgrammingQuestions
{
    public class ProgQuest3
    {
        public const string pathToFile = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ3_Adjacency.txt";

        #region Tests

        public const string pathToTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ3_TC1.txt";
        public const string pathToTestFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ3_TC2_minCut2.txt";
        public const string pathToTestFile3 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\PQ3_TC3_minCut2.txt";

        public static DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int> CreateTestGraph0()
        {
            DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int> graph = new DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int>(3);
            Types.Vertex<int> vertex0 = new Types.Vertex<int>(1, 0);
            Types.Vertex<int> vertex1 = new Types.Vertex<int>(2, 0);
            Types.Vertex<int> vertex2 = new Types.Vertex<int>(3, 0);
            
            graph.AddVertex(vertex0);
            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);
            
            graph.AddUndirectedEdge(vertex0, vertex1);
            graph.AddUndirectedEdge(vertex1, vertex2);
            graph.AddUndirectedEdge(vertex0, vertex2);
            
            Console.WriteLine("Original graph:\n" + graph.ToString());
            return graph;
        }

        public static DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int> CreateTestGraph1()
        {
            DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int> graph = new DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int>(4);
            Types.Vertex<int> vertex0 = new Types.Vertex<int>(0, 0);
            Types.Vertex<int> vertex1 = new Types.Vertex<int>(1, 0);
            Types.Vertex<int> vertex2 = new Types.Vertex<int>(2, 0);
            Types.Vertex<int> vertex3 = new Types.Vertex<int>(3, 0);
            Types.Vertex<int> vertex4 = new Types.Vertex<int>(4, 0);
            Types.Vertex<int> vertex5 = new Types.Vertex<int>(5, 0);
            Types.Vertex<int> vertex6 = new Types.Vertex<int>(6, 0);
            Types.Vertex<int> vertex7 = new Types.Vertex<int>(7, 0);

            graph.AddVertex(vertex0);
            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);
            graph.AddVertex(vertex3);
            graph.AddVertex(vertex4);
            graph.AddVertex(vertex5);
            graph.AddVertex(vertex6);
            graph.AddVertex(vertex7);

            graph.AddUndirectedEdge(vertex0, vertex1);
            graph.AddUndirectedEdge(vertex1, vertex2);
            graph.AddUndirectedEdge(vertex2, vertex3);
            graph.AddUndirectedEdge(vertex0, vertex3);
            graph.AddUndirectedEdge(vertex0, vertex2);
            graph.AddUndirectedEdge(vertex1, vertex3);
            graph.AddUndirectedEdge(vertex1, vertex4);
            //graph.AddUndirectedEdge(vertex1, vertex7);
            graph.AddUndirectedEdge(vertex2, vertex7);
            graph.AddUndirectedEdge(vertex4, vertex5);
            graph.AddUndirectedEdge(vertex5, vertex6);
            graph.AddUndirectedEdge(vertex6, vertex7);
            graph.AddUndirectedEdge(vertex4, vertex7);
            graph.AddUndirectedEdge(vertex4, vertex6);
            graph.AddUndirectedEdge(vertex5, vertex7);

            Console.WriteLine("Original graph:\n" + graph.ToString());
            return graph;
        }

        public static DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int> CreateTestGraph2()
        {
            DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int> graph = new DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int>(4);
            Types.Vertex<int> vertex0 = new Types.Vertex<int>(0, 0);
            Types.Vertex<int> vertex1 = new Types.Vertex<int>(1, 0);
            Types.Vertex<int> vertex2 = new Types.Vertex<int>(2, 0);

            graph.AddVertex(vertex0);
            graph.AddVertex(vertex1);
            graph.AddVertex(vertex2);

            graph.AddUndirectedEdge(vertex0, vertex1);
            graph.AddUndirectedEdge(vertex1, vertex2);            

            Console.WriteLine("Original graph:\n" + graph.ToString());
            return graph;
        }

        public static int TestFindMinCutWithRandomizedContractions(int numIterations)
        {
            int minCut = int.MaxValue;
            for (int i = 0; i < numIterations; i++)
            {
                int temp = ProgrammingQuestions.ProgQuest3.FindMinCutWithRandomizedContractions(Utilities.ReadAdjacencyListIntoGraph(pathToTestFile3));
                //int temp = ProgrammingQuestions.ProgQuest3.FindMinCutWithRandomizedContractions(CreateTestGraph0());

                Console.WriteLine(string.Format("Cut: {0}, Current min: {1}", temp / 2, minCut / 2));
                if (temp < minCut)
                {
                    minCut = temp;
                    Console.WriteLine("New min: " + minCut / 2);
                }
            }
            Console.WriteLine("Best minimum cut: " + minCut / 2);
            Console.WriteLine("==================================\n");
            return minCut;
        }

        #endregion Tests

        public static int FindMinCutWithRandomizedContractions(DataStructures.Graph_AdjacencyList<Types.Vertex<int>, int> graph)
        {
            RandomizedContraction contraction = new RandomizedContraction(graph);
            int cut = contraction.FindMinCut_RandomizeContraction();
            return cut;
        }
    }
}
