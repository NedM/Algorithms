//#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Algorithms.DataStructures;
using Algorithms.Types;

namespace Algorithms
{
    public class Utilities
    {
        public static int[] GenerateRandomizedArray(int length, int minimum, int maximum)
        {
            int[] output = new int[length];
            Random rand = new Random((int)DateTime.Now.Ticks);
            for (int i = 0; i < length; i++)
            {
                output[i] = rand.Next(minimum, maximum);
            }
            return output;
        }

        #region Output formatting helpers

        public static string FormatPrintArray<T>(T[] arrayToPrint, bool onePerLine = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < arrayToPrint.Length; i++)
            {
                sb.Append(arrayToPrint[i].ToString());
                if (i < (arrayToPrint.Length - 1))
                {
                    sb.Append(",");
                }
            }
            sb.Append("]");
            return sb.ToString();
        }

        public static string FormatPrintList<T>(IList<T> list, bool onePerLine = false)
        {
            return FormatPrintCollection<T>(list, onePerLine);
        }

        public static string FormatPrintCollection<T>(IEnumerable<T> collection, bool onePerLine = false)
        {
            if (null == collection)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder();
            if (!onePerLine)
            {
                sb.Append("[");
                foreach (T item in collection)
                {
                    sb.AppendFormat("{0},", item.ToString());
                }

                if (',' == sb[sb.Length - 1])
                {
                    sb = sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("]");
            }
            else
            {
                foreach (T item in collection)
                {
                    sb.AppendFormat("{0}{1}", item.ToString(), Environment.NewLine);
                }
            }
            return sb.ToString();
        }

        public static string FormatPrintDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            StringBuilder sb = new StringBuilder();
            //sb.Append("[");
            foreach(TKey key in dictionary.Keys)
            {
                //sb.AppendFormat("({0}, {1})", key.ToString(), dictionary[key].ToString());
                string valueString = (null == dictionary[key]) ? "NULL" : dictionary[key].ToString();
                sb.AppendFormat("[{0}, {1}]{2}", key.ToString(), valueString, Environment.NewLine);
                //sb.Append(",");                
            }
            //sb = sb.Remove(sb.Length - 1, 1);
            //sb.Append("]");
            return sb.ToString();
        }

        public static string PrintBellmanFordPath<T, U>(T endingVertex)
            where T : PathVertex<U>
            where U : struct, IComparable, IEquatable<U>
        {
            PathVertex<U> current = endingVertex;
            StringBuilder sb = new StringBuilder();
            while (current != null)
            {
                if (!current.Equals(endingVertex))
                {
                    sb.Append("<-");
                }
                sb.AppendFormat("{0}", current.Index);

                current = current.Predecessor;
            }
            return string.Format("Path:{0}{1}", Environment.NewLine, sb.ToString());
        }

        #endregion Output formatting helpers

        #region Input reading helpers

        public static int[] ReadFileIntoArray(string pathToFile)
        {
            int[] arrayOfInts;

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }
            string[] allLines = File.ReadAllLines(pathToFile);
            arrayOfInts = new int[allLines.Length];
            for(int i = 0; i < allLines.Length; i++)
            {
                int.TryParse(allLines[i], out arrayOfInts[i]);
            }

