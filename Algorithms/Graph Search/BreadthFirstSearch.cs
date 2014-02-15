using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;

namespace Algorithms
{
    public class BreadthFirstSearch<T, U> 
        where T : Vertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        private Queue<T> nodeQueue;
        private Graph_AdjacencyList<T, U> graph;

        public BreadthFirstSearch(Graph_AdjacencyList<T, U> graph)
        {
            this.nodeQueue = new Queue<T>();
            this.graph = graph;
        }

        public void Search(T startVertex)
        {
            this.Search(startVertex.Index);
        }

        public void Search(int startIndex)
        {
            T start = this.graph.Vertices[startIndex];
            start.Visited = true;
            this.nodeQueue.Enqueue(start);

            while (this.nodeQueue.Count > 0)
            {
                T node = this.nodeQueue.Dequeue();
#if DEBUG
                Console.WriteLine(string.Format("Visiting node with index {0}", node.Index));
#endif

                //If the node is the target, return the node

                //otherwise, enqueue the neighbors
                foreach (int n in node.Neighbors.Keys)
                {
                    if (!node.Neighbors[n].Visited)
                    {
                        node.Neighbors[n].Visited = true; //mark the node as visited 
                        this.nodeQueue.Enqueue((T)node.Neighbors[n]);  //and add it to the queue
                    }
                }
            }
        }
    }
}
