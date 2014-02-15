using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class Graph_AdjacencyList<T, U> : IGraph<T, U>
        where T : Vertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        protected List<Edge<U>> edges;
        protected Dictionary<int, T> vertices;

        public static Graph_AdjacencyList<T, U> ReverseGraph(IGraph<T, U> graphToReverse)
        {
            Graph_AdjacencyList<T, U> reversedGraph = new Graph_AdjacencyList<T, U>(graphToReverse.Vertices.Count);

            foreach (T vertex in graphToReverse.Vertices)
            {
                //reversedGraph.AddVertex((T)new Vertex<U>(vertex.Index, vertex.Data));
                reversedGraph.AddVertex((T)vertex.ShallowCopy());
            }

            foreach (Edge<U> e in graphToReverse.Edges)
            {
                reversedGraph.AddDirectedEdge((T)e.Head, (T)e.Tail, e.Weight);
            }

            return reversedGraph;
        }

        public List<Edge<U>> Edges
        {
            get
            {
                return this.edges.AsReadOnly().ToList<Edge<U>>();
            }
        }

        public List<T> Vertices
        {
            get
            {
                return this.vertices.Values.ToList<T>();
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

        public Graph_AdjacencyList(IList<T> vertexList)
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
            this.vertices[indexFrom].AddArc(new Edge<U>(indexFrom, indexTo, cost));
            this.vertices[indexFrom].AddNeighbor(this.vertices[indexTo]);
            this.edges.Add(new Edge<U>(this.vertices[indexFrom], this.vertices[indexTo], cost));
        }

        public void AddUndirectedEdge(T vertexA, T vertexB, double cost = 0)
        {
            this.AddUndirectedEdge(vertexA.Index, vertexB.Index, cost);
        }

        public void AddUndirectedEdge(int indexA, int indexB, double cost = 0)
        {
            this.AddDirectedEdge(indexA, indexB, cost);
            this.AddDirectedEdge(indexB, indexA, cost);
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

                if (vert.Neighbors.Keys.Count > 0)
                {
                    sb.Remove(sb.Length - 2, 2);
                }
                sb.AppendFormat("]{0}", Environment.NewLine);
            }
            return sb.ToString();
        }

        #region IGraph Implementation

        IList<T> IGraph<T, U>.Vertices { get { return this.Vertices; } }

        IList<IEdge<U>> IGraph<T, U>.Edges { get { return this.Edges.ToList<IEdge<U>>(); } }
        
        public void RemoveEdge(IEdge<U> edgeToRemove)
        {
            Edge<U> edge;
            if (edgeToRemove is Edge<U>)
            {
                edge = (Edge<U>)edgeToRemove;
            }
            else
            {
                edge = new Edge<U>(edgeToRemove.Tail.Index, edgeToRemove.Head.Index);
            }

            this.RemoveEdge(edge);
        }

        IGraph<T, U> IGraph<T, U>.DeepCopy()
        {
            return this.DeepCopy();
        }

        #endregion IGraph Implementation
    }

    public class Graph_AdjacencyList : IGraph<Vertex<int>, int>
    {
        public List<Edge<int>> Edges;
        public Dictionary<int, Vertex<int>> Vertices;

        public Graph_AdjacencyList(int numVertices = 0)
        {
            if (0 == numVertices)
            {
                this.Vertices = new Dictionary<int, Vertex<int>>();
                this.Edges = new List<Edge<int>>();
            }
            else
            {
                this.Vertices = new Dictionary<int, Vertex<int>>(numVertices);
                this.Edges = new List<Edge<int>>(numVertices); //best guess
            }
        }

        public int Order { get { return this.Vertices.Count; } }
        public int Size { get { return this.Edges.Count; } }

        public List<int> VertexIndices
        {
            get { return this.Vertices.Keys.ToList(); }
        }

        public void AddVertex(Vertex<int> vertexToAdd)
        {
            if (!this.Vertices.ContainsKey(vertexToAdd.Index))
            {
                this.Vertices.Add(vertexToAdd.Index, vertexToAdd);
            }
        }

        public void RemoveVertex(Vertex<int> vertexToRemove)
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
                    Edge<int> e = this.Edges[i];
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

        public void AddDirectedEdge(Edge<int> edgeToAdd)
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
                this.AddDirectedEdge(new Edge<int>(tailIndex, headIndex, weight));                
            }
            else
            {
                throw new ArgumentException("Cannot add edge because one of the vertices is not yet added to the graph!");
            }
        }

        public void AddDirectedEdge(Vertex<int> tail, Vertex<int> head, double weight = 0)
        {
            this.AddDirectedEdge(tail.Index, head.Index, weight);
        }

        public void AddUndirectedEdge(int vert1, int vert2, double weight = 0)
        {
            this.AddDirectedEdge(vert1, vert2, weight);
            this.AddDirectedEdge(vert2, vert1, weight);
        }

        public void AddUndirectedEdge(Vertex<int> vert1, Vertex<int> vert2, double weight)
        {
            this.AddDirectedEdge(vert1, vert2, weight);
            this.AddDirectedEdge(vert2, vert1, weight);
        }

        public void AddUndirectedEdge(IVertex<int> vert1, IVertex<int> vert2, double weight)
        {
            this.AddDirectedEdge(vert1.Index, vert2.Index, weight);
            this.AddDirectedEdge(vert2.Index, vert1.Index, weight);
        }

        public void RemoveEdge(Edge<int> edgeToRemove)
        {
            if (this.Edges.Contains(edgeToRemove))
            {
                this.Edges.Remove(edgeToRemove);
            }
        }

        public List<Vertex<int>> FindNeighbors(Vertex<int> vertexToFindNeighborsFor)
        {
            List<Vertex<int>> neighbors = new List<Vertex<int>>();

            foreach (Edge<int> e in this.Edges)
            {
                if (e.Tail.Equals(vertexToFindNeighborsFor.Index))
                {
                    neighbors.Add(this.Vertices[e.Head.Index].DeepCopy());
                }
            }

            return neighbors;
        }

        public List<int> FindNeighbors(int indexOfVertexToFindNeighborsFor)
        {
            List<int> neighboringIndices = new List<int>();

            foreach (Edge<int> e in this.Edges)
            {
                if(e.Tail.Equals(indexOfVertexToFindNeighborsFor))
                {
                    neighboringIndices.Add(e.Head.Index);
                }
            }

            return neighboringIndices;
        }

        public List<Edge<int>> FindEdgesOriginatingFromVertex(int indexOfVertex)
        {
            List<Edge<int>> edgesOriginating = new List<Edge<int>>();
            foreach (Edge<int> e in this.Edges)
            {
                if (e.Tail.Equals(indexOfVertex))
                {
                    edgesOriginating.Add(e);
                }
            }

            return edgesOriginating;
        }

        public List<Edge<int>> FindEdgesOriginatingFromVertex(Vertex<int> vertex)
        {
            return this.FindEdgesOriginatingFromVertex(vertex.Index);
        }

        public List<Edge<int>> FindEdgesEndingAtVertex(int indexOfVertex)
        {
            List<Edge<int>> edgesEnding = new List<Edge<int>>();
            foreach (Edge<int> e in this.Edges)
            {
                if (e.Head.Equals(indexOfVertex))
                {
                    edgesEnding.Add(e);
                }
            }

            return edgesEnding;
        }

        public List<Edge<int>> FindEdgesEndingAtVertex(Vertex<int> vertex)
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

            foreach (Edge<int> e in this.Edges)
            {
                copyToReturn.AddDirectedEdge(e.Tail.Index, e.Head.Index);
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

        #region IGraph Implementation

        IList<Vertex<int>> IGraph<Vertex<int>, int>.Vertices
        {
            get { return this.Vertices.Values.ToList<Vertex<int>>(); }
        }

        IList<IEdge<int>> IGraph<Vertex<int>, int>.Edges
        {
            get { return this.Edges.AsReadOnly().ToList<IEdge<int>>(); }
        }

        public bool HasVertex(int index)
        {
            return this.Vertices.ContainsKey(index);
        }

        public Vertex<int> GetVertex(int index)
        {
            if (!this.Vertices.ContainsKey(index))
            {
                throw new ArgumentException(string.Format("The index {0} does not exist in the graph!", index));
            }
            return this.Vertices[index];
        }

        public Vertex<int> GetVertexAt(int location)
        {
            if (location < 0 && location >= this.Vertices.Keys.Count)
            {
                throw new ArgumentException(string.Format("Location cannot be less than zero or greater than {0}", this.Vertices.Count));
            }

            return this.Vertices.ElementAt(location).Value;
        }

        public void RemoveEdge(IEdge<int> edgeToRemove)
        {
            this.RemoveEdge(edgeToRemove as Edge<int>);
        }

        IGraph<Vertex<int>, int> IGraph<Vertex<int>, int>.DeepCopy()
        {
            return this.DeepCopy();
        }

        #endregion IGraph Implementation
    }
}
