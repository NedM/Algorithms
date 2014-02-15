using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public interface IGraph<T> where T : struct
    {
        IEnumerable<IVertex<T>> Vertices { get; set; }
        IEnumerable<IEdge<T>> Edges { get; set; }

        void AddVertex(IVertex<T> vertexToAdd);
        void RemoveVertex(IVertex<T> vertexToRemove);

        void AddDirectedEdge(int indexFrom, int indexTo, double cost = 0);
        void AddDirectedEdge(IVertex<T> from, IVertex<T> to, double cost = 0);

        void AddUndirectedEdge(int indexA, int indexB, double cost = 0);
        void AddUndirectedEdge(IVertex<T> A, IVertex<T> B, double cost = 0);

        void RemoveEdge(IEdge<T> edgeToRemove);
    }
}