            return arrayOfInts;
        }

        public static KnapsackItem[] ReadFileIntoKnapsackItemArray(string pathToFile)
        {
            KnapsackItem[] arrayOfItems;

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }
            string[] allLines = File.ReadAllLines(pathToFile);
            arrayOfItems = new KnapsackItem[allLines.Length];

            for (int i = 0; i < allLines.Length; i++)
            {
                string[] splitLine = allLines[i].Split(' ');

                if (splitLine.Length != 2)
                {
                    throw new Exception(string.Format("Input error! Expected each line to have to values separated by a space but found {0} values instead on line {1}!", splitLine.Length, i));
                }
                else
                {
                    arrayOfItems[i] = new KnapsackItem(double.Parse(splitLine[0]), int.Parse(splitLine[1]));
                }
            }
            return arrayOfItems;
        }

        public static List<KeyValuePair<int, int>> ReadFileIntoKeyValueList(string pathToFile)
        {
            List<KeyValuePair<int, int>> listOfKeysAndValues = new List<KeyValuePair<int, int>>();

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
#if DEBUG
            Console.WriteLine(string.Format("Read {0} lines into memory", allLines.Length));
#endif

            foreach (string line in allLines)
            {
                string[] splitStr = line.Split(' ');
                if (splitStr.Length != 2)
                {
                    throw new Exception(string.Format("Expected two space separated integers but found \"{0}\" instead! Aborting...", line));
                }

                listOfKeysAndValues.Add(new KeyValuePair<int, int>(int.Parse(splitStr[0]), int.Parse(splitStr[1])));
            }

#if DEBUG
            Console.WriteLine("List of keys and values:\n" + Utilities.FormatPrintList(listOfKeysAndValues));
#endif

            return listOfKeysAndValues;
        }

        public static Dictionary<int, bool> ReadFileIntoDictionary(string pathToFile)
        {
            Dictionary<int, bool> dictionaryToReturn;
            int key;

            string[] allLines = File.ReadAllLines(pathToFile);
            dictionaryToReturn = new Dictionary<int, bool>(allLines.Length);
            foreach (string s in allLines)
            {
                int.TryParse(s, out key);
                if (!dictionaryToReturn.ContainsKey(key))
                {
                    dictionaryToReturn.Add(key, false);
                }
                else
                {
#if DEBUG
                    Console.WriteLine("Already added key: " + key.ToString());
#endif
                }
            }

#if DEBUG
            Console.WriteLine("Dictionary:\n" + Utilities.FormatPrintDictionary<int, bool>(dictionaryToReturn));
#endif

            return dictionaryToReturn;
        }

        /// <summary>
        /// Reads a text file representation of a graph into a Graph_Adjacency object
        /// The text file should have rows of integer indexes separated by tabs where the
        /// first element in a row is the index of the vertex followed by the indices of its neighbors
        /// </summary>
        /// <param name="pathToFile"></param>
        /// <returns></returns>
        public static Graph_AdjacencyList<Vertex<int>, int> ReadAdjacencyListIntoGraph(string pathToFile)
        {
            Graph_AdjacencyList<Types.Vertex<int>, int> graphOfInts;
            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            graphOfInts = new Graph_AdjacencyList<Types.Vertex<int>, int>(allLines.Length);

            //Create all the vertices first
            foreach (string line in allLines)
            {
                string[] values = line.Split('\t');
                int index;
                int.TryParse(values[0], out index);

                //Create the current vertex
                Vertex<int> vertex = new Vertex<int>(index, 0);
                //Add the current vertex
                graphOfInts.AddVertex(vertex);
            }
            
            //Add neighbors and edges
            foreach (string line in allLines)
            {
                //Resplit the lines
                string[] values = line.Split('\t');
                int baseIndex;
                int.TryParse(values[0], out baseIndex);

                //Find the base vertex
                Vertex<int> baseVertex = graphOfInts.GetVertex(baseIndex);

                for (int i = 1; i < values.Length; i++)
                {
                    int neighborIndex;
                    int.TryParse(values[i], out neighborIndex);

                    //Find the neighbor index
                    Vertex<int> neighbor = graphOfInts.GetVertex(neighborIndex);
                        
                    //Add the neighboring vertex
                    baseVertex.AddNeighbor(neighbor);
                    //Add only a directed edge so we don't double count the edges
                    graphOfInts.AddDirectedEdge(baseVertex, neighbor);
                }
            }

#if DEBUG
            //Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graphOfInts.Order, graphOfInts.Edges.Count, graphOfInts.ToString()));            
#endif
            return graphOfInts;
        }

        public static Graph_AdjacencyList ReadAdjacencyListIntoGraphOfInts(string pathToFile, int numNodes = 500)
        {
            Graph_AdjacencyList graphOfInts;
            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            graphOfInts = new Graph_AdjacencyList(numNodes);

            //Create all the vertices first
            foreach (string line in allLines)
            {
                string[] values = line.Split(' ');
                int index;
                int.TryParse(values[0], out index);

                //Create the current vertex
                Vertex<int> vertex = new Vertex<int>(index);
                //Add the current vertex
                graphOfInts.AddVertex(vertex);
            }
            
            //Add edges
            foreach (string line in allLines)
            {
                //Resplit the lines
                string[] values = line.Split(' ');
                int tail, head, cost;
                int.TryParse(values[0], out tail);
                int.TryParse(values[1], out head);
                int.TryParse(values[2], out cost);

                if (!graphOfInts.Vertices.ContainsKey(tail))
                {
                    graphOfInts.AddVertex(new Vertex<int>(tail));
                }

                if (!graphOfInts.Vertices.ContainsKey(head))
                {
                    graphOfInts.AddVertex(new Vertex<int>(head));
                }

                graphOfInts.AddUndirectedEdge(tail, head, cost);
            }

#if DEBUG
            Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graphOfInts.Order, graphOfInts.Size, graphOfInts.ToString()));            
