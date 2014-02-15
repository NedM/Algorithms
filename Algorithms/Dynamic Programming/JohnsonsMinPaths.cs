#undef DEBUG
using Algorithms.DataStructures;
using Algorithms.Graph_Search;
using Algorithms.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Algorithms.Dynamic_Programming
{
    public class JohnsonsMinPaths<T, U> 
        where T : PathVertex<U>, IEquatable<T>
        where U : struct, IComparable, IEquatable<U>
    {
        public JohnsonsMinPaths(IGraph<T, U> inputGraph)
        {
            this.Graph = inputGraph;
        }

        protected IGraph<T, U> Graph { get; private set; }

        public bool GraphHasNegativeWeightEdges
        {
            get
            {
                bool negativeWeightEdgeDetected = false;
                foreach (IEdge<U> e in this.Graph.Edges)
                {
                    if (e.Weight < 0)
                    {
                        negativeWeightEdgeDetected = true;
                        break;
                    }
                }
                return negativeWeightEdgeDetected;
            }
        }

        public T[,] FindMinPathFromAllSourcesToAllVertices()
        {
            if (null == this.Graph)
            {
                throw new Exception("Input graph cannot be null! Please construct a valid object before calling this method.");
            }
            List<T> BFminPaths;
#if DEBUG
            Console.WriteLine(string.Format("Beginning Bellman-Ford operations"));
#endif
            if (this.GraphHasNegativeWeightEdges)
            {
                BFminPaths = this.DoBellmanFordToCalculateEdgeAdjustments();
            }
            else
            {
                BellmanFordMinPath<T, U> bellmanFord = new BellmanFordMinPath<T, U>(this.Graph);
                BFminPaths = bellmanFord.FindMinPathFromSourceToAllVertices(this.Graph.Vertices[0]);
            }

#if DEBUG
            Console.WriteLine(string.Format("Done doing Bellman-Ford operations"));
#endif

            if (null == BFminPaths)
            {
                //Negative weight cost cycle detected!
                return null;
            }
#if DEBUG
            Console.WriteLine(string.Format("Beginning Dijkstra operations"));
#endif
            T[,] minPaths = this.DoDijkstrasForEachSourceVertex(BFminPaths);
#if DEBUG
            Console.WriteLine(string.Format("Done doing Dijkstra operations"));
#endif
            return minPaths;
        }

        /// <summary>
        /// Warning! This method mutates the Graph!
        /// </summary>
        /// <returns>Null is returned if negative weight cost cycle is found</returns>
        private List<T> DoBellmanFordToCalculateEdgeAdjustments()
        {
            //Add new dummy vertex with zero weight edges to each other vertex
            T newDummySourceVertex = this.AddVertexWithZeroWeightEdges();

            BellmanFordMinPath<T, U> bellmanFord = new BellmanFordMinPath<T, U>(this.Graph);
            List<T> BFminPaths = bellmanFord.FindMinPathFromSourceToAllVertices(newDummySourceVertex);

            if (null != BFminPaths)
            {
                //Remove dummy vertex and all edges added
                this.Graph.RemoveVertex(newDummySourceVertex);

                //Fix edges so that none are negative weight
                this.ReWeightEdges_NoNegativeWeights(BFminPaths);
#if DEBUG
                if (this.GraphHasNegativeWeightEdges)
                {
                    throw new Exception("Graph still has negative edge lengths");
                }
#endif
            }

            return BFminPaths;
        }

        private T[,] DoDijkstrasForEachSourceVertex(List<T> bellmanFordCalculatedShortestPaths)
        {
            int vertexCount = this.Graph.Vertices.Count;
            T[,] minPaths = new T[vertexCount, vertexCount];

            DijkstrasMinPath<T, U> dijkstra = new DijkstrasMinPath<T, U>(this.Graph);

            for (int i = 0; i < vertexCount; i++)
            {
                T source = bellmanFordCalculatedShortestPaths.Where(v => v.Index.Equals(this.Graph.Vertices[i].Index)).FirstOrDefault();

                List<T> DminPathsFromSource = dijkstra.FindMinPathFromSourceToAllVertices(this.Graph.Vertices[i]);
                for (int j = 0; j < vertexCount; j++)
                {
                    T dest = bellmanFordCalculatedShortestPaths.Where(v => v.Index.Equals(this.Graph.Vertices[j].Index)).FirstOrDefault();

                    if (DminPathsFromSource.Contains((IVertex<U>)dest))
                    {
                        T vert = DminPathsFromSource.Where(v => v.Index.Equals(dest.Index)).FirstOrDefault();

                        double adjustedDistance = vert.Distance - source.Distance + dest.Distance;
                                               
                        minPaths[i, j] = (T)new PathVertex<U>(dest, adjustedDistance);
                        minPaths[i, j].Predecessor = (vert.Predecessor == null) ? null : minPaths[i, vert.Predecessor.Index - 1];  //int indexOfPredecessor = this.Graph.Vertices.IndexOf((T)vert.Predecessor);
                    }
                    else
                    {
                        minPaths[i, j] = (T)new PathVertex<U>(dest, double.PositiveInfinity);
                    }
                }
            }
            return minPaths;
        }

        private void ReWeightEdges_NoNegativeWeights(IList<T> minPathsToEachVertex)
        {
            foreach (IEdge<U> edge in this.Graph.Edges)
            {
                T tailVertex = minPathsToEachVertex.Where(v => v.Index.Equals(edge.Tail.Index)).FirstOrDefault();
                T headVertex = minPathsToEachVertex.Where(v => v.Index.Equals(edge.Head.Index)).FirstOrDefault();

                if (null == tailVertex)
                {
                    throw new Exception(string.Format("Failed to find vertex {0} in the collection of vertices!", edge.Tail.ToString()));
                }

                if (null == headVertex)
                {
                    throw new Exception(string.Format("Failed to find vertex {0} in the collection of vertices!", edge.Head.ToString()));
                }

#if DEBUG
                Console.WriteLine(string.Format("Old edge weight = {0}", edge.Weight));
#endif
                //Calculate new non-negative edge weight
                edge.Weight = edge.Weight + tailVertex.Distance - headVertex.Distance;

#if DEBUG
                Console.WriteLine(string.Format("New edge weight = {0}", edge.Weight));
#endif
                if (edge.Weight < 0)
                {
                    throw new Exception("Edge weight still negative!");
                }
            }
        }

        private T AddVertexWithZeroWeightEdges()
        {
            int unUsedIndex = this.FindUnusedIndex();
            PathVertex<U> vertexToAdd = new PathVertex<U>(unUsedIndex);
            this.Graph.AddVertex((T)vertexToAdd);
            foreach (T v in this.Graph.Vertices)
            {
                this.Graph.AddDirectedEdge((T)vertexToAdd, v, 0);
            }

            return this.Graph.GetVertex(unUsedIndex);
        }

        private int FindUnusedIndex()
        {
            //Assume the vertices are indexed by their location in the Vertex list
            //This is not necessarily the case so we must check.
            int unUsed = this.Graph.Vertices.Count;
            while (this.Graph.HasVertex(unUsed))
            {
                unUsed++;
            }
            return unUsed;
        }
    }
}
