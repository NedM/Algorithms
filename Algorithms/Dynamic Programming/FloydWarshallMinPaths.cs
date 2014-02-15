using System;
using System.Collections.Generic;
using System.Text;
using Algorithms.Types;
using Algorithms.DataStructures;

using System.Linq;

namespace Algorithms.Dynamic_Programming
{
    public class FloydWarshallMinPaths<T, U>
        where T : PathVertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        public FloydWarshallMinPaths(IGraph<T, U> inputGraph)
        {
            this.Graph = inputGraph;
        }

        protected IGraph<T, U> Graph { get; private set; }

        public T[,] FindMinPathFromAllSourcesToAllVertices()
        {
            if (this.Graph == null)
            {
                throw new Exception("The Graph is null! Please construct a valid object before calling this method.");
            }

            if (this.Graph.Vertices.Count < 1)
            {
                throw new ArgumentException("Input graph does not have any vertices! Cannot compute min path");
            }

            int numVertices = this.Graph.Vertices.Count;
            double[,] subProblemValues = new double[numVertices, numVertices];
            T[,] rVertexMatrix = new T[numVertices, numVertices];            

            for (int k = 0; k < numVertices; k++)
            {
                for (int i = 0; i < numVertices; i++)
                {
                    for (int j = 0; j < numVertices; j++)
                    {
                        T sourceVertex = this.Graph.GetVertexAt(i);
                        T destVertex = this.Graph.GetVertexAt(j);
                        if (0 == k)
                        {
                            if (i == j)
                            {
                                //The distance from any vertex to itself if 0
                                subProblemValues[i, j] = 0;
                            }
                            else
                            {
                                double ijEdgeWeight;
                                if (this.EdgeExists(sourceVertex, destVertex, out ijEdgeWeight))
                                {
                                    //The distance from any vertex to any other directly connected vertex is the lowest cost edge weight
                                    subProblemValues[i, j] = ijEdgeWeight;

                                    //Fill in data for path reconstruction
                                    rVertexMatrix[i, j] = (T)new PathVertex<U>(destVertex, ijEdgeWeight);
                                    rVertexMatrix[i, j].Predecessor = sourceVertex;
                                }
                                else
                                {
                                    //Initially, since we have an intervening vertex budget of 0, the distance from any vertex to any other not directly connected vertex is +infinity
                                    subProblemValues[i, j] = double.PositiveInfinity;
                                }
                            }
                        }
                        else
                        {
                            double case1 = subProblemValues[i, j];
                            double case2 = subProblemValues[i, k] + subProblemValues[k, j];
                            double min = Math.Min(case1, case2);
                            subProblemValues[i, j] = min;

                            //Check for negative cost cycles
                            if (i == j && subProblemValues[i, j] < 0)
                            {
                                //negative cost cycle detected! Terminating execution!
                                return null;
                            }

                            if (null == rVertexMatrix[i, j])
                            {
                                rVertexMatrix[i, j] = (T)(new PathVertex<U>(destVertex, min));
                            }

                            //Reconstruct the path
                            if (min == case2 && min != case1)
                            {
                                rVertexMatrix[i, j].Distance = min;
                                rVertexMatrix[i, j].Predecessor = rVertexMatrix[i, k]; //new PathVertex<U>(this.Graph.GetVertexAt(k), subProblemValues[i, k]);
                            }
                        }
                    }
                }
            }

            return rVertexMatrix;
        }

        private bool EdgeExists(T sourceVertex, T destVertex, out double edgeWeight)
        {
            bool edgeExists = false;
            edgeWeight = double.PositiveInfinity;
            if (sourceVertex.Neighbors.ContainsKey(destVertex.Index))
            {
                edgeExists = true;
                //Use linq?
                IEdge<U> lowestCostEdge = this.Graph.Edges.Where(edge => (edge.Tail.Index.Equals(sourceVertex.Index) && edge.Head.Index.Equals(destVertex.Index))).OrderBy(e => e.Weight).FirstOrDefault();
                edgeWeight = lowestCostEdge.Weight;
            }
            return edgeExists;    
        }
    }
}
