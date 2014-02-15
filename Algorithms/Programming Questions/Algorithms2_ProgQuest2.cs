using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;
using System.IO;

namespace Algorithms.Programming_Questions
{
    public class Algorithms2_ProgQuest2
    {
        public const string PathToFileQ1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ2_Q1_Graph.txt";
        public const string PathToFileQ2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ2_Q2_Graph.txt";

        public const string PathToGraph = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ1_Q3_Graph.txt";

        public const string PathToTestFile = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\MinSpanTree_Test.txt";
        public const string PathToTestFile1 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ2_Q1_Test.txt";
        public const string PathToTestFile2 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ2_Q1_Test2.txt";
        public const string PathToTestFile3 = @"E:\Dropbox\Programming\dotNET\C#\Code\Algorithms\Alg2_PQ2_Q1_Test3.txt";

        private static Graph_AdjacencyList<Vertex_UnionFind<int>, int> DetermineMinimumSpanningTree_KruskalsAlg(Graph_AdjacencyList<Vertex_UnionFind<int>, int> inputGraph)
        {
            return new Graph_Search.KruskalsAlgorithm<Vertex_UnionFind<int>, int>(inputGraph).FindMinimumSpaningTree();
        }

        private static Edge<int> DetermineMaximumMinSpacingBetweenClusters_KruskalsAlg(Graph_AdjacencyList<Vertex_UnionFind<int>, int> inputGraph, int numClusters)
        {
            return new Graph_Search.KruskalsAlgorithm<Vertex_UnionFind<int>, int>(inputGraph).FindMaxSpacingForKClusters(numClusters);
        }

        private static void DoDetermineMinSpanTreeCost_Kruskals(string pathToInputFile, int numNodes = 500)
        {
            Graph_AdjacencyList<Vertex_UnionFind<int>, int> minSpan = DetermineMinimumSpanningTree_KruskalsAlg(Utilities.ReadAdjacencyListIntoUnionFindGraph(pathToInputFile, numNodes));
            double totalCost = 0;
            foreach (Edge<int> e in minSpan.Edges)
            {
                totalCost += e.Weight;
            }

            Console.WriteLine(string.Format("Minimum Spanning Tree:{0}{1}{0}Cost: {2}", Environment.NewLine, minSpan.ToString(), totalCost / 2));
        }

        private static void DoDetermineMaximumMinSpacingBetweenClusters(string pathToInputFile, int numClusters, int numNodes = 500)
        {
            Edge<int> nextLowestCostEdge = DetermineMaximumMinSpacingBetweenClusters_KruskalsAlg(Utilities.ReadAdjacencyListIntoUnionFindGraph(pathToInputFile, numNodes), numClusters);

            Console.WriteLine(string.Format("Next lowest cost edge has weight {0}. Edge: {1}", nextLowestCostEdge.Weight, nextLowestCostEdge.ToString()));
        }

        public static void DoDetermineMinSpanTreeCost_KruskalsAlg()
        {
            DoDetermineMinSpanTreeCost_Kruskals(PathToGraph, 500);
        }

        public static void DoTestDetermineMinSpanTree0_KruskalsAlg()
        {
            DoDetermineMinSpanTreeCost_Kruskals(PathToTestFile, 7);
        }

        public static void DoTestDetermineMinSpanTree1_KruskalsAlg()
        {
            DoDetermineMinSpanTreeCost_Kruskals(PathToTestFile1, 3);
        }

        #region PQ1

        public static void DoMaximizeMinDistanceBetweenClusters_KruskalsAlg()
        {
            DoDetermineMaximumMinSpacingBetweenClusters(PathToFileQ1, 4, 500);
        }

        public static void DoTestMaximizeMinDistanceBetweenClusters0_KruskalsAlg()
        {
            DoDetermineMaximumMinSpacingBetweenClusters(PathToTestFile, 2, 7);
        }

        public static void DoTestMaximizeMinDistanceBetweenClusters1_KruskalsAlg()
        {
            DoDetermineMaximumMinSpacingBetweenClusters(PathToTestFile2, 4, 50);
        }

        public static void DoTestMaximizeMinDistanceBetweenClusters2_KruskalsAlg()
        {
            DoDetermineMaximumMinSpacingBetweenClusters(PathToTestFile3, 4, 100);
        }

        #endregion PQ1

        #region PQ2

        public static void DoTheThing()
        {
            List<Vertex_UnionFind<int>> vertList = new List<Vertex_UnionFind<int>>();
            UnionFind<Vertex_UnionFind<int>, int> uf = new UnionFind<Vertex_UnionFind<int>, int>(vertList);

            if (!File.Exists(PathToFileQ2))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", PathToFileQ2));
            }

            string[] allLines = File.ReadAllLines(PathToFileQ2);

            //Create all the vertices first
            for (int i = 0; i < allLines.Length; i++)
            {
                int vertex;
                string bitString = allLines[i].Replace(" ", string.Empty);
                if (bitString.Length != 24)
                {
                    throw new Exception();
                }

                vertex = Convert.ToInt32(bitString, 2);

                if(!uf.HasVertex(vertex))
                {                
                    Vertex_UnionFind<int> v = new Vertex_UnionFind<int>(vertex);
                    uf.AddVertex(v);
                    foreach (int key in uf.Clusters.Keys)
                    {
                        int hamDist = Utilities.CalculateHammingDistance(v.Leader.Index, key);
                        if (!v.Index.Equals(key) && hamDist < 3)
                        {
                            uf.Union((Vertex_UnionFind<int>)v.Leader, (Vertex_UnionFind<int>)uf.GetVertex(key).Leader);
                        }
                    }
                }
            }

            //int count = uf.Vertices.Count;
            //for (int i = 0; i < count; i++)
            //{
            //    for (int j = i + 1; j < count; j++)
            //    {
            //        Vertex_UnionFind<int> verti = uf.GetVertexAt(i);
            //        Vertex_UnionFind<int> vertj = uf.GetVertexAt(j);
            //        int hamDist = Utilities.CalculateHammingDistance(verti.Index, vertj.Index);
            //        if (!verti.HasSameLeader(vertj) && hamDist < 3)
            //        {
            //            uf.Union((Vertex_UnionFind<int>)verti.Leader, (Vertex_UnionFind<int>)vertj.Leader);
            //        }
            //    }
            //}

            Console.WriteLine(string.Format("{0}", uf.Clusters.Keys.Count));
        }

        #endregion PQ2
    }
}