#endif
            return graphOfInts;
        }

        public static Graph_AdjacencyList<Vertex<int>, int> ReadAdjacencyListIntoGraph(string pathToFile, int numNodes = 500)
        {
            Graph_AdjacencyList<Vertex<int>, int> graph = new Graph_AdjacencyList<Vertex<int>, int>(numNodes);

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);

            //Create all the vertices first
            foreach (string line in allLines)
            {
                string[] values = line.Split(' ');
                int index;
                int.TryParse(values[0], out index);

                //Create the current vertex
                Vertex<int> vertex = new Vertex<int>(index);
                //Add the current vertex
                graph.AddVertex(vertex);
            }

            //Add edges
            foreach (string line in allLines)
            {
                //Resplit the lines
                string[] values = line.Split(' ');
                int tail, head, cost;
                int.TryParse(values[0], out tail);
                int.TryParse(values[1], out head);
                int.TryParse(values[2], out cost);

                Vertex<int> vTail = new Vertex<int>(tail);
                Vertex<int> vHead = new Vertex<int>(head);

                if (!graph.HasVertex(tail))
                {
                    graph.AddVertex(vTail);
                }

                if (!graph.HasVertex(head))
                {
                    graph.AddVertex(vHead);
                }

                graph.AddUndirectedEdge(vTail, vHead, cost);
            }

#if DEBUG
            Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graph.Order, graph.Size, graph.ToString()));
#endif
            return graph;
        }

        public static Graph_AdjacencyList<Vertex_UnionFind<int>, int> ReadAdjacencyListIntoUnionFindGraph(string pathToFile, int numNodes = 500)
        {
            Graph_AdjacencyList<Vertex_UnionFind<int>, int> graph = new Graph_AdjacencyList<Vertex_UnionFind<int>, int>(numNodes);

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);

            //Create all the vertices first
            foreach (string line in allLines)
            {
                string[] values = line.Split(' ');
                int index;
                int.TryParse(values[0], out index);

                //Create the current vertex
                Vertex_UnionFind<int> vertex = new Vertex_UnionFind<int>(index);
                //Add the current vertex
                graph.AddVertex(vertex);
            }

            //Add edges
            foreach (string line in allLines)
            {
                //Resplit the lines
                string[] values = line.Split(' ');
                int tail, head, cost;
                int.TryParse(values[0], out tail);
                int.TryParse(values[1], out head);
                int.TryParse(values[2], out cost);

                Vertex_UnionFind<int> vTail = new Vertex_UnionFind<int>(tail);
                Vertex_UnionFind<int> vHead = new Vertex_UnionFind<int>(head);

                if (!graph.HasVertex(tail))
                {
                    graph.AddVertex(vTail);
                }

                if (!graph.HasVertex(head))
                {
                    graph.AddVertex(vHead);
                }

                graph.AddDirectedEdge(vTail, vHead, cost);
            }

