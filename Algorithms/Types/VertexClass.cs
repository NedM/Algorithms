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

    public class VertexBase<T> : 
        IVertex<T>, IComparable, IEquatable<IVertex<T>>, IEquatable<VertexBase<T>> 
        where T : struct, IComparable, IEquatable<T>
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
            bool equal = false;
            if (null != other)
            {
                equal = other.Index.Equals(this.Index);
            }
            return equal;       
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + this.Index;
        }

        public virtual int CompareTo(object obj)
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

        public bool Equals(VertexBase<T> other)
        {
            return this.Equals((IVertex<T>)other);
        }
    }

    public class Vertex<T> : 
        VertexBase<T>, IEquatable<Vertex<T>> 
        where T : struct, IComparable, IEquatable<T>
    {
        public Dictionary<int, Vertex<T>> Neighbors;
        private List<IEdge<T>> arcs;
        public Vertex(int index) : base(index) 
        {
            this.Neighbors = new Dictionary<int, Vertex<T>>();
        }

        public Vertex(IVertex<T> vertex) : this(vertex.Index, vertex.Data) { }
        public Vertex(Vertex<T> vertex) : this(vertex.Index, vertex.Data, vertex.Arcs) { }
        public Vertex(int index, T data) : this(index, data, new List<IEdge<T>>()) { }
        public Vertex(int index, T data, IEnumerable<IEdge<T>> arcs)
            : base(index, data)
        {
            this.arcs = new List<IEdge<T>>();
            this.Neighbors = new Dictionary<int, Vertex<T>>();

            foreach (IEdge<T> arc in arcs)
            {
                this.arcs.Add(arc);
                this.AddNeighbor(new Vertex<T>(arc.Head));
            }
        }

        public System.Collections.ObjectModel.ReadOnlyCollection<IEdge<T>> Arcs
        {
            get { return this.arcs.AsReadOnly(); }
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
                var arcsToNeighbor = this.arcs.Where(e => e.Head.Index.Equals(index));
                foreach (IEdge<T> arc in arcsToNeighbor)
                {
                    this.RemoveArc(arc);
                }
                this.Neighbors.Remove(index);
            }
        }

        public void RemoveAllNeighbors()
        {
            this.Neighbors.Clear();
            this.arcs.Clear();
        }

        public void AddArc(IEdge<T> arc)
        {
            this.arcs.Add(arc);
            this.AddNeighbor(new Vertex<T>(arc.Head));
        }

        private void RemoveArc(IEdge<T> arc)
        {
            if (this.arcs.Contains(arc))
            {
                this.arcs.Remove(arc);
            }
        }

        public int OutDegree
        {
            get { return this.arcs.Count; }
        }

        public virtual Vertex<T> DeepCopy()
        {
            Vertex<T> copy = new Vertex<T>(this.Index, this.Data, this.arcs);
            copy.Visited = this.Visited;
            return copy;
        }

        public virtual Vertex<T> ShallowCopy()
        {
            Vertex<T> copy = new Vertex<T>(this.Index, this.Data);
            return copy;
        }

        public bool Equals(Vertex<T> other)
        {
            return base.Equals(other);
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

    public class PathVertex<T> : 
        Vertex<T>, IEquatable<PathVertex<T>>
        where T : struct, IComparable, IEquatable<T>
    {
        public PathVertex(int index, double distance = double.PositiveInfinity)
            : base(index)
        {
            this.Distance = distance;
        }

        public PathVertex(int index, T data, double distance = double.PositiveInfinity)
            : base(index, data)
        {
            this.Distance = distance;
        }

        public PathVertex(IVertex<T> vertex, double distance = double.PositiveInfinity)
            : this(vertex.Index, vertex.Data, distance)
        {
        }

        public PathVertex(PathVertex<T> vertex)
            : this(vertex, vertex.Distance)
        {
            this.Predecessor = vertex.Predecessor;
        }

        public PathVertex<T> Predecessor { get; set; }
        public double Distance { get; set; }

        public override int CompareTo(object obj)
        {
            if (!(obj is PathVertex<T>))
            {
                throw new ArgumentException(string.Format("Comparison is not defined for types {0} and {1}", this.GetType().ToString(), obj.GetType().ToString()));
            }

            PathVertex<T> vertex = (PathVertex<T>)obj;

            return this.Distance.CompareTo(vertex.Distance);
        }

        public new string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.Index, this.Distance, 
                (this.Predecessor == null ? "NULL" : this.Predecessor.Index.ToString()));
        }

        public bool Equals(PathVertex<T> other)
        {
            bool equal = false;

            if (other != null)
            {
                equal = base.Equals(other) && 
                   this.Distance.Equals(other.Distance);
            }
            return equal;
        }

        public override Vertex<T> DeepCopy()
        {
            PathVertex<T> copy = new PathVertex<T>(this);
            return copy;
        }

        public override Vertex<T> ShallowCopy()
        {
            PathVertex<T> copy = new PathVertex<T>(base.Index, base.Data, this.Distance);
            return copy;
        }
    }

    public class LogicVertex<T> : 
        Vertex<T>, IEquatable<LogicVertex<T>>, IComparable<LogicVertex<T>>
        where T : struct, IComparable, IEquatable<T>
    {
        public LogicVertex(IVertex<T> vertex) : this(vertex.Index, vertex.Data) { }
        public LogicVertex(Vertex<T> vertex) : this(vertex.Index, vertex.Data) { }
        public LogicVertex(int index, T data, bool logic = true) : base(index, data)
        {
            this.Logic = logic;
            base.Visited = false;
        }

        public KeyValuePair<T, bool> LogicData { get { return new KeyValuePair<T, bool>(base.Data, this.Logic); } }

        public bool Logic { get; set; }

        public bool Equals(object obj)
        {
            if (null == obj || !(obj is LogicVertex<T>))
            {
                return false;
            }
            else
            {
                return this.Equals((LogicVertex<T>)obj);
            }
        }

        public bool Equals(LogicVertex<T> other)
        {
            return this.Index.Equals(other.Index) &&
                   this.LogicData.Key.Equals(other.LogicData.Key) &&
                   this.LogicData.Value.Equals(other.LogicData.Value);
        }

        public int CompareTo(LogicVertex<T> other)
        {
            if (this.LogicData.Key.Equals(other.LogicData.Key))
            {
                return this.LogicData.Value.CompareTo(other.LogicData.Value);
            }
            else
            {
                return this.Data.CompareTo(other.Data);
            }
        }

        public override Vertex<T> DeepCopy()
        {
            LogicVertex<T> copy = new LogicVertex<T>(base.Index, base.Data, this.Logic);
            copy.Visited = base.Visited;
            return copy;
        }

        public override Vertex<T> ShallowCopy()
        {
            LogicVertex<T> copy = new LogicVertex<T>(base.Index, base.Data, this.Logic);
            return copy;
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}{2}]", this.Index, this.LogicData.Value ? string.Empty : "!", this.LogicData.Key); 
        }

        public bool IsLogicalCompliment(LogicVertex<T> other)
        {
            if (!(this.LogicData.Key.Equals(other.LogicData.Key)))
            {
                return false;
            }
            else
            {
                //Return this XOR other
                return this.LogicData.Value ^ other.LogicData.Value;
            }
        }
        
        public static bool CollectionContainsContradiction(ICollection<LogicVertex<T>> collection)
        {
            bool contradictionFound = false;
            Dictionary<T, bool> hashTable = new Dictionary<T, bool>();

            foreach (LogicVertex<T> vertex in collection)
            {
                if (hashTable.ContainsKey(vertex.Data))
                {
                    if (hashTable[vertex.Data] ^ vertex.Logic)
                    {
                        contradictionFound = true;
                        break;
                    }
                }
                else
                {
                    hashTable.Add(vertex.Data, vertex.Logic);
                }
            }
            return contradictionFound;
        }
    }
}
