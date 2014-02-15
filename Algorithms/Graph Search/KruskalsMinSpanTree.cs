using System;
using System.Collections.Generic;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;

namespace Algorithms.Graph_Search
{
    public class KruskalsAlgorithm<T, U> 
        where T : Vertex_UnionFind<U>, ILeader<U>
        where U : struct, IComparable, IEquatable<U>
    {
        private Graph_AdjacencyList<T, U> graph;
        public KruskalsAlgorithm(Graph_AdjacencyList<T, U> graph)
        {
            this.graph = graph;
        }

        public Graph_AdjacencyList<T, U> FindMinimumSpaningTree()
        {
            List<T> vertexList = new List<T>(this.graph.Order);

            foreach (T vertex in this.graph.Vertices)
            {
                vertexList.Add((T)new Vertex_UnionFind<U>(vertex.Index, vertex.Data));
            }

            List<Edge_UnionFind<U>> edgeList = new List<Edge_UnionFind<U>>();
            foreach (Edge<U> edge in this.graph.Edges)
            {
                edgeList.Add(new Edge_UnionFind<U>(
                    new Vertex_UnionFind<U>(edge.Tail.Index, edge.Tail.Data),
                    new Vertex_UnionFind<U>(edge.Head.Index, edge.Head.Data),
                    edge.Weight));
            }

            //Sort the edges so lowest cost are first
            edgeList.Sort();

            return this.FindMinimumSpaningTree(vertexList, edgeList);
        }

        public Graph_AdjacencyList<T, U> FindMinimumSpaningTree(List<T> listOfUnconnectedVertices, List<Edge_UnionFind<U>> sortedListOfEdges)
        {
            Edge<U> dummy;
            return this.DoKruskalsAlgorithm(listOfUnconnectedVertices, sortedListOfEdges, 1, out dummy);
        }

        public Edge<U> FindMaxSpacingForKClusters(int numClusters)
        {
            List<T> vertexList = new List<T>(this.graph.Order);

            foreach (T vertex in this.graph.Vertices)
            {
                vertexList.Add((T)new Vertex_UnionFind<U>(vertex.Index, vertex.Data));
            }

            List<Edge_UnionFind<U>> edgeList = new List<Edge_UnionFind<U>>();
            foreach (Edge<U> edge in this.graph.Edges)
            {
                edgeList.Add(new Edge_UnionFind<U>(
                    new Vertex_UnionFind<U>(edge.Tail.Index, edge.Tail.Data),
                    new Vertex_UnionFind<U>(edge.Head.Index, edge.Head.Data),
                    edge.Weight));
            }

            //Sort the edges so lowest cost are first
            edgeList.Sort();

            return this.FindMaxSpacingForKClusters(vertexList, edgeList, numClusters);
        }

        public Edge<U> FindMaxSpacingForKClusters(List<T> listOfUnconnectedVertices, List<Edge_UnionFind<U>> sortedListOfEdges, int numClusters)
        {
            Edge<U> nextMinimumDistance;
            this.DoKruskalsAlgorithm(listOfUnconnectedVertices, sortedListOfEdges, numClusters, out nextMinimumDistance);
            return nextMinimumDistance;
        }

        private Graph_AdjacencyList<T, U> DoKruskalsAlgorithm(List<T> listOfUnconnectedVertices, List<Edge_UnionFind<U>> sortedListOfEdges, int numClusters, out Edge<U> nextLowestCostEdge)
        {
            UnionFind<T, U> uf = new UnionFind<T, U>(listOfUnconnectedVertices);
            int i = 0;

            if (numClusters < 1)
            {
                throw new ArgumentException(string.Format("Expected number of clusters to be greater than or equal to 1 but value was {0}!", numClusters), "numClusters");
            }

            while (uf.Clusters.Keys.Count > numClusters && i < sortedListOfEdges.Count)
            {
                Edge_UnionFind<U> currentEdge = sortedListOfEdges[i];

                T vTail = uf.GetVertex(currentEdge.Tail.Index);
                T vHead = uf.GetVertex(currentEdge.Head.Index);

                if (!vTail.HasSameLeader(vHead))
                {
                    //Found two clusters that are separated
                    //Add edge to the min span tree
                    uf.AddUndirectedEdge(vTail, vHead, currentEdge.Weight);
                    //merge them!
                    uf.Union((T)vTail.Leader, (T)vHead.Leader);
                }
                i++;
            }

            if (uf.Clusters.Keys.Count > numClusters)
            {
                throw new Exception(string.Format("Ran out of edges before reaching target number of clusters. Target #: {0}, Current #: {1}", numClusters, uf.Clusters.Keys.Count));
            }

            if (numClusters == 1)
            {
                Console.WriteLine("Can't return next lowest cost edge because there are no more edges that cross a boundary");
                nextLowestCostEdge = null;
            }            
            else
            {
                if (i < sortedListOfEdges.Count)
                {
                    while (i < sortedListOfEdges.Count)
                    {
                        Edge_UnionFind<U> current = sortedListOfEdges[i];
                        T tail = uf.GetVertex(current.Tail.Index);
                        T head = uf.GetVertex(current.Head.Index);
                        if (!tail.HasSameLeader(head))
                        {
                            break;
                        }
                        i++;
                    }
                    nextLowestCostEdge = sortedListOfEdges[i];
                }
                else
                {
                    throw new Exception(string.Format("Cant return next lowest cost edge because we have exceeded the bounds of the array!"));
                }
            }

            Console.WriteLine(string.Format("Exiting Kruskal's Algorithm with {0} clusters", uf.Clusters.Count));
            foreach(int key in uf.Clusters.Keys)
            {
                Console.WriteLine(string.Format("Cluster with leader {0} has {1} elements.", key, uf.Clusters[key]));
            }


            return uf.Graph;
        }
    }
}
