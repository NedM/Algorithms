using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Algorithms.Types
{
    public class TreeNode<T> where T : struct, IComparable
    {
        public TreeNode() : this(new T()) { }

        public TreeNode(T data, TreeNode<T> parent = null, TreeNode<T> left = null, TreeNode<T> right = null)
        {
            this.Left = left;
            this.Right = right;
            this.Parent = parent;
            this.Data = data;
        }

        public T Data { get; set; }
        public TreeNode<T> Left { get; set; }
        public TreeNode<T> Right { get; set; }
        public TreeNode<T> Parent { get; set; }

        public TreeNode<T> Copy()
        {
            return new TreeNode<T>(this.Data, this.Parent, this.Left, this.Right);
        }

        public override bool Equals(object obj)
        {
            if (obj is TreeNode<T>)
            {
                bool areEqual = true;
                TreeNode<T> treeObj = obj as TreeNode<T>;

                areEqual &= this.Data.Equals(treeObj.Data);

                if (this.Parent != null && treeObj.Parent != null)
                {
                    areEqual &= this.Parent.Data.Equals(treeObj.Parent.Data);
                }
                else if ((this.Parent != null && treeObj.Parent == null) ||
                        (this.Parent == null && treeObj.Parent != null))
                {
                    areEqual = false;
                }

                if (this.Left != null && treeObj.Left != null)
                {
                    areEqual &= this.Left.Data.Equals(treeObj.Left.Data);
                }
                else if ((this.Left != null && treeObj.Left == null) ||
                        (this.Left == null && treeObj.Left != null))
                {
                    areEqual = false;
                }

                if (this.Right != null && treeObj.Right != null)
                {
                    areEqual &= this.Right.Data.Equals(treeObj.Right.Data);
                }
                else if ((this.Right != null && treeObj.Right == null) ||
                        (this.Right == null && treeObj.Right != null))
                {
                    areEqual = false;
                }


                return areEqual;
            }
            else
            {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode()
        {
            return this.Data.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Data: {1}{0}Parent data: {2}{0}Left data: {3}{0}Right data: {4}{0}",
                Environment.NewLine, this.Data.ToString(), this.Parent.Data.ToString(), this.Left.Data.ToString(), this.Right.Data.ToString());
        }
    }
}
