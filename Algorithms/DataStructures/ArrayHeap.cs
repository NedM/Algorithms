#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class ArrayHeap<T> where T : IComparable, IEquatable<T>
    {
        public enum HeapType { Min, Max };

        private readonly HeapType heapType;
        protected List<T> HeapList;

        public ArrayHeap(HeapType typeOfHeap, int size = 0)
        {
            this.heapType = typeOfHeap;
            if (size <= 0)
            {
                this.HeapList = new List<T>();
            }
            else
            {
                this.HeapList = new List<T>(size);
            }
        }

        public T Root
        {
            get
            {
                if (this.HeapList.Count <= 0)
                {
                    throw new IndexOutOfRangeException("Heap is empty! Cannot return the root!");
                }
                else
                {
                    return this.HeapList[0];
                }
            }
        }

        public int Size { get { return this.HeapList.Count; } }

        public int Count { get { return this.Size; } }

        public System.Collections.ObjectModel.ReadOnlyCollection<T> Heap
        {
            get { return this.HeapList.AsReadOnly(); }
        }

        public T Peek()
        {
            return this.Root;
        }

        /// <summary>
        /// Removes the root of the heap and returns it
        /// </summary>
        /// <returns>The Root node of the heap</returns>
        public T Pop()
        {
            T root = this.Root;
            this.DeleteAt(0);
            return root;
        }

        public virtual void Insert(T node)
        {
            this.Insert(node, this.heapType);
        }

        public virtual void Delete(T node)
        {
            this.Delete(node, this.heapType);
        }

        public void DeleteAt(int iNode)
        {
            this.DeleteAt(iNode, this.heapType);
        }

        public bool VerifyHeap()
        {
            return this.VerifyHeap(this.heapType);
        }

        public void FixHeap()
        {
            this.FixHeap(this.heapType);
        }
        
        public override string ToString()
        {
            return Utilities.FormatPrintList<T>(this.HeapList);
        }

        protected void Insert(T node, HeapType type)
        {
            this.HeapList.Add(node);
            int iNode = this.HeapList.LastIndexOf(node);
            int iParent = this.GetParentIndex(iNode);
            T temp;

            //Compare recently added node to its parent
            while ((iParent >= 0) && this.CompareNodeWithParent(type, iNode, iParent))
            {
#if DEBUG
                //Console.WriteLine(string.Format("Swapping {0} in position {1} with {2} in position {3}", this.HeapList[iParent], iParent, this.HeapList[iNode], iNode));
#endif
                //If the node comes earlier in the sequence than the parent...
                //Do swap
                temp = this.HeapList[iParent];

                this.HeapList[iParent] = this.HeapList[iNode];
                this.HeapList[iNode] = temp;

                iNode = iParent;
                iParent = this.GetParentIndex(iNode);
            }
        }

        protected void Delete(T node, HeapType type)
        {
            if (!this.HeapList.Contains(node))
            {
                //node does not exist in the tree. Return
                return;
            }

            int iNode = this.HeapList.LastIndexOf(node);
            this.DeleteAt(iNode, type);
        }

        protected void DeleteAt(int iNode, HeapType type)
        {
            if (iNode < 0 || iNode >= this.HeapList.Count)
            {
                throw new ArgumentException(string.Format("iNode must be between 0 and {0}!", (this.HeapList.Count - 1)));
            }

            T temp;
            //Swap node to be deleted with last node in list
            temp = this.HeapList[iNode];
            this.HeapList[iNode] = this.HeapList[this.HeapList.Count - 1];
            this.HeapList[this.HeapList.Count - 1] = temp;
            //Delete the node at the end of the list now
            this.HeapList.RemoveAt(this.HeapList.Count - 1);

            //While we're in the tree and while the node compares unfavorably with either
            //of its children, do the swap
            int indexOfMaxMinChild = this.GetChildIndex(iNode, type);

            while (indexOfMaxMinChild >= 0 && indexOfMaxMinChild < this.HeapList.Count &&
                   this.HeapList[iNode].CompareTo(this.HeapList[indexOfMaxMinChild]) < 0)
            {
                //Do swap
#if DEBUG
                //Console.WriteLine(string.Format("Swapping {0} in position {1} with {2} in position {3}", this.HeapList[iNode], iNode, this.HeapList[indexOfMaxMinChild], indexOfMaxMinChild));
#endif
                temp = this.HeapList[iNode];
                this.HeapList[iNode] = this.HeapList[indexOfMaxMinChild];
                this.HeapList[indexOfMaxMinChild] = temp;

                iNode = indexOfMaxMinChild;
                indexOfMaxMinChild = this.GetChildIndex(iNode, type);
            }
        }

        protected bool VerifyHeap(HeapType type)
        {
            bool treeOk = true;

            for (int i = 0; i < this.HeapList.Count; i++)
            {
                int leftIndex = 2 * i + 1;
                int rightIndex = 2 * i + 2;
                int compareLeft;
                int compareRight;
                bool bCompare = false;

                if (leftIndex < this.HeapList.Count)
                {
                    compareLeft = this.HeapList[i].CompareTo(this.HeapList[leftIndex]);
                    bCompare = (type == HeapType.Min) ? compareLeft > 0 : compareLeft < 0;
                    if (bCompare)
                    {
                        //Error in the tree!
                        treeOk = false;
#if DEBUG
                        Console.WriteLine(string.Format("Error in heap detected! {0} at position {1} vs. {2} at position {3}", this.HeapList[i], i, this.HeapList[leftIndex], leftIndex));
#endif
                        break;
                    }
                }

                if (rightIndex < this.HeapList.Count)
                {
                    compareRight = this.HeapList[i].CompareTo(this.HeapList[rightIndex]);
                    bCompare = (type == HeapType.Min) ? compareRight > 0 : compareRight < 0;
                    if (bCompare)
                    {
                        //Error in the tree!
                        treeOk = false;
#if DEBUG
                        Console.WriteLine(string.Format("Error in heap detected! {0} at position {1} vs. {2} at position {3}", this.HeapList[i], i, this.HeapList[rightIndex], rightIndex));
#endif
                        break;
                    }
                }
            }

            return treeOk;
        }

        protected void FixHeap(HeapType type)
        {
            int i = 0;
            int compareLeft, compareRight;
            bool bCompare;

            while (this.HeapList != null && i < this.HeapList.Count)
            {
                if (i < 0) { i = 0; }
                int leftIndex = 2 * i + 1, rightIndex = 2 * i + 2, swapIndex = i;
                if (leftIndex < this.HeapList.Count)
                {
                    compareLeft = this.HeapList[swapIndex].CompareTo(this.HeapList[leftIndex]);
                    bCompare = (type == HeapType.Min) ? compareLeft > 0 : compareLeft < 0;
                    if (bCompare)
                    {
                        //Items are out of order!
                        swapIndex = leftIndex;
                    }
                }

                if (rightIndex < this.HeapList.Count)
                {
                    compareRight = this.HeapList[swapIndex].CompareTo(this.HeapList[rightIndex]);
                    bCompare = (type == HeapType.Min) ? compareRight > 0 : compareRight < 0;
                    if (bCompare)
                    {
                        //Items are out of order
                        swapIndex = rightIndex;
                    }
                }

                if (swapIndex != i)
                {
                    //Do swap
                    T temp = this.HeapList[i];
                    this.HeapList[i] = this.HeapList[swapIndex];
                    this.HeapList[swapIndex] = temp;

                    //Move back up the heap to make sure that we swap all the way back to the root
                    i--;
                }
                else
                {
                    i++;
                }
            }
        }

        protected int GetParentIndex(int nodeIndex)
        {
            if (nodeIndex < 0)
            {
                throw new ArgumentException("Index must be a real integer greater or equal to zero", "nodeIndex");
            }

            double approxParentIndex = (nodeIndex - 1) / 2.0;
            return (int)Math.Floor(approxParentIndex);
        }

        protected int GetChildIndex(int nodeIndex, HeapType type)
        {
            if (nodeIndex < 0)
            {
                throw new ArgumentException("Index must be a real integer greater or equal to zero", "nodeIndex");
            }

            int leftIndex = 2 * nodeIndex + 1;
            int rightIndex = 2 * nodeIndex + 2;

            if (leftIndex >= this.HeapList.Count &&
                rightIndex >= this.HeapList.Count)
            {
                return -1;
            }
            else if (leftIndex >= this.HeapList.Count)
            {
                return rightIndex;
            }
            else if (rightIndex >= this.HeapList.Count)
            {
                return leftIndex;
            }
            else
            {
                T left = this.HeapList[leftIndex];
                T right = this.HeapList[rightIndex];

                if (type == HeapType.Min)
                {
                    return (left.CompareTo(right) < 0) ? leftIndex : rightIndex;
                }
                else if (type == HeapType.Max)
                {
                    return (left.CompareTo(right) > 0) ? leftIndex : rightIndex;
                }
                else
                {
                    throw new ArgumentException("Expected value of Min or Max for heap type!", "type");
                }
            }
        }

        private bool CompareNodeWithParent(HeapType type, int iNode, int iParent)
        {
            int compareResult = this.HeapList[iNode].CompareTo(this.HeapList[iParent]);
            bool bResult = (type == HeapType.Min) ? compareResult < 0 : compareResult > 0;
            return bResult;
        }
    }

    public class MinArrayHeap<T> : ArrayHeap<T> where T : IComparable, IEquatable<T>
    {
        public MinArrayHeap(int size = 0) : base(HeapType.Min, size)
        {
        }

        public override void Insert(T node)
        {
            base.Insert(node, HeapType.Min);
        }

        public override void Delete(T node)
        {
            base.Delete(node, HeapType.Min);
        }

        public static void Test()
        {
            MinArrayHeap<int> heap = new MinArrayHeap<int>();

            heap.Insert(5);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(3);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(7);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(6);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(6);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(-1);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(4);

            Console.WriteLine(heap.ToString() + Environment.NewLine);

            heap.Delete(3);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Delete(4);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Delete(3);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
        }
    }

    public class MaxArrayHeap<T> : ArrayHeap<T> where T : IComparable, IEquatable<T>
    {
        public MaxArrayHeap(int size = 0) : base(HeapType.Max, size)
        {
            if (size <= 0)
            {
                base.HeapList = new List<T>();
            }
            else
            {
                base.HeapList = new List<T>(size);
            }
        }

        public override void Insert(T node)
        {
            base.Insert(node, HeapType.Max);
        }

        public override void Delete(T node)
        {
            base.Delete(node, HeapType.Max);
        }

        public static void Test()
        {
            MaxArrayHeap<int> heap = new MaxArrayHeap<int>();

            heap.Insert(10);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(-1);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(9);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(-2);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(-3);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(8);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(7);
            Console.WriteLine(heap.ToString() + Environment.NewLine);

            heap.Delete(-2);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.FixHeap(ArrayHeap<int>.HeapType.Max);
            Console.WriteLine(heap.ToString() + Environment.NewLine);

            heap.Delete(9);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Delete(10);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            Console.WriteLine("Root = " + heap.Peek());
            int test = heap.Pop();
            Console.WriteLine(heap.ToString() + Environment.NewLine);
        }
    }
}
