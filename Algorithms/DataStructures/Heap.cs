#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public abstract class BaseHeap<T> : BinayTree<T> where T : struct, IComparable
    {
        protected BaseHeap() : base() { }
        protected BaseHeap(TreeNode<T> root) : base(root) { }

        protected TreeNode<T> FindFirstLeaf()
        {
            if (null == base.Root)
            {
                return null;
            }
            else
            {
                Queue<TreeNode<T>> nodeQueue = new Queue<TreeNode<T>>();

                nodeQueue.Enqueue(base.Root);
                //Start from root and move down one level each iteration until we find a null leaf
                //Return the parent of the null leaf node

                while (nodeQueue.Count > 0)
                {
                    TreeNode<T> current = nodeQueue.Dequeue();
                    if (null == current)
                    {
                        throw new NullReferenceException("Expected current node to be non NULL!");
                    }

                    if (null != current.Left && null != current.Right)
                    {
                        nodeQueue.Enqueue(current.Left);
                        nodeQueue.Enqueue(current.Right);
                    }
                    else
                    {
                        return current;
                    }
                }

                throw new Exception("ERROR! Should have found a null leaf node by now! Should not have gotten to this code!");
            }
        }
    }

    public class MinHeap<T> : BaseHeap<T> where T : struct, IComparable
    {
        public MinHeap() : base() { }
        public MinHeap(TreeNode<T> root) : base(root) { }

        public static void TestMinHeap()
        {
            MinHeap<int> heap = new MinHeap<int>();

            TreeNode<int> node5 = new TreeNode<int>(5);
            TreeNode<int> node4 = new TreeNode<int>(4);
            TreeNode<int> node3 = new TreeNode<int>(3);
            TreeNode<int> node6 = new TreeNode<int>(6);
            TreeNode<int> node6a = new TreeNode<int>(6);
            TreeNode<int> nodeNeg1 = new TreeNode<int>(-1);
            TreeNode<int> node7 = new TreeNode<int>(7);

            heap.Insert(node5);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node3);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node7);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node6);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node6a);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(nodeNeg1);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node4);
            
            Console.WriteLine(heap.ToString());

            heap.Delete(node3);
            heap.Delete(node4);
            heap.Delete(node3);
        }

        public override void Insert(TreeNode<T> nodeToInsert)
        {
            if (nodeToInsert == null)
            {
                throw new ArgumentException("nodeToInsert", "Cannot be null!!");
            }

            //Add node to tree
            TreeNode<T> leafParent = base.FindFirstLeaf();
            if (null == leafParent)
            {
                //tree is empty. Add root node
                base.Root = nodeToInsert;
                return;
            }

            if (null == leafParent.Left)
            {
                leafParent.Left = nodeToInsert;
                nodeToInsert.Parent = leafParent;
            }
            else if (null == leafParent.Right)
            {
                leafParent.Right = nodeToInsert;
                nodeToInsert.Parent = leafParent;
            }
            else
            {
                throw new ApplicationException("Encountered logic error in FindFirstLeaf! Should not have reached this line!");
            }
            
            while (nodeToInsert.Parent != null && 
                   nodeToInsert.Data.CompareTo(nodeToInsert.Parent.Data) < 0)  //nodeToInsert < Parent
            {
                //Do swap
                //1. Copy off the parent to a temp variable
                TreeNode<T> temp = nodeToInsert.Parent.Copy();
                TreeNode<T> formerParent = nodeToInsert.Parent;
                //2. figure out whether the parent is the left or right child of the parent of the parent
                if (formerParent.Parent != null)
                {
                    if (formerParent.Parent.Left.Data.CompareTo(nodeToInsert.Parent.Data) == 0)
                    {
                        //repoint to the node to insert
                        formerParent.Parent.Left = nodeToInsert;
                    }
                    else
                    {
                        formerParent.Parent.Right = nodeToInsert;
                    }
                }

                if (formerParent.Equals(base.Root))
                {
                    base.Root = nodeToInsert;
                }

                if (formerParent.Left.Equals(nodeToInsert))
                {
                    formerParent.Left = nodeToInsert.Left;
                    formerParent.Right = nodeToInsert.Right;
                    formerParent.Parent = nodeToInsert;
                    nodeToInsert.Parent = temp.Parent;
                    nodeToInsert.Right = temp.Right;
                    nodeToInsert.Left = formerParent;                    
                }
                else
                {
                    formerParent.Left = nodeToInsert.Left;
                    formerParent.Right = nodeToInsert.Right;
                    formerParent.Parent = nodeToInsert;
                    nodeToInsert.Parent = temp.Parent;
                    nodeToInsert.Left = temp.Left;
                    nodeToInsert.Right = formerParent;
                }
            }
        }

        public override void Delete(TreeNode<T> nodeToDelete)
        {
            throw new NotImplementedException();
        }
    }

    public class MaxHeap<T> : BaseHeap<T> where T : struct, IComparable
    {
        public MaxHeap() : base() { }
        public MaxHeap(TreeNode<T> root) : base(root) { }

        public static void TestMaxHeap()
        {
            MaxHeap<int> heap = new MaxHeap<int>();

            TreeNode<int> node5 = new TreeNode<int>(5);
            TreeNode<int> node4 = new TreeNode<int>(4);
            TreeNode<int> node3 = new TreeNode<int>(3);
            TreeNode<int> node6 = new TreeNode<int>(6);
            TreeNode<int> node6a = new TreeNode<int>(6);
            TreeNode<int> nodeNeg1 = new TreeNode<int>(-1);
            TreeNode<int> node7 = new TreeNode<int>(7);

            heap.Insert(node5);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node3);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node7);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node6);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node6a);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(nodeNeg1);
            Console.WriteLine(heap.ToString() + Environment.NewLine);
            heap.Insert(node4);
            
            Console.WriteLine(heap.ToString());

            heap.Delete(node3);
            heap.Delete(node4);
            heap.Delete(node3);
        }

        public override void Insert(TreeNode<T> nodeToInsert)
        {
            if (nodeToInsert == null)
            {
                throw new ArgumentException("nodeToInsert", "Cannot be null!!");
            }

            //Add node to tree
            TreeNode<T> leafParent = base.FindFirstLeaf();
            if (null == leafParent)
            {
                //tree is empty. Add root node
                base.Root = nodeToInsert;
                return;
            }

            if (null == leafParent.Left)
            {
                leafParent.Left = nodeToInsert;
                nodeToInsert.Parent = leafParent;
            }
            else if (null == leafParent.Right)
            {
                leafParent.Right = nodeToInsert;
                nodeToInsert.Parent = leafParent;
            }
            else
            {
                throw new ApplicationException("Encountered logic error in FindFirstLeaf! Should not have reached this line!");
            }

            while (nodeToInsert.Parent != null &&
                   nodeToInsert.Data.CompareTo(nodeToInsert.Parent.Data) > 0)  //nodeToInsert > Parent
            {
                //Do swap
                //1. Copy off the parent to a temp variable
                TreeNode<T> temp = nodeToInsert.Parent.Copy();
                TreeNode<T> formerParent = nodeToInsert.Parent;
                //2. figure out whether the parent is the left or right child of the parent of the parent
                if (formerParent.Parent != null)
                {
                    if (formerParent.Parent.Left.Data.CompareTo(nodeToInsert.Parent.Data) == 0)
                    {
                        //repoint to the node to insert
                        formerParent.Parent.Left = nodeToInsert;
                    }
                    else
                    {
                        formerParent.Parent.Right = nodeToInsert;
                    }
                }

                if (formerParent.Equals(base.Root))
                {
                    base.Root = nodeToInsert;
                }

                if (formerParent.Left.Equals(nodeToInsert))
                {
                    formerParent.Left = nodeToInsert.Left;
                    formerParent.Right = nodeToInsert.Right;
                    formerParent.Parent = nodeToInsert;
                    nodeToInsert.Parent = temp.Parent;
                    nodeToInsert.Right = temp.Right;
                    nodeToInsert.Left = formerParent;
                }
                else
                {
                    formerParent.Left = nodeToInsert.Left;
                    formerParent.Right = nodeToInsert.Right;
                    formerParent.Parent = nodeToInsert;
                    nodeToInsert.Parent = temp.Parent;
                    nodeToInsert.Left = temp.Left;
                    nodeToInsert.Right = formerParent;
                }
            }
        }

        public override void Delete(TreeNode<T> nodeToDelete)
        {
            throw new NotImplementedException();
        }
    }
}
