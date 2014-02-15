using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Algorithms.DataStructures;
using Algorithms.Types;

namespace Algorithms.Dynamic_Programming
{
    public class BellmanFordMinPath<T, U>
        where T : PathVertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        public BellmanFordMinPath(IGraph<T, U> inputGraph)
        {
            if (null == inputGraph)
            {
                throw new ArgumentNullException("inputGraph", "The inputGraph cannot be null! Please pass a non-null graph object.");
            }
            this.Graph = inputGraph;
        }

        protected IGraph<T, U> Graph { get; private set; }

        public List<T> FindMinPathFromSourceToAllVertices(T source)
        {
            if (this.Graph == null)
            {
                throw new Exception("The Graph is null! Please construct a valid object before calling this method.");
            }

            if(source == null)
            {
                throw new ArgumentNullException("Source vertex cannot be null!");
            }

            if(this.Graph.Vertices.Count < 1)
            {
                throw new ArgumentException("Input graph does not have any vertices! Cannot compute min path");
            }

            List<T> verticesWithMinDistances = new List<T>();
            int numVertices = this.Graph.Vertices.Count;
            T[,] subProblemValues = new T[numVertices, numVertices];

            //i represents our edge budget for restricting the sub-problem size. 
            //To reach each of n vertices you will need a maximum of n-1 edges.
            //If using n edges to reach n vertices costs less than using n-1 edges, then we have a negative weight cycle somewhere
            for (int i = 0; i < numVertices; i++)
            {
                //v represents the index of the destination vertex
                for (int j = 0; j < numVertices; j++)
                {
                    T currentVertex = this.Graph.Vertices[j];

                    if (0 == i)
                    {
                        //If i = 0 then we have no edge budget to reach other vertices so either...
                        if (currentVertex.Equals(source))
                        {
                            //...The target is the same as the source and the value is 0 or...
                            subProblemValues[i, j] = (T)(new PathVertex<U>(currentVertex, 0));
                        }
                        else
                        {
                            //...The target cannot be reached from the source and the cost of this path is +infinity
                            subProblemValues[i, j] = (T)(new PathVertex<U>(currentVertex, double.PositiveInfinity));
                        }
                        subProblemValues[i, j].Predecessor = null;
                    }
                    else
                    {
                        double case1, case2;
                        int predecessorIndex;
                        case1 = subProblemValues[i - 1, j].Distance;
                        case2 = this.FindMinCostPathGivenSubProblemValues(currentVertex, subProblemValues, i - 1, out predecessorIndex);
                        double min = Math.Min(case1, case2);
                        subProblemValues[i, j] = (T)(new PathVertex<U>(currentVertex, min));
                        if (min == case2 && min != case1)
                        {
                            subProblemValues[i, j].Predecessor = subProblemValues[i - 1, predecessorIndex];
                        }
                        else
                        {
                            subProblemValues[i, j].Predecessor = subProblemValues[i - 1, j].Predecessor;
                        }
                    }
                }
#if DEBUG
                //if (i % 10 == 0)
                //{
                    Console.WriteLine(string.Format("Done processing loop {0} of {1}", i + 1, numVertices));
                //}
#endif
            }

            //Check for negative weight cycles
            for (int k = 0; k < numVertices; k++)
            {
                if (subProblemValues[numVertices - 2, k].Distance > subProblemValues[numVertices - 1, k].Distance)
                {
                    //Negative weight cycle detected!
                    //Set return value and break!
                    verticesWithMinDistances = null;
                    break;
                }
                else
                {
                    PathVertex<U> vertexToAdd = new PathVertex<U>(subProblemValues[numVertices - 2, k]);
                    verticesWithMinDistances.Add((T)vertexToAdd);
                }
            }
            //Return null if negative weight cycles are detected, otherwise return the array of min path costs
            return verticesWithMinDistances;
        }

        private List<Edge<U>> FindAllIncidentEdges(T vertexToFindIncidentEdgesFor)
        {
            List<Edge<U>> incidentEdges = new List<Edge<U>>();
            foreach (Edge<U> edge in this.Graph.Edges)
            {
                if (edge.Head.Equals(vertexToFindIncidentEdgesFor))
                {
                    Edge<U> incidentEdge = new Edge<U>(edge);
                    incidentEdges.Add(incidentEdge);
                }
            }
            return incidentEdges;
        }

        private double FindMinCostPathGivenSubProblemValues(T vertexToFindIncidentEdgesFor, T[,] subProblems, int edgeBudget, out int predecessorIndex)
        {
            //This method is terribly inefficient. Need to clean it up so we're not searching over all the edges in the graph each time (if possible)

            double lowestCostPathDistance = double.PositiveInfinity;
            predecessorIndex = -1;
            //List<Edge<U>> incidentEdgesOld = this.FindAllIncidentEdges(vertexToFindIncidentEdgesFor);
            IEnumerable<IEdge<U>> incidentEdges = this.Graph.Edges.Where(e => e.Head.Equals(vertexToFindIncidentEdgesFor));
            
            foreach (IEdge<U> edge in incidentEdges)
            {
                T vertW = (T)(edge.Tail);
                int w = this.Graph.Vertices.IndexOf(vertW);
                double lowCostPath = subProblems[edgeBudget, w].Distance + edge.Weight;
                if (lowCostPath < lowestCostPathDistance)
                {
                    lowestCostPathDistance = lowCostPath;
                    predecessorIndex = w;
                }
            }

            return lowestCostPathDistance;
        }
    }
}
