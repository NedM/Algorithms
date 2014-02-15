#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;

namespace Algorithms.Dynamic_Programming
{
    public class TravellingSalesmanProblem<T, U>
        where T : PathVertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        public TravellingSalesmanProblem(IGraph<T, U> inputGraph)
        {
            //Do some validations?? What are the requirements?
            this.Graph = inputGraph;
            this.BitMasks = new Dictionary<int, List<int>>();

            //Pre fill bit masks
            int numVertices = this.Graph.Vertices.Count;
            for (int i = 0; i <= numVertices; i++)
            {
                this.BitMasks.Add(i, this.GospersHack(i, numVertices));
            }
        }

        protected IGraph<T, U> Graph { get; private set; }
        protected Dictionary<int, List<int>> BitMasks;

        public double FindShortestRoute(ref T source)
        {
            if(null == this.Graph)
            {
                throw new Exception("Graph is null! Please construct a valid object before calling this method!");
            }

            if(this.Graph.Vertices.Count <= 0)
            {
                throw new Exception("Graph has zero vertices! Cannot calculate the shortest route on a graph with no nodes!");
            }

            int numNodes = this.Graph.Vertices.Count;
            int indexOfSource = this.Graph.Vertices.IndexOf(source);
            int sourceMask = this.GenerateBitMask(indexOfSource, numNodes);
            int maxSubSet = this.BitMasks[numNodes].First(); 
            double numSubsets = this.GenerateSubsetListThatContainsSource(sourceMask, numNodes);            
            Dictionary<int, double[]> subProblemValues = new Dictionary<int, double[]>((int)numSubsets);
                        
            //pre-compute step
            foreach (List<int> values in this.BitMasks.Values)
            {
                foreach (int S in values)
                {
                    subProblemValues.Add(S, null);
                    if (S == sourceMask)
                    {
                        subProblemValues[S] = new double[numNodes];
                        for (int i = 0; i < numNodes; i++)
                        {
                            if (i == indexOfSource)
                            {
                                subProblemValues[S][indexOfSource] = 0;
                            }
                            else
                            {
                                subProblemValues[S][i] = double.PositiveInfinity;
                            }
                        }
                    }
                    //else
                    //{
                    //    subProblemValues[S] = new double[numNodes];
                    //    subProblemValues[S][indexOfSource] = double.PositiveInfinity;
                    //}
                }
            }

            //m represents the cardinality of the sub set size (i.e. how many nodes are used in the sub set (here represented by set bits))
            for (int m = 2; m <= numNodes; m++)
            {
                //Filter the list of sub sets to include only those with m vertices visited
                List<int> filteredSubSets = this.BitMasks[m];//this.FilterSubSetListByNumBitsSet(subSets, m);
                foreach (int subSet in filteredSubSets)
                {
                    //Get the vertices in the sub set
                    List<T> listOfVerticesInSubset = this.GetListOfVerticesInSubset(subSet, numNodes);
                    foreach (T vertex in listOfVerticesInSubset)
                    {
                        if (!vertex.Equals(source))
                        {
                            int indexOfVertex = this.Graph.Vertices.IndexOf(vertex);
                            int vertexMask = this.GenerateBitMask(indexOfVertex, numNodes);
                            int subSetMinusIthVertexMask = (subSet ^ vertexMask);  //XOR the vertex mask with subset mask to get the subset minus the vertex
#if DEBUG
                            Console.WriteLine(string.Format("{0} XOR {1} is {2}",
                                Convert.ToString(subSet, 2).PadLeft(numNodes, '0'),
                                Convert.ToString(vertexMask, 2).PadLeft(numNodes, '0'),
                                Convert.ToString(subSetMinusIthVertexMask, 2).PadLeft(numNodes, '0')));

                            if ((subSetMinusIthVertexMask ^ vertexMask) != subSet)
                            {
                                throw new Exception(string.Format("Something went wrong! {0} XOR {1} should be {2}",
                                    Convert.ToString(subSetMinusIthVertexMask, 2).PadLeft(numNodes, '0'),
                                    Convert.ToString(vertexMask, 2).PadLeft(numNodes, '0'),
                                    Convert.ToString(subSet, 2).PadLeft(numNodes, '0')));
                            }
#endif

                            int k;
                            double lowestCostLastHop = this.GetLowestCostEdgeToDest(subSetMinusIthVertexMask, indexOfVertex, numNodes, out k);
                            if (subProblemValues[subSet] == null)
                            {
                                subProblemValues[subSet] = new double[numNodes];
                            }
                            subProblemValues[subSet][indexOfVertex] = subProblemValues[subSetMinusIthVertexMask][k] + lowestCostLastHop;

                            //Fill in path
                            T vertexInGraph = this.Graph.Vertices[indexOfVertex];
                            vertexInGraph.Distance = subProblemValues[subSet][indexOfVertex];
                            vertexInGraph.Predecessor = this.Graph.Vertices[k];
                        }
                    }

                }
            }
            int penultimate;
            double lowestCostFinalHop = this.GetLowestCostEdgeToDest(maxSubSet, indexOfSource, numNodes, out penultimate);
            double returnToSourceCost = subProblemValues[maxSubSet][penultimate] + lowestCostFinalHop;
            source.Distance = returnToSourceCost;
            source.Predecessor = this.Graph.Vertices[penultimate];
            return returnToSourceCost;
        }

