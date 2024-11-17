using ST10144453_PROG7312.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public static class ServiceRequestTreeFactory
    {
        public static IServiceRequestTree CreateTree(TreeType type)
        {
            switch (type)
            {
                case TreeType.Basic:
                    return new BasicServiceRequestTree();
                case TreeType.BinarySearch:
                    return new BinarySearchServiceRequestTree();
                case TreeType.AVL:
                    return new ServiceRequestAVLTree();
                case TreeType.RedBlack:
                    return new RedBlackServiceRequestTree();
                default:
                    return new BasicServiceRequestTree();
            }
        }
    }
}
