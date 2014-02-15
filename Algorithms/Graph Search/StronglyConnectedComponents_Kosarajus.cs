#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.DataStructures;
using Algorithms.Types;

namespace Algorithms.Graph_Search
{
    public class StronglyConnectedComponents<T, U>
        where T : Vertex<U>
        where U : struct, IComparable, IEquatable<U>
    {
        private IGraph<T, U> graph;
        private int finishingTime;
        private Dictionary<int, T> vertexFinishingTimes;

        public StronglyConnectedComponents(IGraph<T, U> inputGraph)
        {
            this.graph = inputGraph;
            this.finishingTime = 0;
            this.vertexFinishingTimes = new Dictionary<int, T>();
        }

        public Dictionary<int, List<T>> FindSCCs_KosarajusAlg()
        {
            if (null == this.graph || this.graph.Vertices.Count <= 0)
            {
                throw new Exception("Graph is not valid! Please construct a valid object before calling this method");
            }

            Dictionary<int, List<T>> stronglyConnectedComponents = new Dictionary<int, List<T>>();

#if DEBUG
            Console.WriteLine(string.Format("Input graph:{1}{0}", this.graph.ToString(), Environment.NewLine));
            Console.WriteLine("Reversing input graph...");
#endif

            IGraph<T, U> reversed = Graph_AdjacencyList<T, U>.ReverseGraph(this.graph);

#if DEBUG
            Console.WriteLine(string.Format("Reversed graph:{1}{0}", reversed.ToString(), Environment.NewLine));
            Console.WriteLine("Finding finishing times using recursion...");
#endif
            foreach (T node in reversed.Vertices)
            {
                this.Kosaraju_DFS_Recursive(ref reversed, node);
            }

#if DEBUG
            Console.WriteLine(string.Format("Finishing times:{1}{0}", Utilities.FormatPrintDictionary<int, T>(this.vertexFinishingTimes), Environment.NewLine));
            Console.WriteLine("Finding SCCs using stack...");
#endif
            for (int i = this.graph.Vertices.Count; i > 0; i--)
            {
                KeyValuePair<int, List<T>> sccKVP = this.Kosaraju_DFS(ref this.graph, this.vertexFinishingTimes[i]);
                if (!stronglyConnectedComponents.ContainsKey(sccKVP.Key) && sccKVP.Value.Count > 0)
                {
#if DEBUG
                    Console.WriteLine(string.Format("Adding strongly connected component with leader index {0} and {1} members to the collection.", sccKVP.Key, sccKVP.Value.Count));
#endif
                    stronglyConnectedComponents.Add(sccKVP.Key, sccKVP.Value);
                }
            }
#if DEBUG
            Console.WriteLine(string.Format("Strongly connected components:"));
            foreach (int key in stronglyConnectedComponents.Keys)
            {
                Console.WriteLine(string.Format("Leader: {0} with {1} members: [{2}]", 
                    key, stronglyConnectedComponents[key].Count, Utilities.FormatPrintList<T>(stronglyConnectedComponents[key])));
            }
#endif

            return stronglyConnectedComponents;
        }

        private void Kosaraju_DFS_Recursive(ref IGraph<T, U> revGraph, T node)
        {
            if (node.Visited)
            {
                return;
            }

            node.Visited = true;
            foreach (int index in node.Neighbors.Keys)
            {
                T neighborInGraph = revGraph.GetVertex(index);
                if (!neighborInGraph.Visited)
                {
                    this.Kosaraju_DFS_Recursive(ref revGraph, neighborInGraph);
                }
            }
            this.finishingTime++;
            T finishedVertex = (T)node.ShallowCopy();
            finishedVertex.Visited = false;
            this.vertexFinishingTimes.Add(this.finishingTime, finishedVertex);
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns>A KeyValuePair with key = index of the leader and value = list of nodes in the SCC</returns>
        private KeyValuePair<int, List<T>> Kosaraju_DFS(ref IGraph<T, U> graph, T node)
        {
            Stack<T> nodeStack = new Stack<T>();
            KeyValuePair<int, List<T>> SCC = new KeyValuePair<int, List<T>>(node.Index, new List<T>());

            int nodeIndex = node.Index;
            T nodeInGraph = graph.GetVertex(nodeIndex);
            if (!nodeInGraph.Visited)
            {
                nodeStack.Push(nodeInGraph);
                while (nodeStack.Count > 0)
                {
                    T current = nodeStack.Pop();
                    current.Visited = true;
                    SCC.Value.Add(current);

                    foreach (int index in current.Neighbors.Keys)
                    {
                        T neighborInGraph = graph.GetVertex(index);
                        if (!neighborInGraph.Visited)
                        {
                            nodeStack.Push(neighborInGraph);
                        }
                    }
                }
            }
            return SCC;
        }
    }

    public class StronglyConnectedComponents_Kosarajus
    {
        private int finishingTime;
        private Dictionary<int, int> vertexFinishingTimes; //Key = FinishingTime, Value = Vertex
        private Vertex<int> mostRecentLeader;
        private KeyValuePair<int, int> SCC; //key = leader, value = size

        public StronglyConnectedComponents_Kosarajus()
        {
            this.finishingTime = 0;
            this.mostRecentLeader = null;
            this.SCC = new KeyValuePair<int, int>();
            this.vertexFinishingTimes = new Dictionary<int, int>();
        }

        public Dictionary<int, int> FindSCCsInGraph(Graph_AdjacencyList_Lite graph)
        {
            Dictionary<int, int> leaders = new Dictionary<int, int>();

            graph.MarkAllVerticesAsUnvisited();
            this.finishingTime = 0;
            this.SCC = new KeyValuePair<int, int>();

#if DEBUG
            Console.WriteLine("Reversing Graph...");
#endif

            //Reverse the graph
            Graph_AdjacencyList_Lite reversed = Graph_AdjacencyList_Lite.ReverseGraph(graph);

#if DEBUG
            Console.WriteLine("Finding finishing times using recursion...");
#endif

            //Do DFS #1 and keep track of finishing times
            foreach (int v in reversed.Vertices.Keys)
            {
                this.Kosaraju_DFS_Recursive(ref reversed, v);  //use Recursive method to find finishing times
            }

#if DEBUG
            Console.WriteLine("Finding leaders for SCCs using stack...");
#endif


            //Run DFS #2 returning the leader node for each SCC
            for (int i = (graph.Order); i > 0; i--)
            {                
#if DEBUG
                if (i % 10000 == 0)
                {
                    Console.WriteLine("Running DFS starting from node with finishing time " + i);
                }
#endif
                this.Kosarajus_DFS(ref graph, this.vertexFinishingTimes[i]);  //use stack method to find leaders
                if (!leaders.ContainsKey(this.SCC.Key))
                {
                    leaders.Add(this.SCC.Key, this.SCC.Value);
#if DEBUG
                    if (i % 1000 == 0)
                    {
                        Console.WriteLine("Added leader to list. List now contains {0} items", leaders.Keys.Count);
                    }
#endif

                }
            }

#if DEBUG
            Console.WriteLine("Done finding SCCs!");
#endif

            return leaders;
        }
                    
        private void Kosaraju_DFS_Recursive(ref Graph_AdjacencyList_Lite graph, int node)
        {
            if (graph.Visited[node] == true)
            {
#if DEBUG
                //Console.WriteLine(string.Format("Already visited node with index {0}", node));
#endif
                return;
            }

#if DEBUG
            //Console.WriteLine(string.Format("Inspecting node with index {0}", node));
#endif

            graph.Visited[node] = true;
            foreach (int n in graph.Vertices[node])
            {
                if (!graph.Visited[n])
                {
                    this.Kosaraju_DFS_Recursive(ref graph, n);
                }
            }

            this.finishingTime++;
            this.vertexFinishingTimes.Add(finishingTime, node);
#if DEBUG
            if (this.finishingTime % 10000 == 0)
            {
                Console.WriteLine(string.Format("Finished node with index {0}. Finishing time was {1}", this.vertexFinishingTimes[finishingTime], finishingTime));
            }
#endif

            return;
        }

        private void Kosarajus_DFS(ref Graph_AdjacencyList_Lite graph, int initial)
        {
            Stack<int> nodeStack = new Stack<int>();
            int sizeOfSCC = 0;
            
            if(graph.Visited[initial] == true)
            {
                //If we've already visited the start node, return
#if DEBUG
                //Console.WriteLine(string.Format("Can't start here! Already visited node with index {0}", initial));
#endif
                return;
            }

#if DEBUG
            //Console.WriteLine(string.Format("Starting at unvisited node with index {0}", initial));
#endif
            graph.Visited[initial] = true;
            nodeStack.Push(initial);
            sizeOfSCC++;
            while (nodeStack.Count > 0)
            {
                int node = nodeStack.Pop();
#if DEBUG
                //Console.WriteLine(string.Format("Inspecting node with index {0}", node));
#endif
                //push the neighbors onto the stack
                foreach (int n in graph.Vertices[node])
                {
                    if (!graph.Visited[n])
                    {
#if DEBUG
                        //Console.WriteLine(string.Format("Visiting unvisited node with index {0}", n));
#endif
                        graph.Visited[n] = true;  //Mark the node as visited
                        nodeStack.Push(n);  //And push it onto the stack
                        sizeOfSCC++;
                    }
                    else
                    {
#if DEBUG
                        //Console.WriteLine(string.Format("Already visited node with index {0}", n));
#endif
                    }
                }
            }
            this.SCC = new KeyValuePair<int, int>(initial, sizeOfSCC);
        }
    }
}
