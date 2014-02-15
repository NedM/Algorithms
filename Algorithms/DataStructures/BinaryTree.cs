#undef DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Algorithms.Types;

namespace Algorithms.DataStructures
{
    public class BinayTree<T> where T : struct, IComparable
    {
        public static void TestBinaryTree()
        {
            BinayTree<int> biTree = new BinayTree<int>();

            TreeNode<int> node5 = new TreeNode<int>(5);
            TreeNode<int> node4 = new TreeNode<int>(4);
            TreeNode<int> node3 = new TreeNode<int>(3);
            TreeNode<int> node6 = new TreeNode<int>(6);
            TreeNode<int> node6a = new TreeNode<int>(6);
            TreeNode<int> nodeNeg1 = new TreeNode<int>(-1);
            TreeNode<int> node7 = new TreeNode<int>(7);

            biTree.Insert(node5);
            biTree.Insert(node4);
            biTree.Insert(node3);
            biTree.Insert(node6);
            biTree.Insert(node6a);
            biTree.Insert(nodeNeg1);
            biTree.Insert(node7);
            biTree.Insert(new TreeNode<int>(1));

            Console.WriteLine(biTree.ToString());
        }

        public BinayTree() : this(null) { }

        public BinayTree(TreeNode<T> root)
        {
            this.Root = root;
        }

        public TreeNode<T> Root { get; set; }

        public virtual void Insert(TreeNode<T> nodeToInsert)
        {
            if (null == this.Root)
            {
                this.Root = nodeToInsert;
            }
            else
            {
                this.Insert(nodeToInsert, this.Root);
            }
        }

        private void Insert(TreeNode<T> nodeToInsert, TreeNode<T> startingNode)
        {
            if (null == nodeToInsert)
            {
                throw new ArgumentException("nodeToInsert", "cannot be NULL!");
            }

            if (null == startingNode)
            {
                throw new ArgumentException("startingNode", "cannot be NULL!");
            }

            if (nodeToInsert.Data.CompareTo(startingNode.Data) <= 0)
            {
                //Go left
                if (null == startingNode.Left)
                {
                    startingNode.Left = nodeToInsert;
                    nodeToInsert.Parent = startingNode;
                }
                else
                {
                    this.Insert(nodeToInsert, startingNode.Left);
                }
            }
            else
            {
                //Go right
                if (null == startingNode.Right)
                {
                    startingNode.Right = nodeToInsert;
                    nodeToInsert.Parent = startingNode;
                }
                else
                {
                    this.Insert(nodeToInsert, startingNode.Right);
                }
            }
        }

        public virtual void Delete(TreeNode<T> nodeToDelete)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            Queue<TreeNode<T>> nodeQueue = new Queue<TreeNode<T>>();

            if (this.Root == null)
            {
                return "The tree is empty. Root is null";
            }

            nodeQueue.Enqueue(this.Root);

            while (nodeQueue.Count > 0)
            {
                TreeNode<T> node = nodeQueue.Dequeue();

#if DEBUG
                Console.WriteLine("Dequeing node with data: " + node.Data.ToString());
#endif

                string leftString = (node.Left == null) ? "NULL" : node.Left.Data.ToString();
                string rightString = (node.Right == null) ? "NULL" : node.Right.Data.ToString();
                sb.AppendFormat("[{0}, ({1}, {2})]{3}", node.Data.ToString(), leftString, rightString, Environment.NewLine);

                if (node.Left != null)
                {
#if DEBUG
                    Console.WriteLine("Enqueuing node with data: " + node.Left.Data.ToString());
#endif
                    nodeQueue.Enqueue(node.Left);
                }

                if (node.Right != null)
                {
#if DEBUG
                    Console.WriteLine("Enqueuing node with data: " + node.Right.Data.ToString());
#endif
                    nodeQueue.Enqueue(node.Right);
                }
            }

            return sb.ToString();
        }
    }
}
