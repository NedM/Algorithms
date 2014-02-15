using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class Graph_AdjacencyMatrix<T> : GraphBaseClass<T> where T : struct, IComparable, IEquatable<T>, IVertex<T>
    {
        private readonly int numberOfVerticies;
        public int[,] EdgeArray;

        public Graph_AdjacencyMatrix(int numVerticies) : base(numVerticies)
        {
            this.EdgeArray = new int[numVerticies, numVerticies];
            this.numberOfVerticies = numVerticies;
        }

        public void AddDirectedEdge(int indexOfTail, int indexOfHead)
        {
            this.AddDirectedEdge(indexOfTail, indexOfHead, 1);
        }
        
        public void AddDirectedEdge(int indexOfTail, int indexOfHead, int cost)
        {
            this.SetEdgeValue(indexOfTail, indexOfHead, cost);
        }

        public void RemoveDirectedEdge(int indexOfTail, int indexOfHead)
        {
            this.SetEdgeValue(indexOfTail, indexOfHead, 0);
        }

        public void AddUndirectedEdge(int indexOfTail, int indexOfHead)
        {
            this.AddDirectedEdge(indexOfTail, indexOfHead);
            this.AddDirectedEdge(indexOfHead, indexOfTail);
        }

        public void AddUndirectedEdge(int indexOfTail, int indexOfHead, int cost)
        {
            this.AddDirectedEdge(indexOfTail, indexOfHead, cost);
            this.AddDirectedEdge(indexOfHead, indexOfTail, cost);
        }

        public void RemoveUndirectedEdge(int indexOfTail, int indexOfHead)
        {
            this.RemoveDirectedEdge(indexOfTail, indexOfHead);
            this.RemoveDirectedEdge(indexOfHead, indexOfTail);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            for (int k = 0; k < this.numberOfVerticies; k++)
            {
                sb.AppendFormat(" {0}", k);
            }
            sb.Append(Environment.NewLine);

            for (int i = 0; i < this.numberOfVerticies; i++)
            {
                sb.AppendFormat("{0} ", i);
                for (int j = 0; j < this.numberOfVerticies; j++)
                {                    
                    sb.AppendFormat("{0} ", this.EdgeArray[i, j]);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private void SetEdgeValue(int indexOfTail, int indexOfHead, int value)
        {
            if (indexOfTail > this.numberOfVerticies || indexOfHead > this.numberOfVerticies)
            {
                throw new ArgumentException("Index out of range! Cannot specify an index greater than " + this.numberOfVerticies);
            }

            this.EdgeArray[indexOfTail, indexOfHead] = value;
        }
    }
}
