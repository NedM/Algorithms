using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class GraphBaseClass<T> where T : struct, IComparable, IEquatable<T>
    {
        private List<VertexBase<T>> vertices;
        private List<EdgeBase<T>> edges;

        public GraphBaseClass(int numberOfVerticies = 0, int numberOfEdges = 0)
        {
            if (numberOfVerticies > 0)
            {
                this.vertices = new List<VertexBase<T>>(numberOfVerticies);
            }
            else
            {
                this.vertices = new List<VertexBase<T>>();
            }

            if (numberOfEdges > 0)
            {
                this.edges = new List<EdgeBase<T>>(numberOfEdges);
            }
            else
            {
                this.edges = new List<EdgeBase<T>>();
            }
        }

        public List<IVertex<T>> Vertices
        {
            get { return new List<IVertex<T>>(this.vertices); }
        }

        public List<IEdge<T>> Edges
        {
            get { return new List<IEdge<T>>(this.edges); }
        }
    }
}
