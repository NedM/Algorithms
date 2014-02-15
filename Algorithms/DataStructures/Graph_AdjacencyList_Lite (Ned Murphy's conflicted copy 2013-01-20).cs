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
                    reversedGraph.AddDirectedEdge(neighbor, v);
                }
            }

            return reversedGraph;
        }

        public MultiValueDictionary<int, int> Vertices;
        public Dictionary<int, bool> Visited;

        public Graph_AdjacencyList_Lite(int numberOfVerticies = 0)
        {
            if (0 == numberOfVerticies)
            {
                this.Vertices = new MultiValueDictionary<int, int>();
                this.Visited = new Dictionary<int, bool>();
            }
            else
            {
                this.Vertices = new MultiValueDictionary<int, int>(numberOfVerticies);
                this.Visited = new Dictionary<int, bool>(numberOfVerticies);
            }
        }

        public int Order { get { return this.Vertices.Count; } }

        public int Size
        {
            get
            {
                int count = 0;
                foreach (int key in this.Vertices.Keys)
                {
                    count += this.Vertices[key].Count;
                }
                return count;
            }
        }

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
                
        public void AddDirectedEdge(int indexFrom, int indexTo, int cost = 0)
        {           
            this.Vertices.Add(indexFrom, indexTo);
        }

        public void RemoveEdge(int indexFrom, int indexTo)
        {
            this.Vertices.RemoveValue(indexFrom, indexTo);
        }

        public Graph_AdjacencyList_Lite DeepCopy()
        {
            Graph_AdjacencyList_Lite copyToReturn = new Graph_AdjacencyList_Lite(this.Order);

            foreach (int v in this.Vertices.Keys)
            {
                copyToReturn.Vertices.Add(v, this.Vertices[v]);
                copyToReturn.Visited.Add(v, this.Visited[v]);
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
            //return base.ToString();
        }
    }
}
