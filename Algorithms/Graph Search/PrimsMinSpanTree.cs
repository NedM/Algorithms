using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;
using Algorithms.DataStructures;

namespace Algorithms.Graph_Search
{
    public class PrimsMinSpanTree
    {
        private Graph_AdjacencyList graph;

        public PrimsMinSpanTree(Graph_AdjacencyList graph)
        {
            this.graph = graph;
        }

        public Graph_AdjacencyList FindMinimumSpaningTree()
        {
            Graph_AdjacencyList minSpanTree = new Graph_AdjacencyList();

            //Select an arbitrary starting vertex
            int startingVertex = this.SelectRandomVertexInGraph();            
            //Add the first vertex to the spanning tree
            minSpanTree.AddVertex(this.graph.Vertices[startingVertex].DeepCopy());

            //Examine edges crossing the boundary (where the tail is in the spanned vertices and the head is not)
            List<Edge<int>> crossingEdges = this.FindEdgesCrossingBoundary(minSpanTree.VertexIndices);
            while (crossingEdges.Count > 0)
            {
                //and find the one with the lowest cost
                crossingEdges.Sort();
                minSpanTree.AddVertex(new Vertex<int>(crossingEdges[0].Head));
                minSpanTree.AddUndirectedEdge(crossingEdges[0].Tail, crossingEdges[0].Head, crossingEdges[0].Weight);
                
                crossingEdges.Clear();
                crossingEdges = this.FindEdgesCrossingBoundary(minSpanTree.VertexIndices);
            }

            return minSpanTree;
        }

        private int SelectRandomVertexInGraph()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int rInt = -1;
            int count = 0;
            int stopCount = rand.Next(0, this.graph.Vertices.Keys.Count);

            foreach (int key in this.graph.Vertices.Keys)
            {
                count++;
                if (count >= stopCount)
                {
                    rInt = key;
                    break;
                }
            }

            return rInt;
        }

        private List<Edge<int>> FindEdgesCrossingBoundary(List<int> spannedVertices)
        {
            List<Edge<int>> crossingEdges = new List<Edge<int>>();

            foreach (Edge<int> e in this.graph.Edges)
            {
                if (!crossingEdges.Contains(e) && 
                    spannedVertices.Contains(e.Tail.Index) && 
                    !spannedVertices.Contains(e.Head.Index))
                {
                    crossingEdges.Add(e);
                }
            }

            return crossingEdges;
        }
    }
}
