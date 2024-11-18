using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using ST10144453_PROG7312.MVVM.Model;

namespace ST10144453_PROG7312.MVVM.View.Visualizations
{
    public class TreeVisualizationControl : BaseVisualizationControl
    {
        private const double VERTICAL_SPACING = 80;
        private const double HORIZONTAL_SPACING = 50;
        private TreeNode root;

        public override void UpdateVisualization(IEnumerable<ServiceRequestModel> requests)
        {
            visualizationCanvas.Children.Clear();
            if (!requests.Any()) return;

            // Build tree structure
            BuildTree(requests);
            
            // Calculate positions
            CalculateNodePositions(root, 0, 0);
            
            // Draw the tree
            DrawTree();
        }

        private void BuildTree(IEnumerable<ServiceRequestModel> requests)
        {
            var requestsList = requests.ToList();
            if (!requestsList.Any()) return;

            // Create nodes
            var nodes = requestsList.Select(r => new TreeNode(r)).ToList();
            
            // Sort by priority or date
            nodes = nodes.OrderBy(n => n.Request.RequestDate).ToList();
            
            // Set root as the first node
            root = nodes[0];
            
            // Build relationships based on dates and priorities
            for (int i = 1; i < nodes.Count; i++)
            {
                var currentNode = nodes[i];
                var parentNode = FindBestParent(root, currentNode);
                parentNode.Children.Add(currentNode);
                currentNode.Parent = parentNode;
            }
        }

        private TreeNode FindBestParent(TreeNode current, TreeNode newNode)
        {
            // Simple logic: attach to the nearest parent with capacity
            if (current.Children.Count < 3) // Limit children to 3 for better visualization
                return current;

            foreach (var child in current.Children)
            {
                var result = FindBestParent(child, newNode);
                if (result != null)
                    return result;
            }

            return current;
        }

        private void CalculateNodePositions(TreeNode node, double x, double level)
        {
            if (node == null) return;

            node.Level = level;
            
            // Calculate children positions first to determine center
            double totalWidth = 0;
            foreach (var child in node.Children)
            {
                CalculateNodePositions(child, totalWidth, level + 1);
                totalWidth += child.Width + HORIZONTAL_SPACING;
            }

            // Center the node above its children
            if (node.Children.Any())
            {
                node.Position = new Point(
                    (node.Children.First().Position.X + node.Children.Last().Position.X) / 2,
                    level * (node.Height + VERTICAL_SPACING)
                );
            }
            else
            {
                node.Position = new Point(x, level * (node.Height + VERTICAL_SPACING));
            }
        }

        private void DrawTree()
        {
            if (root == null) return;

            var nodesToDraw = new Queue<TreeNode>();
            nodesToDraw.Enqueue(root);

            while (nodesToDraw.Count > 0)
            {
                var node = nodesToDraw.Dequeue();
                
                // Draw connections to children
                foreach (var child in node.Children)
                {
                    DrawConnection(node, child);
                    nodesToDraw.Enqueue(child);
                }

                // Draw node
                DrawNode(node);
            }
        }

        private void DrawNode(TreeNode node)
        {
            var border = new Border
            {
                Width = node.Width,
                Height = node.Height,
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.DarkGray),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Effect = new System.Windows.Media.Effects.DropShadowEffect
                {
                    BlurRadius = 5,
                    ShadowDepth = 2,
                    Opacity = 0.3
                }
            };

            var panel = new StackPanel { Margin = new Thickness(5) };
            
            panel.Children.Add(new TextBlock 
            { 
                Text = node.Request.Category,
                FontWeight = FontWeights.Bold,
                TextTrimming = TextTrimming.CharacterEllipsis
            });
            
            panel.Children.Add(new TextBlock 
            { 
                Text = node.Request.Status,
                Foreground = new SolidColorBrush(Colors.Gray),
                TextTrimming = TextTrimming.CharacterEllipsis
            });

            border.Child = panel;

            Canvas.SetLeft(border, node.Position.X - node.Width / 2);
            Canvas.SetTop(border, node.Position.Y);
            visualizationCanvas.Children.Add(border);
        }

        private void DrawConnection(TreeNode parent, TreeNode child)
        {
            var line = new Line
            {
                X1 = parent.Position.X,
                Y1 = parent.Position.Y + parent.Height,
                X2 = child.Position.X,
                Y2 = child.Position.Y,
                Stroke = new SolidColorBrush(Colors.DarkGray),
                StrokeThickness = 1.5
            };

            visualizationCanvas.Children.Add(line);
        }

        public override string GetDescription()
        {
            return "Hierarchical tree visualization of service requests, organized by priority and submission date.";
        }
    }
}