#if DEBUG
            Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graph.Order, graph.Size, graph.ToString()));
#endif
            return graph;
        }

        public static Graph_AdjacencyList<PathVertex<double>, double> ReadEdgeListIntoGraph_BellmanFord(string pathToFile, int numNodes = 1000)
        {
            Graph_AdjacencyList<PathVertex<double>, double> graph = new Graph_AdjacencyList<PathVertex<double>, double>(numNodes);
            int index0, index1;
            double cost;

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            for (int i = 0; i < allLines.Length; i++)
            {
                string[] splitLine = allLines[i].Split(' ');
                if (splitLine.Length != 3)
                {
                    throw new Exception(string.Format("File read error at line {0}! Expected line to have 3 space separated entries but there were {1} instead!", i, splitLine.Length));
                }

                int.TryParse(splitLine[0], out index0);
                int.TryParse(splitLine[1], out index1);
                double.TryParse(splitLine[2], out cost);

                graph.AddVertex(new PathVertex<double>(index0));
                graph.AddVertex(new PathVertex<double>(index1));

                graph.AddDirectedEdge(index0, index1, cost);
#if DEBUG
                if (i % 10000 == 0)
                {
                    Console.WriteLine("Done parsing line " + i);
                }
#endif
            }
            Console.WriteLine("Done parsing " + pathToFile);
#if DEBUG
            //Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graphOfInts.Order, graphOfInts.Edges.Count, graphOfInts.ToString()));            
#endif
            return graph;
        }

        public static Graph_AdjacencyList ReadEdgeListIntoAdjacencyList(string pathToFile, int numNodes = 1000)
        {
            Graph_AdjacencyList graph = new Graph_AdjacencyList(numNodes);
            int index0, index1;
            int cost;

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            for (int i = 0; i < allLines.Length; i++)
            {
                string[] splitLine = allLines[i].Split(' ');
                if (splitLine.Length != 3)
                {
                    throw new Exception(string.Format("File read error at line {0}! Expected line to have 3 space separated entries but there were {1} instead!", i, splitLine.Length));
                }

                int.TryParse(splitLine[0], out index0);
                int.TryParse(splitLine[1], out index1);
                int.TryParse(splitLine[2], out cost);

                graph.AddVertex(new PathVertex<int>(index0));
                graph.AddVertex(new PathVertex<int>(index1));

                graph.AddDirectedEdge(index0, index1, cost);
#if DEBUG
                if (i % 10000 == 0)
                {
                    Console.WriteLine("Done parsing line " + i);
                }
#endif
            }
            Console.WriteLine("Done parsing " + pathToFile);
#if DEBUG
            //Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graphOfInts.Order, graphOfInts.Edges.Count, graphOfInts.ToString()));            
#endif
            return graph;
        }

        public static Graph_AdjacencyList_Lite ReadEdgeListIntoGraph_Lite(string pathToFile, int numNodes = 500)
        {
            Graph_AdjacencyList_Lite graphOfInts;
            int index0, index1, cost;

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            graphOfInts = new Graph_AdjacencyList_Lite(numNodes);
            for (int i = 0; i < allLines.Length; i++)
            {
                string[] splitLine = allLines[i].Split(' ');
                if (splitLine.Length != 3)
                {
                    throw new Exception(string.Format("File read error at line {0}! Expected line to have 3 space separated entries but there were {1} instead!", i, splitLine.Length));
                }

                int.TryParse(splitLine[0], out index0);
                int.TryParse(splitLine[1], out index1);
                int.TryParse(splitLine[2], out cost);

                graphOfInts.AddVertex(index0);
                graphOfInts.AddVertex(index1);

                graphOfInts.AddDirectedEdge(index0, index1, cost);
#if DEBUG
                if (i % 10000 == 0)
                {
                    Console.WriteLine("Done parsing line " + i);
                }
#endif
            }
            Console.WriteLine("Done parsing " + pathToFile);
#if DEBUG
            //Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graphOfInts.Order, graphOfInts.Edges.Count, graphOfInts.ToString()));            
#endif
            return graphOfInts;
        }

        public static Graph_AdjacencyList<Vertex<int>, int> ReadEdgeListIntoGraph(string pathToFile)
        {
            const int numNodes = 875714;
            Graph_AdjacencyList<Vertex<int>, int> graphOfInts;
            Vertex<int> v0;
            Vertex<int> v1;
            int index0;
            int index1;

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            graphOfInts = new Graph_AdjacencyList<Vertex<int>, int>(numNodes, allLines.Length);
            for(int i = 0; i < allLines.Length; i++)
            {
                    string[] splitLine = allLines[i].Split(' ');
                    if (splitLine.Length != 3)
                    {
                        throw new Exception(string.Format("File read error at line {0}! Expected line to have 3 space separated entries but there were {1} instead!", i, splitLine.Length));
                    }

                    int.TryParse(splitLine[0], out index0);
                    int.TryParse(splitLine[1], out index1);
                    v0 = new Vertex<int>(index0, 0);
                    v1 = new Vertex<int>(index1, 0);

                    v0.AddNeighbor(v1);

                    graphOfInts.AddVertex(v0);
                    graphOfInts.AddVertex(v1);

                    graphOfInts.AddDirectedEdge(v0, v1);
#if DEBUG
                    if (i % 10000 == 0)
                    {
                        Console.WriteLine("Done parsing line " + i);
                    }
#endif
            }
#if DEBUG
            //Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graphOfInts.Order, graphOfInts.Edges.Count, graphOfInts.ToString()));            
#endif
            return graphOfInts;
        }

        public static Graph_AdjacencyList<PathVertex<Point>, Point> ReadPointListIntoGraph(string pathToFile, int numNodes = 25)
        {
            Graph_AdjacencyList<PathVertex<Point>, Point> graph = new Graph_AdjacencyList<PathVertex<Point>, Point>(numNodes);
            double x, y;
            double cost;

            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            //Add all the vertices
            for (int i = 0; i < allLines.Length; i++)
            {
                string[] splitLine = allLines[i].Split(' ');
                if (splitLine.Length != 2)
                {
                    throw new Exception(string.Format("File read error at line {0}! Expected line to have 2 space separated entries but there were {1} instead!", i, splitLine.Length));
                }

                double.TryParse(splitLine[0], out x);
                double.TryParse(splitLine[1], out y);

                //Store the vertex position as the data
                graph.AddVertex(new PathVertex<Point>(i, new Point(x, y)));
#if DEBUG
                if (i % 10000 == 0)
                {
                    Console.WriteLine("Done parsing line " + i);
                }
#endif
            }

            //Now add edges
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                for (int j = 0; j < graph.Vertices.Count; j++)
                {
                    if (i != j)
                    {
                        cost = Point.CalculateEuclidianDistance(graph.Vertices[i].Data, graph.Vertices[j].Data);
                        graph.AddDirectedEdge(i, j, cost);
                    }
                }
            }
            Console.WriteLine("Done parsing " + pathToFile);
