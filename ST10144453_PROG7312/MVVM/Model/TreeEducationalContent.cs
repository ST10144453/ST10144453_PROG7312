using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public static class TreeEducationalContent
    {
        public static string GetDetailedDescription(TreeType treeType)
        {
            switch (treeType)
            {
                case TreeType.Basic:
                    return GetBasicTreeDescription();
                case TreeType.BinarySearch:
                    return GetBSTDescription();
                case TreeType.AVL:
                    return GetAVLDescription();
                case TreeType.RedBlack:
                    return GetRedBlackDescription();
                default:
                    return string.Empty;
            }
        }

        public static string GetTimeComplexityInfo(TreeType treeType)
        {
            switch (treeType)
            {
                case TreeType.Basic:
                    return "Insert: O(1), Search: O(n), Delete: O(n)";
                case TreeType.BinarySearch:
                    return "Insert: O(h), Search: O(h), Delete: O(h) where h is height";
                case TreeType.AVL:
                    return "All operations: O(log n) - guaranteed";
                case TreeType.RedBlack:
                    return "All operations: O(log n) - guaranteed";
                default:
                    return string.Empty;
            }
        }

        private static string GetBasicTreeDescription() =>
            "A basic tree is a hierarchical data structure where each node can have " +
            "multiple children. It's the simplest form of tree structure and is often " +
            "used for representing hierarchical relationships.";

        private static string GetBSTDescription() =>
            "A binary search tree is a binary tree where for each node, all elements " +
            "in the left subtree are less than the node, and all elements in the right " +
            "subtree are greater than the node.";

        private static string GetAVLDescription() =>
            "An AVL tree is a self-balancing binary search tree where the heights of " +
            "the two child subtrees of any node differ by at most one. This ensures " +
            "O(log n) operations.";

        private static string GetRedBlackDescription() =>
            "A red-black tree is a self-balancing binary search tree with one extra " +
            "bit of storage per node: its color, which can be either red or black. " +
            "The tree maintains balance through a set of properties that ensure no " +
            "path is more than twice as long as any other.";
    }
}
