using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View.Visualizations
{
    public class TreeNode
    {
        public ServiceRequestModel Request { get; set; }
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();
        public TreeNode Parent { get; set; }
        public Point Position { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Level { get; set; }

        public TreeNode(ServiceRequestModel request)
        {
            Request = request;
            Width = 150;  // Default width
            Height = 60;  // Default height
        }
    }
}