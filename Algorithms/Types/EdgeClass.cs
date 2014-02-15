using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public interface IEdge<T> where T : struct
    {
        IVertex<T> Head { get; set; }
        IVertex<T> Tail { get; set; }
        double Weight { get; set; }
    }

    public class EdgeBase<T> : IEdge<T>, IComparable, IEquatable<IEdge<T>> where T : struct, IComparable, IEquatable<T>
    {
        public EdgeBase() : this(new VertexBase<T>(0), new VertexBase<T>(0)) { }
        public EdgeBase(VertexBase<T> from, VertexBase<T> to, double weight = 0)
        {
            this.Tail = from;
            this.Head = to;
            this.Weight = weight;
        }

        public IVertex<T> Tail { get; set; }
        public IVertex<T> Head { get; set; }
        public double Weight { get; set; }

        public bool OriginatesFrom(IVertex<T> vertex)
        {
            if (this.Tail.Equals(vertex))
                return true;
            else
                return false;
        }

        public bool TerminatesAt(IVertex<T> vertex)
        {
            if (this.Head.Equals(vertex))
                return true;
            else
                return false;
        }

        public override bool Equals(object obj)
        {
            bool bReturn = false;
            if (!(obj is EdgeBase<T>))
            {
                return false;
            }

            bReturn = this.Equals((IEdge<T>)obj);

            return bReturn;
        }

        public bool Equals(IEdge<T> other)
        {
            if (null == other)
            {
                return false;
            }

            bool bReturn = other.Head.Equals(this.Head) &&
                           other.Tail.Equals(this.Tail) &&
                           (other.Weight == this.Weight);

            return bReturn;
        }

        public override int GetHashCode()
        {
            return (int)this.Weight + base.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (!(obj is EdgeBase<T>))
            {
                throw new ArgumentException(string.Format("Comparison is not defined for types {0} and {1}", this.GetType().ToString(), obj.GetType().ToString()));
            }

            EdgeBase<T> e = (EdgeBase<T>)obj;
            return this.Weight.CompareTo(e.Weight);
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", this.Tail.Index, this.Head.Index, this.Weight);
        }
    }

    public class Edge<T> : EdgeBase<T> where T : struct, IComparable, IEquatable<T>
    {
        public Edge(int fromIndex, int toIndex, double cost = 0) : this(new Vertex<T>(fromIndex), new Vertex<T>(toIndex), cost) { }
        public Edge(Edge<T> edge) : this(edge.Tail, edge.Head, edge.Weight) { }
        public Edge(Vertex<T> from, Vertex<T> to, double cost = 0) : base(from, to, cost)
        {
            this.Tail = from;
            this.Head = to;
            base.Weight = cost;
        }

        public new Vertex<T> Tail { get; set; }
        public new Vertex<T> Head { get; set; }

        public Edge<T> Reverse()
        {
            return new Edge<T>(this.Head, this.Tail, this.Weight);
        }

        public override bool Equals(object obj)
        {
            bool bReturn = false;
            if (!(obj is EdgeBase<T>))
            {
                return false;
            }

            if (!(obj is Edge<T>))
            {
                return base.Equals(obj);
            }

            bReturn = this.Equals((Edge<T>)obj);

            return bReturn;
        }

        public bool Equals(Edge<T> other)
        {
            if (null == other)
            {
                return false;
            }

            bool bReturn = other.Head.Equals(this.Head) &&
                           other.Tail.Equals(this.Tail) &&
                           (other.Weight == this.Weight);

            return bReturn;
        }

        public override int GetHashCode()
        {
            return (int)this.Weight + base.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}, {1}, {2}]", this.Tail.Index, this.Head.Index, this.Weight);
        }
    }

    public class Edge_UnionFind<T> : Edge<T> where T : struct, IComparable, IEquatable<T>
    {
        public Edge_UnionFind(int fromIndex, int toIndex, double cost = 0) : this(new Vertex_UnionFind<T>(fromIndex), new Vertex_UnionFind<T>(toIndex), cost) { }
        public Edge_UnionFind(Vertex_UnionFind<T> from, Vertex_UnionFind<T> to, double cost = 0) : base(from, to, cost)
        {
            this.Tail = from;
            this.Head = to;
            base.Weight = cost;
        }

        public new Vertex_UnionFind<T> Head { get; set; }
        public new Vertex_UnionFind<T> Tail { get; set; }

        public override bool Equals(object obj)
        {
            bool bReturn = false;
            if (!(obj is Edge<T>))
            {
                return false;
            }

            if (!(obj is Edge_UnionFind<T>))
            {
                return base.Equals(obj);
            }

            bReturn = this.Equals((Edge_UnionFind<T>)obj);

            return bReturn;
        }

        public bool Equals(Edge_UnionFind<T> other)
        {
            if (null == other)
            {
                return false;
            }

            bool bReturn = other.Head.Equals(this.Head) &&
                           other.Tail.Equals(this.Tail) &&
                           (other.Weight == this.Weight);

            return bReturn;
        }

        public override int GetHashCode()
        {
            return (int)base.Weight + base.GetHashCode();
        }


    }
}