        private double GetLowestCostEdgeToDest(int maskOfAvailablePredecessors, int indexOfTarget, int numNodes, out int indexOfPredecessor)
        {
            indexOfPredecessor = -1;
            double minCost = double.PositiveInfinity;
            List<T> predecessorVertices = this.GetListOfVerticesInSubset(maskOfAvailablePredecessors, numNodes);
            foreach (T node in predecessorVertices)
            {
                foreach (IEdge<U> arc in node.Arcs)
                {
                    if (arc.Head.Index.Equals(indexOfTarget) && arc.Weight < minCost)
                    {
                        minCost = arc.Weight;
                        indexOfPredecessor = arc.Tail.Index;
                    }
                }
            }
            return minCost;
        }

        private List<T> GetListOfVerticesInSubset(int mask, int numVertices)
        {
#if DEBUG
            Console.WriteLine(string.Format("Vertex mask: {0}", Convert.ToString(mask, 2).PadLeft(numVertices, '0')));
#endif
            List<T> verticesInSubset = new List<T>();
            for (int i = 0; i < numVertices; i++)
            {
                int bit = (int)Math.Pow(2, i);
                if ((bit & mask) == bit)
                {
                    T vertexInSubset = this.Graph.GetVertexAt(i);
#if DEBUG
                    Console.WriteLine(string.Format("Adding vertex {0}", ((VertexBase<U>)vertexInSubset).ToString()));
#endif
                    verticesInSubset.Add(vertexInSubset);
                }
            }
            return verticesInSubset;
        }

        //Generate subsets 
        private double GenerateSubsetListThatContainsSource(int sourceMask, int numVertices)
        {
            double countOfSubSets = 0;

            //Bitmask where bit i is 1 if the node at that index has been visited and 0 otherwise
            //i represents the number of set bits available
            for (int n = 0; n <= numVertices; n++)
            {
                this.BitMasks[n].RemoveAll(set => ((set & sourceMask) != sourceMask));
                countOfSubSets += this.BitMasks[n].Count;
            }

            return countOfSubSets;
        }

        private int GenerateBitMask(int indexOfVertex, int numVertices)
        {
            int rInt = 0;
            if (indexOfVertex > numVertices || indexOfVertex < 0)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < numVertices; i++)
            {
                if (i == indexOfVertex)
                {
                    rInt = (int)Math.Pow(2, i);
                    break;
                }
            }
#if DEBUG
            Console.WriteLine(string.Format("Bit mask: {0}", Convert.ToString(rInt, 2).PadLeft(numVertices, '0')));
#endif

            return rInt;
        }

        /// <summary>
        /// Gosper's hack found at http://www.cl.cam.ac.uk/~am21/hakmemc.html and http://programmers.stackexchange.com/a/67087
        /// </summary>
        /// <param name="numBitsAvailableForSet"></param>
        /// <param name="numBitsTotal"></param>
        /// <returns></returns>
        private List<int> GospersHack(int numBitsToSet, int numBitsTotal)
        {
            List<int> setsWithNBitsSet = new List<int>();

#if DEBUG
            Console.WriteLine(string.Format("Selecting combinations of {0} bits from {1} total", numBitsToSet, numBitsTotal));
#endif

            if (numBitsToSet <= 0)
            {
                return setsWithNBitsSet;
            }
                        
            int set = (1 << numBitsToSet) - 1;
            int limit = (1 << numBitsTotal);

            while (set < limit)
            {
#if DEBUG
                Console.WriteLine(string.Format("Adding {0} to set", Convert.ToString(set, 2).PadLeft(numBitsTotal, '0')));
#endif
                setsWithNBitsSet.Add(set);

                // Gosper's hack:
                int c = set & -set;
                int r = set + c;
                set = (((r ^ set) >> 2) / c) | r;
            }

            return setsWithNBitsSet;
        }

        private List<int> FilterSubSetListByNumBitsSet(List<int> allSubSets, int numBitsSetFilter)
        {
            List<int> filteredList = new List<int>();
            foreach (int bitMask in allSubSets)
            {
                if (this.CountSetBits(bitMask) == numBitsSetFilter)
                {
                    filteredList.Add(bitMask);
                }
            }
            return filteredList;
        }

        private int CountSetBits(int numToCountSetBitsFor)
        {
            int shifting = numToCountSetBitsFor;
            int count = 0;
            for (count = 0; shifting >= 1; shifting >>= 1)
            {
                count += shifting & 1;
            }

#if DEBUG
            Console.WriteLine(string.Format("{0} bits set in {1}", count, Convert.ToString(numToCountSetBitsFor, 2)));
#endif 
            return count;
        }
    }
}
