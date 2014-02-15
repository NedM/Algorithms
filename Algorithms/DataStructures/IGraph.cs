using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public interface IGraph<T, U> 
        where T : IVertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        IList<T> Vertices { get; }
        IList<IEdge<U>> Edges { get; }

        IGraph<T, U> DeepCopy();

        bool HasVertex(int index);
        T GetVertex(int index);
        T GetVertexAt(int location);

        void AddVertex(T vertexToAdd);
        void RemoveVertex(T vertexToRemove);

        void AddDirectedEdge(int indexFrom, int indexTo, double cost = 0);
        void AddDirectedEdge(T from, T to, double cost = 0);

        void AddUndirectedEdge(int indexA, int indexB, double cost = 0);
        void AddUndirectedEdge(T A, T B, double cost = 0);

        void RemoveEdge(IEdge<U> edgeToRemove);
    }
}
