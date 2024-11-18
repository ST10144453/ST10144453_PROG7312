using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ST10144453_PROG7312.MVVM.Model;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class ServiceRequestGraphControl : UserControl
    {
        public static readonly DependencyProperty RequestsProperty =
            DependencyProperty.Register(
                "Requests", 
                typeof(IEnumerable<ServiceRequestModel>), 
                typeof(ServiceRequestGraphControl), 
                new PropertyMetadata(null, OnDataChanged));

        public IEnumerable<ServiceRequestModel> Requests
        {
            get => (IEnumerable<ServiceRequestModel>)GetValue(RequestsProperty);
            set => SetValue(RequestsProperty, value);
        }

        private Dictionary<string, Ellipse> nodeShapes;
        private Dictionary<string, Line> edgeShapes;
        private ServiceRequestGraph graph;

        public ServiceRequestGraphControl()
        {
            InitializeComponent();
            nodeShapes = new Dictionary<string, Ellipse>();
            edgeShapes = new Dictionary<string, Line>();
            
            // Initial size for testing
            Width = 800;
            Height = 600;
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ServiceRequestGraphControl control)
            {
                control.UpdateGraph();
            }
        }

        private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateGraph();
        }

        public void UpdateGraph()
        {
            GraphCanvas.Children.Clear();
            if (Requests == null) return;

            // Use the GraphHelper with canvas dimensions
            var graphData = ServiceRequestGraphHelper.CreateServiceRequestGraph(
                Requests,
                GraphCanvas.ActualWidth > 0 ? GraphCanvas.ActualWidth : Width,
                GraphCanvas.ActualHeight > 0 ? GraphCanvas.ActualHeight : Height
            );
            
            // Draw edges first (so they appear behind nodes)
            foreach (var edge in graphData.Edges)
            {
                var sourceNode = graphData.Nodes.First(n => n.Id == edge.Source);
                var targetNode = graphData.Nodes.First(n => n.Id == edge.Target);
                
                DrawEdge(new Point(sourceNode.X, sourceNode.Y), 
                        new Point(targetNode.X, targetNode.Y));
            }

            // Draw nodes
            foreach (var node in graphData.Nodes)
            {
                DrawNode(new Point(node.X, node.Y), node.Data);
            }
        }

        private Dictionary<string, Point> CalculateForceDirectedLayout(Dictionary<string, ServiceRequestGraphNode> nodes)
        {
            // Force-directed layout implementation
            var positions = new Dictionary<string, Point>();
            var forces = new Dictionary<string, (double fx, double fy)>();
            
            // Initialize random positions
            Random rand = new Random();
            foreach (var node in nodes.Values)
            {
                positions[node.Data.RequestID.ToString()] = new Point(
                    rand.NextDouble() * GraphCanvas.ActualWidth,
                    rand.NextDouble() * GraphCanvas.ActualHeight
                );
            }

            // Iterate to find stable positions
            const int iterations = 50;
            const double k = 50; // Spring constant
            const double gravity = 0.1;

            for (int iter = 0; iter < iterations; iter++)
            {
                // Reset forces
                forces.Clear();
                foreach (var node in nodes.Values)
                {
                    forces[node.Data.RequestID.ToString()] = (0, 0);
                }

                // Calculate forces
                foreach (var node1 in nodes.Values)
                {
                    var pos1 = positions[node1.Data.RequestID.ToString()];
                    
                    foreach (var node2 in nodes.Values)
                    {
                        if (node1 == node2) continue;
                        
                        var pos2 = positions[node2.Data.RequestID.ToString()];
                        var dx = pos2.X - pos1.X;
                        var dy = pos2.Y - pos1.Y;
                        var distance = Math.Sqrt(dx * dx + dy * dy);
                        
                        if (distance == 0) continue;

                        // Repulsive force between all nodes
                        var repulsion = -k / (distance * distance);
                        var fx = repulsion * dx / distance;
                        var fy = repulsion * dy / distance;

                        // Attractive force between connected nodes
                        if (node1.Adjacent.ContainsKey(node2.Data.RequestID.ToString()))
                        {
                            var attraction = Math.Log(distance / k) * k;
                            fx += attraction * dx / distance;
                            fy += attraction * dy / distance;
                        }

                        var (currentFx, currentFy) = forces[node1.Data.RequestID.ToString()];
                        forces[node1.Data.RequestID.ToString()] = (currentFx + fx, currentFy + fy);
                    }
                }

                // Apply forces
                foreach (var node in nodes.Values)
                {
                    var id = node.Data.RequestID.ToString();
                    var (fx, fy) = forces[id];
                    var pos = positions[id];

                    // Add gravity towards center
                    fx += gravity * (GraphCanvas.ActualWidth/2 - pos.X);
                    fy += gravity * (GraphCanvas.ActualHeight/2 - pos.Y);

                    // Update position
                    positions[id] = new Point(
                        pos.X + Math.Max(-5, Math.Min(5, fx)),
                        pos.Y + Math.Max(-5, Math.Min(5, fy))
                    );
                }
            }

            return positions;
        }

        private void DrawNode(Point position, ServiceRequestModel request)
        {
            var ellipse = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = GetStatusBrush(request.Status),
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 1
            };

            Canvas.SetLeft(ellipse, position.X - 20);
            Canvas.SetTop(ellipse, position.Y - 20);

            // Add hover effects
            ellipse.MouseEnter += (s, e) => ShowTooltip(request, e);
            ellipse.MouseLeave += (s, e) => HideTooltip();
            ellipse.MouseMove += (s, e) => UpdateTooltipPosition(e);

            GraphCanvas.Children.Add(ellipse);

            // Add label
            var textBlock = new TextBlock
            {
                Text = request.Category,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Width = 80,
                FontSize = 10
            };

            Canvas.SetLeft(textBlock, position.X - 40);
            Canvas.SetTop(textBlock, position.Y + 20);
            GraphCanvas.Children.Add(textBlock);
        }

        private void DrawEdge(Point start, Point end)
        {
            var line = new Line
            {
                X1 = start.X,
                Y1 = start.Y,
                X2 = end.X,
                Y2 = end.Y,
                Stroke = new SolidColorBrush(Colors.Gray),
                StrokeThickness = 1,
                Opacity = 0.5
            };

            GraphCanvas.Children.Add(line);
        }

        private Brush GetStatusBrush(string status)
        {
            switch (status)
            {
                case "Completed":
                    return new SolidColorBrush(Color.FromRgb(76, 175, 80));  // Green
                case "In Progress":
                    return new SolidColorBrush(Color.FromRgb(255, 193, 7)); // Yellow
                case "Pending":
                    return new SolidColorBrush(Color.FromRgb(244, 67, 54));    // Red
                default:
                    return new SolidColorBrush(Color.FromRgb(158, 158, 158));  // Grey
            }
        }

      
        private void ShowTooltip(ServiceRequestModel request, MouseEventArgs e)
        {
            TooltipTitle.Text = request.Category;
            TooltipDetails.Text = $"Status: {request.Status}\n" +
                                 $"Created: {request.RequestDate:d}\n" +
                                 $"By: {request.CreatedBy}\n" +
                                 $"Description: {request.Description}";

            TooltipBorder.Visibility = Visibility.Visible;
            UpdateTooltipPosition(e);
        }

        private void HideTooltip()
        {
            TooltipBorder.Visibility = Visibility.Collapsed;
        }

        private void UpdateTooltipPosition(MouseEventArgs e)
        {
            var point = e.GetPosition(GraphCanvas);
            
            double tooltipX = point.X + 10;
            double tooltipY = point.Y + 10;

            // Ensure tooltip stays within canvas bounds
            if (tooltipX + TooltipBorder.ActualWidth > GraphCanvas.ActualWidth)
            {
                tooltipX = GraphCanvas.ActualWidth - TooltipBorder.ActualWidth;
            }
            if (tooltipY + TooltipBorder.ActualHeight > GraphCanvas.ActualHeight)
            {
                tooltipY = GraphCanvas.ActualHeight - TooltipBorder.ActualHeight;
            }

            Canvas.SetLeft(TooltipBorder, tooltipX);
            Canvas.SetTop(TooltipBorder, tooltipY);
        }
    }
}