#if DEBUG
            //Console.WriteLine(string.Format("# Verticies: {0}, # edges: {1}\nOriginal Graph:\n{2}", graphOfInts.Order, graphOfInts.Edges.Count, graphOfInts.ToString()));            
#endif
            return graph;
        }

        public static Graph_AdjacencyList<LogicVertex<int>, int> Read2SATDataIntoImplicationGraph(string pathToFile, int numNodes = 1000)
        {
            Graph_AdjacencyList<LogicVertex<int>, int> graph = new Graph_AdjacencyList<LogicVertex<int>, int>(numNodes);
            int a, b, aIndex, bIndex;
            bool logA = true, logB = true;
            
            if (!File.Exists(pathToFile))
            {
                throw new ArgumentException(string.Format("{0} does not exist!", pathToFile));
            }

            string[] allLines = File.ReadAllLines(pathToFile);
            //Add all the vertices
            for (int i = 0; i < allLines.Length; i++)
            {
                string[] splitLine = allLines[i].Split(' ');
                if (splitLine.Length != 2)
                {
                    throw new Exception(string.Format("File read error at line {0}! Expected line to have 2 space separated entries but there were {1} instead!", i, splitLine.Length));
                }

                int.TryParse(splitLine[0], out aIndex);
                int.TryParse(splitLine[1], out bIndex);

                a = Math.Abs(aIndex);
                b = Math.Abs(bIndex);

                logA = (aIndex >= 0);
                logB = (bIndex >= 0);
                
                LogicVertex<int> lvA = new LogicVertex<int>(aIndex, a, logA);
                if (!graph.HasVertex(aIndex))
                {
                    graph.AddVertex(lvA);
                }

                int notAIndex = aIndex * (-1);
                LogicVertex<int> lvNotA = new LogicVertex<int>(notAIndex, a, !logA);
                if (!graph.HasVertex(notAIndex))
                {
                    graph.AddVertex(lvNotA);
                }
                
                LogicVertex<int> lvB = new LogicVertex<int>(bIndex, b, logB);
                if (!graph.HasVertex(bIndex))
                {
                    graph.AddVertex(lvB);
                }

                int notBIndex = bIndex * (-1);
                LogicVertex<int> lvNotB = new LogicVertex<int>(notBIndex, b, !logB);
                if (!graph.HasVertex(notBIndex))
                {
                    graph.AddVertex(lvNotB);
                }

                graph.AddDirectedEdge(lvNotA, lvB);
                graph.AddDirectedEdge(lvNotB, lvA);
            }
#if DEBUG
            Console.WriteLine(string.Format("# verticies: {0}. # edges: {1}", graph.Order, graph.Size));            
#endif
            return graph;
        }

        public static MaxArrayHeap<WeightedJob> ReadJobListIntoHeap(string pathToFile)
        {
            MaxArrayHeap<WeightedJob> heap;

            string[] allLines = File.ReadAllLines(pathToFile);
            heap = new MaxArrayHeap<WeightedJob>(allLines.Length);

            for(int i = 0; i < allLines.Length; i++)
            {
                int w, l;
                string[] subStr = allLines[i].Split(' ');
                if (subStr.Length != 2)
                {
                    throw new Exception(string.Format("Line {0} of {1} had length {2} when length 2 was expected!", i, allLines.Length, subStr.Length));
                }

                if (!int.TryParse(subStr[0], out w))
                {
                    throw new Exception(string.Format("Value {0} in line {1} is not an int!", subStr[0], i));
                }

                if (!int.TryParse(subStr[1], out l))
                {
                    throw new Exception(string.Format("Value {0} in line {1} is not an int!", subStr[1], i));
                }

                WeightedJob job = new WeightedJob(w, l);
                heap.Insert(job);

                if (!heap.VerifyHeap())
                {
                    Console.WriteLine(string.Format("Warning! Found potential error in heap after inserting {0}", job.ToString())); 
                }
            }

            Console.WriteLine("Done parsing input file! Read heap with size " + heap.Size);
#if DEBUG
            Console.WriteLine("Heap:\n" + heap.ToString());
#endif

            return heap;
        }

        #endregion Input reading helpers

        public static int CalculateHammingDistance(int a, int b)
        {
            int dist = 0, val = a ^ b;
            // Count the number of set bits (Knuth's algorithm)
            while (val > 0)
            {
                ++dist;
                val &= val - 1;
            }
            return dist;
        }
    }
}
