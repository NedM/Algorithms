using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class Graph_AdjacencyList_Lite 
    {
        public static Graph_AdjacencyList_Lite ReverseGraph(Graph_AdjacencyList_Lite graphToReverse)
        {
            Graph_AdjacencyList_Lite reversedGraph = new Graph_AdjacencyList_Lite(graphToReverse.Order);

            foreach (int v in graphToReverse.Vertices.Keys)
            {
                reversedGraph.AddVertex(v);
                List<int> neighbors = graphToReverse.Vertices[v];
                foreach (int neighbor in neighbors)
                {                    
                    if (!reversedGraph.Vertices.ContainsKey(neighbor))
                    {
                        reversedGraph.AddVertex(neighbor);
                    }
                    //reversedGraph.AddDirectedEdge(neighbor, v, graphToReverse.Edges[new Tuple<int,int>(v, neighbor)]);
                }
            }

            foreach (Tuple<int, int> e in graphToReverse.Edges.Keys)
            {
                reversedGraph.AddDirectedEdges(e.Item2, e.Item1, graphToReverse.Edges[e]);
            }

            return reversedGraph;
        }

        public MultiValueDictionary<int, int> Vertices;
        public MultiValueDictionary<Tuple<int, int>, double> Edges;
        public Dictionary<int, bool> Visited;

        public Graph_AdjacencyList_Lite(int numberOfVerticies = 0)
        {
            if (0 == numberOfVerticies)
            {
                this.Vertices = new MultiValueDictionary<int, int>();
                this.Visited = new Dictionary<int, bool>();
                this.Edges = new MultiValueDictionary<Tuple<int, int>, double>();
            }
            else
            {
                this.Vertices = new MultiValueDictionary<int, int>(numberOfVerticies);
                this.Visited = new Dictionary<int, bool>(numberOfVerticies);
                this.Edges = new MultiValueDictionary<Tuple<int, int>, double>(numberOfVerticies); //Best guess
            }
        }

        public int Order { get { return this.Vertices.Count; } }

        public int Size { get { return this.Edges.Keys.Count; } }

        public void MarkAllVerticesAsUnvisited()
        {
            foreach (int vertex in this.Vertices.Keys)
            {
                if (!this.Visited.ContainsKey(vertex))
                {
                    this.Visited.Add(vertex, false);
                }
                else
                {
                    this.Visited[vertex] = false;
                }
            }
        }

        public void AddVertex(int vertexToAdd)
        {            
            if (!this.Vertices.ContainsKey(vertexToAdd))
            {
                this.Vertices.Add(vertexToAdd, new List<int>());
            }

            if (!this.Visited.ContainsKey(vertexToAdd))
            {
                this.Visited.Add(vertexToAdd, false);
            }
        }
                
        public void AddDirectedEdge(int indexFrom, int indexTo, double cost = 0)
        {           
            this.Vertices.Add(indexFrom, indexTo);
            this.Edges.Add(new Tuple<int, int>(indexFrom, indexTo), cost);
        }

        public void AddDirectedEdges(int indexFrom, int indexTo, ICollection<double> costs)
        {
            foreach (double cost in costs)
            {
                this.AddDirectedEdge(indexFrom, indexTo, cost);
            }
        }

        public void AddUndirectedEdge(int index1, int index2, double cost = 0)
        {
            this.AddDirectedEdge(index1, index2, cost);
            this.AddDirectedEdge(index2, index1, cost);
        }

        public void RemoveEdge(int indexFrom, int indexTo)
        {
            this.Vertices.RemoveValue(indexFrom, indexTo);
            this.Edges.Remove(new Tuple<int, int>(indexFrom, indexTo));
        }

        public Graph_AdjacencyList_Lite DeepCopy()
        {
            Graph_AdjacencyList_Lite copyToReturn = new Graph_AdjacencyList_Lite(this.Order);

            foreach (int v in this.Vertices.Keys)
            {
                copyToReturn.Vertices.Add(v, this.Vertices[v]);
                copyToReturn.Visited.Add(v, this.Visited[v]);
            }

            foreach (Tuple<int, int> e in this.Edges.Keys)
            {
                copyToReturn.Edges.Add(e, this.Edges[e]);
            }

            return copyToReturn;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (int v in this.Vertices.Keys)
            {
                sb.AppendFormat("Index: {0}\tNeighbors: [", v);

                List<int> neighbors = this.Vertices[v];
                for (int i = 0; i < neighbors.Count; i++)
                {
                    sb.AppendFormat("{0}", neighbors[i]);
                    if (i < neighbors.Count - 1)
                    {
                        sb.Append(",");
                    }
                }
                sb.AppendFormat("]{0}", Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
