//#define DEBUG

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;
using Algorithms.DataStructures;

namespace Algorithms
{
    public class RandomizedContraction
    {
        private Graph_AdjacencyList<Vertex<int>, int> graph;

        public RandomizedContraction(Graph_AdjacencyList<Vertex<int>, int> graph)
        {
            this.graph = graph;
        }

        public Graph_AdjacencyList<Vertex<int>, int> Graph
        {
            get { return this.graph; }
        }

        private Edge<int> PickRandomEdge()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int index = rand.Next(0, this.graph.Edges.Count);
            return this.graph.Edges[index];
        }

        /// <summary>
        /// Merge two nodes at the ends of the edge passed in, removing the edge(s) between
        /// And repoint the edges originating from or incident on the removed edge to the remaining edge
        /// </summary>
        /// <param name="edge">The edge that defines the vertices to merge</param>
        private void MergeNodes(Edge<int> edge)
        {
            Vertex<int> nodeToMerge = edge.Tail;
            Vertex<int> nodeToMergeInto = edge.Head;

            List<Edge<int>> edgesToBeRemoved = new List<Edge<int>>();

            //Repoint all edges originating from nodeToMerge to nodeToMergeInto
            foreach (Edge<int> e in this.graph.Edges)
            {
                if (e.Tail.Equals(nodeToMerge))
                {
                    if (!e.Head.Equals(nodeToMergeInto))
                    {
                        //Repoint tail
                        e.Tail = nodeToMergeInto;

                        //remove old neighbor
                        nodeToMerge.RemoveNeighbor(e.Head);

                        //Add new neighbor
                        nodeToMergeInto.AddNeighbor(e.Head);
                    }
                    else
                    {
                        //This is an edge between the two we are going to merge
                        //Add it to the list for deletion later
                        edgesToBeRemoved.Add(e);                        
                    }
                }

                if (e.Head.Equals(nodeToMerge))
                {
                    if (!e.Tail.Equals(nodeToMergeInto))
                    {
                        //repoint head
                        e.Head = nodeToMergeInto;

                        //remove old neighbor
                        e.Tail.RemoveNeighbor(nodeToMerge);

                        //add new neighbor
                        e.Tail.AddNeighbor(nodeToMergeInto);
                    }
                    else
                    {
                        //This is an edge between the two we are going to merge
                        //add it to the list for later deletion
                        edgesToBeRemoved.Add(e);
                    }
                }
            }

            //Remove edges marked for deletion
            foreach (Edge<int> ed in edgesToBeRemoved)
            {
                this.graph.RemoveEdge(ed);
            }

            //All edges should be repointed. Now remove superfluous node
            nodeToMerge.RemoveAllNeighbors();
            nodeToMergeInto.RemoveNeighbor(nodeToMerge);
            this.graph.RemoveVertex(nodeToMerge);
        }

        private void DoSingleRandomizedContraction()
        {
            Edge<int> randomEdge = this.PickRandomEdge();
            //this.MergeNodes(ref randomEdge);
            this.MergeNodes(randomEdge);
        }

        public int FindMinCut_RandomizeContraction()
        {
            //Contract the graph until there are only two nodes left
            while (this.graph.Vertices.Count > 2)
            {
                this.DoSingleRandomizedContraction();
#if DEBUG
                //Console.WriteLine(string.Format("# Verticies after contraction: {0}, # edges: {1}\nContracted graph:\n{2}", this.graph.Order, this.graph.Edges.Count, this.graph.ToString()));
#endif
            }

            return this.graph.Edges.Count;
        }
    }
}
