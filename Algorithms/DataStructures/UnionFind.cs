using System;
using System.Collections.Generic;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class UnionFind_Dictionary<T> where T : struct, IComparable, IEquatable<T>
    {
        private List<T> listOfComponents;
        private Dictionary<int, List<T>> setsOfConnectedComponents;

        public UnionFind_Dictionary(List<T> listOfUnconnectedComponents, bool listIsUnsorted = true)
        {
            this.setsOfConnectedComponents = new Dictionary<int, List<T>>();
            this.listOfComponents = listOfUnconnectedComponents;
            for (int i = 0; i < listOfUnconnectedComponents.Count; i++)
            {
                this.setsOfConnectedComponents.Add(i, new List<T>() { listOfUnconnectedComponents[i] });
            }

            if (listIsUnsorted)
            {
                listOfComponents.Sort();
            }
        }

        public int NumberOfComponents { get { return this.setsOfConnectedComponents.Keys.Count; } }

        public int Find(T component)
        {
            int leader = -1;
            foreach (int i in setsOfConnectedComponents.Keys)
            {
                if (setsOfConnectedComponents[i].Contains(component))
                {
                    leader = i;
                    break;
                }
            }

            if (-1 == leader)
            {
                throw new Exception(string.Format("Failed to find component {0} in data set!", component.ToString()));
            }

            return leader;
        }

        public void Union(int leaderOfGroup1, int leaderOfGroup2)
        {
            if (this.setsOfConnectedComponents[leaderOfGroup1].Count.CompareTo(this.setsOfConnectedComponents[leaderOfGroup2].Count) <= 0)
            {
                //Fewer items in group1. Merge group1 into group2
                this.MergeHelper(leaderOfGroup1, leaderOfGroup2);
            }
            else
            {
                //fewer items in group2. merge group2 into group1
                this.MergeHelper(leaderOfGroup2, leaderOfGroup1);
            }
        }

        private void MergeHelper(int leaderOfGroupToMerge, int leaderOfGroupToMergeInto)
        {
            //Merge smaller group into larger
            List<T> smallerGroup = this.setsOfConnectedComponents[leaderOfGroupToMerge];
            List<T> largerGroup = this.setsOfConnectedComponents[leaderOfGroupToMergeInto];
            if (smallerGroup.Count.CompareTo(largerGroup.Count) > 0)
            {
                throw new ArgumentException(string.Format("Group to merge had more elements ({0}) than group to merge into ({1})!", smallerGroup.Count, largerGroup.Count));
            }
            this.setsOfConnectedComponents[leaderOfGroupToMergeInto].AddRange(this.setsOfConnectedComponents[leaderOfGroupToMerge]);
            this.setsOfConnectedComponents.Remove(leaderOfGroupToMerge);
        }
    }

    public class UnionFind<T, U> : Graph_AdjacencyList<T, U> 
        where T : Vertex_UnionFind<U>, ILeader<U>
        where U : struct, IComparable, IEquatable<U>
    {
        public UnionFind(List<T> unconnectedComponents) : base(unconnectedComponents) 
        {
        }

        public Graph_AdjacencyList<T, U> Graph
        {
            get { return this.DeepCopy(); }
        }

        public Dictionary<int, int> Clusters
        {
            get
            {
                Dictionary<int, int> clusters = new Dictionary<int, int>();
                foreach (T vertex in base.Vertices)
                {
                    if (!clusters.ContainsKey(vertex.Leader.Index))
                    {
                        clusters.Add(vertex.Leader.Index, ((T)vertex.Leader).Rank);
                    }
                }
                return clusters;
            }
        }

        public T Find(T objectToFindLeaderFor)
        {
            return (T)objectToFindLeaderFor.Leader;
        }

        public bool AreInSameConnectedSet(T a, T b)
        {
            return a.HasSameLeader(b);
        }

        public void Union(T leaderOfGroup1, T leaderOfGroup2)
        {
            int elementsIn1 = leaderOfGroup1.Rank;
            int elementsIn2 = leaderOfGroup2.Rank;
            if (elementsIn1.CompareTo(elementsIn2) < 0)
            {
                //Merge 1 into 2
                this.MergeHelper(leaderOfGroup1, leaderOfGroup2);
            }
            else
            {
                //Merge 2 into 1
                this.MergeHelper(leaderOfGroup2, leaderOfGroup1);
            }
        }

        private void MergeHelper(T leaderOfGroupToMerge, T leaderOfGroupToMergeInto)
        {
            //Do depth first search
            Stack<T> nodeStack = new Stack<T>();

            nodeStack.Push(leaderOfGroupToMerge);

            while (nodeStack.Count > 0)
            {
                T current = base.GetVertex(nodeStack.Pop().Index);

                if (!current.Leader.Equals(leaderOfGroupToMergeInto))
                {
                    //Update leader
                    current.Leader = leaderOfGroupToMergeInto;
                    leaderOfGroupToMergeInto.Rank++;
                }

                //Add neighbors to be updates to the stack
                foreach (T neighbor in current.Neighbors.Values)
                {
                    if (!current.Equals(neighbor) && 
                        !neighbor.Leader.Equals(leaderOfGroupToMergeInto) &&
                        !nodeStack.Contains(current))
                    {
                        nodeStack.Push(neighbor);
                    }
                }
            }
        }
    }
}
