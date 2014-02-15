using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class Graph_AdjacencyList<T, U> 
        where T : Vertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        protected List<Edge<U>> edges;
        protected Dictionary<int, T> vertices;

        public static Graph_AdjacencyList<T, U> ReverseGraph(Graph_AdjacencyList<T, U> graphToReverse)
        {
            Graph_AdjacencyList<T, U> reversedGraph = graphToReverse.DeepCopy();

            foreach (Edge<U> edge in reversedGraph.Edges)
            {
                T temp = (T)edge.Head;
                edge.Head = edge.Tail;
                edge.Tail = temp;
            }

            //Delete neighbors
            foreach(T v in reversedGraph.Vertices)
            {
                foreach (int n in v.Neighbors.Keys)
                {
                    v.AddNeighbor(v.Neighbors[n]);
                }
            }
            return reversedGraph;
        }

        public List<Edge<U>> Edges
        {
            get
            {
                List<Edge<U>> edgeList = new List<Edge<U>>();
                foreach (Edge<U> edge in this.edges)
                {
                    edgeList.Add(edge);
                }

                return edgeList;
            }
        }

        public List<T> Vertices
        {
            get
            {
                List<T> vertexList = new List<T>();

                foreach (int key in this.vertices.Keys)
                {
                    vertexList.Add(this.vertices[key]);
                }
                return vertexList;
            }
        }

        #region Constructors

        public Graph_AdjacencyList()
        {
            this.vertices = new Dictionary<int, T>();
            this.edges = new List<Edge<U>>();
        }

        public Graph_AdjacencyList(int numberOfVerticies) : this(numberOfVerticies, numberOfVerticies) { }

        public Graph_AdjacencyList(int numberOfVerticies, int numberOfEdges)
        {
            this.vertices = new Dictionary<int, T>(numberOfVerticies) { };
            this.edges = new List<Edge<U>>(numberOfEdges) { };  
        }

        public Graph_AdjacencyList(Dictionary<int, T> vertices)
        {
            this.vertices = new Dictionary<int, T>(vertices);
            this.edges = new List<Edge<U>>();
        }

        public Graph_AdjacencyList(List<T> vertexList)
        {
            this.vertices = new Dictionary<int, T>();
            foreach (T v in vertexList)
            {
                if (!this.vertices.ContainsKey(v.Index))
                {
                    this.vertices.Add(v.Index, v);
                }
            }
            this.edges = new List<Edge<U>>();
        }

        public Graph_AdjacencyList(Graph_AdjacencyList<T, U> graph)
        {
            this.vertices = new Dictionary<int, T>(graph.vertices);
            this.edges = new List<Edge<U>>(graph.Edges);
        }

        #endregion Constructors

        /// <summary>
        /// The number of vertices in the graph
        /// </summary>
        public int Order
        {
            get { return this.vertices.Count; }
        }

        /// <summary>
        /// The number of edges in the graph
        /// </summary>
        public int Size { get { return this.edges.Count; } }

        public T GetVertex(int index)
        {
            if (!this.HasVertex(index))
            {
                throw new ArgumentException(string.Format("The index {0} does not exist in the graph!", index));
            }
            return this.vertices[index];
        }

        public T GetVertexAt(int location)
        {
            if (location < 0 && location >= this.vertices.Keys.Count)
            {
                throw new ArgumentException(string.Format("Location cannot be less than zero or greater than {0}", this.vertices.Count));
            }
            return this.vertices.ElementAt(location).Value;
        }

        public bool HasVertex(int index)
        {
            return this.vertices.ContainsKey(index);
        }

        public void MarkAllVerticesAsUnvisited()
        {
            foreach (int v in this.vertices.Keys)
            {
                this.vertices[v].Visited = false;
            }
        }

        public void RemoveVertex(T vertexToRemove)
        {
            if (vertexToRemove != null)
            {
                int index = vertexToRemove.Index;
                
                if (this.vertices.ContainsKey(index))
                {
                    //Remove the vertex from all the lists of neighbors in the graph
                    List<int> neighborIndices = vertexToRemove.Neighbors.Keys.ToList();
                    foreach(int neighbor in neighborIndices)
                    {
                        this.vertices[neighbor].RemoveNeighbor(vertexToRemove);
                    }

                    //remove all the edges eminating or terminating at the vertex to remove
                    List<Edge<U>> edgesToRemove = new List<Edge<U>>();
                    foreach (Edge<U> e in this.edges)
                    {
                        if (e.Head.Equals(vertexToRemove) || e.Tail.Equals(vertexToRemove))
                        {
                            edgesToRemove.Add(e);
                        }
                    }

                    foreach (Edge<U> ed in edgesToRemove)
                    {
                        this.RemoveEdge(ed);
                    }

#if DEBUG
                    //Console.WriteLine(string.Format("Removing vertex {0}", vertexToRemove.Index));
#endif
                    //Remove all neighbors from the vertex
                    vertexToRemove.RemoveAllNeighbors();

                    //Finally Remove the target vertex from the graph
                    this.vertices.Remove(index);
                }
            }
        }

        public void AddVertex(T vertexToAdd)
        {
            if (!this.vertices.ContainsKey(vertexToAdd.Index))
            {
                this.vertices.Add(vertexToAdd.Index, vertexToAdd);
            }
        }
                
        public void AddDirectedEdge(T from, T to, double cost = 0)
        {
            this.AddDirectedEdge(from.Index, to.Index, cost);
        }

        public void AddDirectedEdge(int indexFrom, int indexTo, double cost = 0)
        {
            this.vertices[indexFrom].AddNeighbor(this.vertices[indexTo]);
            this.edges.Add(new Edge<U>(this.vertices[indexFrom], this.vertices[indexTo], cost));
        }

        public void AddUndirectedEdge(T from, T to, double cost = 0)
        {
            this.AddDirectedEdge(from.Index, to.Index, cost);
            this.AddDirectedEdge(to.Index, from.Index, cost);
        }

        public void RemoveEdge(Edge<U> edgeToRemove)
        {
            if (this.edges.Contains(edgeToRemove))
            {
                this.edges.Remove(edgeToRemove);
            }
        }

        public Graph_AdjacencyList<T, U> DeepCopy()
        {
            Graph_AdjacencyList<T, U> copyToReturn = new Graph_AdjacencyList<T, U>(this.Order);

            foreach (int v in this.vertices.Keys)
            {
                copyToReturn.AddVertex(this.vertices[v]);
            }

            foreach (Edge<U> e in this.edges)
            {
                copyToReturn.AddDirectedEdge((T)e.Tail, (T)e.Head, e.Weight);
            }

            return copyToReturn;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (int v in this.vertices.Keys)
            {
                T vert = this.vertices[v];
                sb.AppendFormat("Index: {0}\tNeighbors: [", vert.Index);

                foreach (int neighborIndex in vert.Neighbors.Keys)
                {
                    sb.AppendFormat("{0}, ", this.vertices[neighborIndex].Index);
                }
                sb.Remove(sb.Length - 2, 2);
                sb.AppendFormat("]{0}", Environment.NewLine);
            }
            return sb.ToString();
        }
    }

    public class Graph_AdjacencyList
    {
        public List<Edge_Lite> Edges;
        public Dictionary<int, Vertex_Lite> Vertices;

        public Graph_AdjacencyList(int numVertices = 0)
        {
            if (0 == numVertices)
            {
                this.Vertices = new Dictionary<int, Vertex_Lite>();
                this.Edges = new List<Edge_Lite>();
            }
            else
            {
                this.Vertices = new Dictionary<int, Vertex_Lite>(numVertices);
                this.Edges = new List<Edge_Lite>(numVertices); //best guess
            }
        }

        public int Order { get { return this.Vertices.Count; } }
        public int Size { get { return this.Edges.Count; } }

        public List<int> VertexIndices
        {
            get { return this.Vertices.Keys.ToList(); }
        }

        public void AddVertex(Vertex_Lite vertexToAdd)
        {
            if (!this.Vertices.ContainsKey(vertexToAdd.Index))
            {
                this.Vertices.Add(vertexToAdd.Index, vertexToAdd);
            }
        }

        public void RemoveVertex(Vertex_Lite vertexToRemove)
        {
            this.RemoveVertex(vertexToRemove.Index);
        }

        public void RemoveVertex(int indexOfVertexToRemove)
        {
            if (this.Vertices.ContainsKey(indexOfVertexToRemove))
            {
                List<int> edgeIndicesToRemove = new List<int>();
                for (int i = 0; i < this.Edges.Count; i++)
                {
                    Edge_Lite e = this.Edges[i];
                    if (e.Head.Equals(indexOfVertexToRemove) || e.Tail.Equals(indexOfVertexToRemove))
                    {
                        //mark for deletion later
                        edgeIndicesToRemove.Add(i);
                    }
                }

                for (int j = 0; j < edgeIndicesToRemove.Count; j++)
                {
#if DEBUG
                    Console.WriteLine(string.Format("Removing edge: {0}", this.Edges[j].ToString()));
#endif
                    this.Edges.RemoveAt(j);
                }

#if DEBUG
                Console.WriteLine(string.Format("Removing vertex: {0}", this.Vertices[indexOfVertexToRemove].ToString()));
#endif

                this.Vertices.Remove(indexOfVertexToRemove);
            }
        }

        public void AddDirectedEdge(Edge_Lite edgeToAdd)
        {
            if (!this.Edges.Contains(edgeToAdd))
            {
                this.Edges.Add(edgeToAdd);
            }
        }

        public void AddDirectedEdge(int tailIndex, int headIndex, double weight = 0)
        {
            if (this.Vertices.ContainsKey(headIndex) && this.Vertices.ContainsKey(tailIndex))
            {
                this.AddDirectedEdge(new Edge_Lite(tailIndex, headIndex, weight));                
            }
            else
            {
                throw new ArgumentException("Cannot add edge because one of the vertices is not yet added to the graph!");
            }
        }

        public void AddDirectedEdge(Vertex_Lite tail, Vertex_Lite head, double weight = 0)
        {
            this.AddDirectedEdge(tail.Index, head.Index, weight);
        }

        public void AddUndirectedEdge(int vert1, int vert2, double weight = 0)
        {
            this.AddDirectedEdge(vert1, vert2, weight);
            this.AddDirectedEdge(vert2, vert1, weight);
        }

        public void AddUndirectedEdge(Vertex_Lite vert1, Vertex_Lite vert2, double weight)
        {
            this.AddDirectedEdge(vert1, vert2, weight);
            this.AddDirectedEdge(vert2, vert1, weight);
        }

        public void RemoveEdge(Edge_Lite edgeToRemove)
        {
            if (this.Edges.Contains(edgeToRemove))
            {
                this.Edges.Remove(edgeToRemove);
            }
        }

        public List<Vertex_Lite> FindNeighbors(Vertex_Lite vertexToFindNeighborsFor)
        {
            List<Vertex_Lite> neighbors = new List<Vertex_Lite>();

            foreach (Edge_Lite e in this.Edges)
            {
                if (e.Tail.Equals(vertexToFindNeighborsFor.Index))
                {
                    neighbors.Add(this.Vertices[e.Head].DeepCopy());
                }
            }

            return neighbors;
        }

        public List<int> FindNeighbors(int indexOfVertexToFindNeighborsFor)
        {
            List<int> neighboringIndices = new List<int>();

            foreach (Edge_Lite e in this.Edges)
            {
                if(e.Tail.Equals(indexOfVertexToFindNeighborsFor))
                {
                    neighboringIndices.Add(e.Head);
                }
            }

            return neighboringIndices;
        }

        public List<Edge_Lite> FindEdgesOriginatingFromVertex(int indexOfVertex)
        {
            List<Edge_Lite> edgesOriginating = new List<Edge_Lite>();
            foreach (Edge_Lite e in this.Edges)
            {
                if (e.Tail.Equals(indexOfVertex))
                {
                    edgesOriginating.Add(e);
                }
            }

            return edgesOriginating;
        }

        public List<Edge_Lite> FindEdgesOriginatingFromVertex(Vertex_Lite vertex)
        {
            return this.FindEdgesOriginatingFromVertex(vertex.Index);
        }

        public List<Edge_Lite> FindEdgesEndingAtVertex(int indexOfVertex)
        {
            List<Edge_Lite> edgesEnding = new List<Edge_Lite>();
            foreach (Edge_Lite e in this.Edges)
            {
                if (e.Head.Equals(indexOfVertex))
                {
                    edgesEnding.Add(e);
                }
            }

            return edgesEnding;
        }

        public List<Edge_Lite> FindEdgesEndingAtVertex(Vertex_Lite vertex)
        {
            return this.FindEdgesEndingAtVertex(vertex.Index);
        }

        public Graph_AdjacencyList DeepCopy()
        {
            Graph_AdjacencyList copyToReturn = new Graph_AdjacencyList(this.Order);

            foreach (int v in this.Vertices.Keys)
            {
                copyToReturn.AddVertex(this.Vertices[v]);
            }

            foreach (Edge_Lite e in this.Edges)
            {
                copyToReturn.AddDirectedEdge(e.Tail, e.Head);
            }

            return copyToReturn;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (int v in this.Vertices.Keys)
            {
                sb.AppendFormat("Index: {0}\tNeighbors: [", v);

                List<int> neighboringIndices = this.FindNeighbors(this.Vertices[v].Index);
                for (int i = 0; i < neighboringIndices.Count; i++)
                {
                    sb.AppendFormat("{0}", neighboringIndices[i]);
                    if (i < neighboringIndices.Count - 1)
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
