using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;

namespace Algorithms
{
    public class DepthFirstSearch<T, U> 
        where T : Vertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        private Graph_AdjacencyList<T, U> graph;

        public DepthFirstSearch(Graph_AdjacencyList<T, U> graph)
        {
            this.graph = graph;
        }

        public void Search(T start, T targetedData)
        {
            this.Search(start.Index, targetedData);
        }

        public T Search(int startIndex, T targetedData)
        {
            Stack<T> nodeStack = new Stack<T>();
            T start = this.graph.Vertices[startIndex];
            if (start.Visited)
            {
                //If we've already visited the start node, return
#if DEBUG
                Console.WriteLine(string.Format("Can't start here! Already visited node with index {0}", start.Index));
#endif
                return null;
            }

#if DEBUG
            Console.WriteLine(string.Format("Starting at unvisited node with index {0}", start.Index));
#endif
            start.Visited = true;
            nodeStack.Push(start);

            while (nodeStack.Count > 0)
            {
                T node = nodeStack.Pop();
#if DEBUG                
                Console.WriteLine(string.Format("Inspecting node with index {0}", node.Index));
#endif

                //If this node is the target, return the node
                if (node.Data.Equals(targetedData))
                {
                    return node;
                }

                //Otherwise push the neighbors onto the stack
                foreach (int n in node.Neighbors.Keys)
                {
                    if(!node.Neighbors[n].Visited)
                    {
#if DEBUG
                        Console.WriteLine(string.Format("Visiting unvisited node with index {0}", node.Neighbors[n].Index));
#endif
                        node.Neighbors[n].Visited = true;  //Mark the node as visited
                        nodeStack.Push((T)node.Neighbors[n]);  //And push it onto the stack
                    }
                    else
                    {
#if DEBUG
                        Console.WriteLine(string.Format("Already visited node with index {0}", node.Neighbors[n].Index));
#endif 
                    }
                }
            }

            //throw new Exception(string.Format("Failed to find target {0} within the graph!", targetedData.ToString()));
            return null;
        }
    }
}
