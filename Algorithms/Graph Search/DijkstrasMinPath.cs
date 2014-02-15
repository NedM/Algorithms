using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Algorithms.Types;
using Algorithms.DataStructures;

namespace Algorithms.Graph_Search
{
    public class DijkstrasMinPath<T, U>
        where T : PathVertex<U>, IEquatable<T>
        where U : struct, IComparable, IEquatable<U>
    {
        public DijkstrasMinPath(IGraph<T, U> inputGraph)
        {
            //Validate graph
            foreach (IEdge<U> edge in inputGraph.Edges)
            {
                if (edge.Weight < 0)
                {
                    throw new ArgumentException("Negative edge length detected! Dijkstra's algorithm only works on graphs with non-negative edge lengths!");
                }
            }
            this.Graph = inputGraph;
        }

        protected IGraph<T, U> Graph { get; private set; }

        /// <summary>
        /// Calculates the shortest paths from the given source to each vertex in the input graph
        /// </summary>
        /// <param name="source">The vertex to calculate shortest paths from</param>
        /// <returns>A list of PathVertices filled with the shortest distances from the source to each vertex</returns>
        public List<T> FindMinPathFromSourceToAllVertices(T source)
        {
            List<T> path = new List<T>();
            MinArrayHeap<T> nodeHeap = new MinArrayHeap<T>(this.Graph.Vertices.Count);

            foreach (T vert in this.Graph.Vertices)
            {
                if (vert.Equals(source))
                {
                    vert.Distance = 0;
                }
                else
                {
                    vert.Distance = double.PositiveInfinity;
                }
                vert.Visited = false;
                vert.Predecessor = null;
                nodeHeap.Insert(vert);
            }

            while (nodeHeap != null && nodeHeap.Size > 0)
            {
                nodeHeap.FixHeap();
                T current = nodeHeap.Pop();
                if (current.Distance == double.PositiveInfinity)
                {
                    //Have visited all the nodes in the graph
                    break;
                }

                foreach (T neighbor in current.Neighbors.Values)
                {
                    if (!neighbor.Visited)
                    {
                        double dist = current.Distance + this.GetLowestCostArc(current, neighbor);
                        if (dist < neighbor.Distance)
                        {
                            neighbor.Distance = dist;
                            neighbor.Predecessor = current;
                        }
                    }
                }

                //Mark current node as visited and continue
                current.Visited = true;
                path.Add(current);
            }
            return path;
        }

        private double GetLowestCostArc(T source, T dest)
        {
            double lowestCost = double.PositiveInfinity;
            IEdge<U> arcWithLowestCost = this.Graph.Edges.Where(e => e.Tail.Equals(source) && e.Head.Equals(dest)).OrderBy(e => e.Weight).FirstOrDefault(); 
            if(arcWithLowestCost != null)
            {
                lowestCost = arcWithLowestCost.Weight;
            }
            return lowestCost;
        }
    }
}
