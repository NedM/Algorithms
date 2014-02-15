using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public interface IVertex<T> where T : struct
    {
        int Index { get; }
        T Data { get; set; }
        bool Visited { get; set; }
    }

    public class VertexBase<T> : IVertex<T>, IComparable, IEquatable<IVertex<T>> where T : struct, IComparable, IEquatable<T>
    {
        public VertexBase(int index)
        {
            this.Index = index;
            this.Visited = false;
        }

        public VertexBase(int index, T data)
        {
            this.Index = index;
            this.Data = data;
            this.Visited = false;
        }

        public int Index { get; protected set; }
        public T Data { get; set; }
        public bool Visited { get; set; }

        public override bool Equals(object obj)
        {
            bool bReturn = false;
            if (!(obj is VertexBase<T>))
            {
                bReturn = false;
            }
            else
            {
                bReturn = this.Equals((VertexBase<T>)obj);
            }
            return bReturn;
        }

        public bool Equals(IVertex<T> other)
        {
            return other.Index.Equals(this.Index);            
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + this.Index;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is VertexBase<T>))
            {
                throw new ArgumentException(string.Format("Comparison is not defined for types {0} and {1}", this.GetType().ToString(), obj.GetType().ToString()));
            }

            VertexBase<T> vertex = (VertexBase<T>)obj;

            return this.Data.CompareTo(vertex.Data);            
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", this.Index, this.Data.ToString());
        }
    }

    public class Vertex<T> : VertexBase<T> where T : struct, IComparable, IEquatable<T>
    {
        public Dictionary<int, Vertex<T>> Neighbors;

        public Vertex(int index) : base(index) 
        {
            this.Neighbors = new Dictionary<int, Vertex<T>>();
        }

        public Vertex(int index, T data) : this(index, data, new Dictionary<int, Vertex<T>>()) { }

        public Vertex(int index, T data, Dictionary<int, Vertex<T>> neighbors)
            : base(index, data)
        {
            this.Neighbors = new Dictionary<int, Vertex<T>>(neighbors);
        }

        public List<Edge<T>> Arcs
        {
            get
            {
                List<Edge<T>> rList = new List<Edge<T>>();
                foreach (int node in this.Neighbors.Keys)
                {
                    rList.Add(new Edge<T>(this, this.Neighbors[node]));
                }
                return rList;
            }
        }

        public void AddNeighbor(Vertex<T> neighbor)
        {
            if (!this.Neighbors.ContainsKey(neighbor.Index))
            {
                this.Neighbors.Add(neighbor.Index, neighbor);
            }
        }

        public void RemoveNeighbor(Vertex<T> neighbor)
        {
            this.RemoveNeighbor(neighbor.Index);
        }

        public void RemoveNeighbor(int index)
        {
            if (this.Neighbors.ContainsKey(index))
            {
                this.Neighbors.Remove(index);
            }
        }

        public void RemoveAllNeighbors()
        {
            this.Neighbors.Clear();
        }

        public int Degree
        {
            get { return this.Neighbors.Count; }
        }
    }

    public struct Vertex_Lite : IVertex<int>, IEquatable<Vertex_Lite>, IComparable<Vertex_Lite>
    {
        private int index;
        private int data;
        private bool visited;

        public Vertex_Lite(int index, int data = 0)
        {
            this.index = index;
            this.data = data;
            this.visited = false;
        }

        public int Index { get { return this.index; } }
        public int Data { get { return this.data; } set { this.data = value; } }
        public bool Visited { get { return this.visited; } set { this.visited = value; } }

        public Vertex_Lite DeepCopy()
        {
            Vertex_Lite copy = new Vertex_Lite(this.Index, this.Data);
            copy.Visited = this.Visited;
            return copy;
        }

        public bool Equals(Vertex_Lite other)
        {
            return this.Index.Equals(other.Index);
        }

        public int CompareTo(Vertex_Lite other)
        {
            return this.Data.CompareTo(other.Data);
        }

        public override string ToString()
        {
            return string.Format("{{0}, {1}, {2}}", this.Index, this.Data, this.Visited.ToString());
        }
    }

    public class Vertex_UnionFind<T> : Vertex<T>, ILeader<T> where T : struct, IComparable, IEquatable<T>
    {   
        public Vertex_UnionFind(int index) : base(index)
        {
            this.Leader = this;
            this.Rank = 1;
        }

        public Vertex_UnionFind(int index, T data) : this(index, index, data) { }
        public Vertex_UnionFind(int index, int leaderIndex, T data) : this(index, new Vertex_UnionFind<T>(leaderIndex), data) { }
        public Vertex_UnionFind(int index, Vertex_UnionFind<T> leader, T data) : base(index, data)
        {
            this.Leader = leader;
            this.Rank = 1;
        }
        
        public IVertex<T> Leader { get; set; }

        public int Rank { get; set; }

        public bool HasSameLeader(ILeader<T> otherVertex)
        {
            return this.Leader.Equals(otherVertex.Leader);
        }
        
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.Index, this.Leader.Index, this.Data.ToString());
        }
    }

    public struct Vertex_Lite_UnionFind : IVertex<int>, IEquatable<Vertex_Lite_UnionFind>, IComparable<Vertex_Lite_UnionFind>, ILeader<int>
    {
        private int index;
        private int data;
        private IVertex<int> leader;
        private bool visited;
        private int rank;

        public Vertex_Lite_UnionFind(int index, int data = 0) : this(index, new Vertex<int>(index, data), data) { }
        public Vertex_Lite_UnionFind(int index, IVertex<int> leader, int data = 0)
        {
            this.index = index;
            this.data = data;
            this.leader = leader;
            this.visited = false;
            this.rank = 1;
        }

        public int Index { get { return this.index; } }
        public int Data { get { return this.data; } set { this.data = value; } }
        public IVertex<int> Leader { get { return this.leader; } set { this.leader = value; } }
        public bool Visited { get { return this.visited; } set { this.visited = value; } }
        public int Rank { get { return this.rank; } set { this.rank = value; } }

        public Vertex_Lite_UnionFind DeepCopy()
        {
            Vertex_Lite_UnionFind copy = new Vertex_Lite_UnionFind(this.Index, this.Leader, this.Data);
            copy.Visited = this.Visited;
            return copy;
        }

        public bool HasSameLeader(ILeader<int> other)
        {
            return this.Leader.Equals(other.Leader);
        }

        public bool Equals(Vertex_Lite_UnionFind other)
        {
            return this.Index.Equals(other.Index);
        }

        public int CompareTo(Vertex_Lite_UnionFind other)
        {
            return this.Data.CompareTo(other.Data);
        }

        public override string ToString()
        {
            return string.Format("{{0}, {1}, {2}, {3}}", this.Index, this.Leader, this.Data, this.Visited.ToString());
        }
    }
}
