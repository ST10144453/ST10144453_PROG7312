using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestGraphHelper
    {
        public class GraphNode
        {
            public string Id { get; set; }
            public ServiceRequestModel Data { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public string Color { get; set; }
            public List<string> ConnectedNodes { get; set; }

            public GraphNode(ServiceRequestModel data)
            {
                Id = data.RequestID.ToString();
                Data = data;
                ConnectedNodes = new List<string>();
                Color = GetStatusColor(data.Status);
            }

            private string GetStatusColor(string status)
            {
                switch (status)
                {
                    case "Completed":
                        return "#4CAF50";
                    case "In Progress":
                        return "#FFC107";
                    case "Pending":
                        return "#F44336";
                    case "Rejected":
                        return "#9E9E9E";
                    default:
                        return "#9E9E9E";
                }
            }
        }

        public class GraphData
        {
            public List<GraphNode> Nodes { get; set; }
            public List<(string Source, string Target)> Edges { get; set; }

            public GraphData()
            {
                Nodes = new List<GraphNode>();
                Edges = new List<(string Source, string Target)>();
            }
        }

        public static GraphData CreateServiceRequestGraph(IEnumerable<ServiceRequestModel> requests, double width, double height)
        {
            var graph = new GraphData();
            var processedNodes = new Dictionary<string, GraphNode>();

            // Create nodes
            foreach (var request in requests)
            {
                var node = new GraphNode(request);
                graph.Nodes.Add(node);
                processedNodes[node.Id] = node;
            }

            // Create edges based on relationships
            foreach (var request in requests)
            {
                var sourceNode = processedNodes[request.RequestID.ToString()];

                // Connect requests with same category
                var relatedRequests = requests.Where(r =>
                    r.RequestID != request.RequestID &&
                    (r.Category == request.Category || r.CreatedBy == request.CreatedBy));

                foreach (var related in relatedRequests)
                {
                    graph.Edges.Add((sourceNode.Id, related.RequestID.ToString()));
                }
            }

            // Calculate node positions using force-directed layout
            CalculateNodePositions(graph, width, height);

            return graph;
        }

        private static void CalculateNodePositions(GraphData graph, double width, double height)
        {
            const int iterations = 100;
            const double k = 50;
            const double gravity = 0.1;
            const double nodeRadius = 40; // Increased from 20
            const double padding = 60;  // Added padding

            Random random = new Random();

            // Initialize random positions within boundaries
            foreach (var node in graph.Nodes)
            {
                node.X = random.NextDouble() * (width - 2 * nodeRadius) + nodeRadius;
                node.Y = random.NextDouble() * (height - 2 * nodeRadius) + nodeRadius;
            }

            // Iterate to find optimal positions
            for (int i = 0; i < iterations; i++)
            {
                foreach (var node in graph.Nodes)
                {
                    double fx = 0, fy = 0;

                    // Apply repulsive forces
                    foreach (var other in graph.Nodes)
                    {
                        if (node != other)
                        {
                            double dx = node.X - other.X;
                            double dy = node.Y - other.Y;
                            double distance = Math.Sqrt(dx * dx + dy * dy);

                            if (distance > 0)
                            {
                                double force = k * k / distance;
                                fx += (dx / distance) * force;
                                fy += (dy / distance) * force;
                            }
                        }
                    }

                    // Apply attractive forces for connected nodes
                    foreach (var edge in graph.Edges.Where(e => e.Source == node.Id || e.Target == node.Id))
                    {
                        var other = graph.Nodes.First(n =>
                            n.Id == (edge.Source == node.Id ? edge.Target : edge.Source));

                        double dx = node.X - other.X;
                        double dy = node.Y - other.Y;
                        double distance = Math.Sqrt(dx * dx + dy * dy);

                        if (distance > 0)
                        {
                            fx -= (dx / distance) * distance / k;
                            fy -= (dy / distance) * distance / k;
                        }
                    }

                    // Apply gravity towards center
                    fx -= gravity * (node.X - width / 2);
                    fy -= gravity * (node.Y - height / 2);

                    // Update position with boundary checks
                    node.X = Math.Max(nodeRadius + padding, Math.Min(width - nodeRadius - padding,
                        node.X + Math.Max(-5, Math.Min(5, fx))));

                    node.Y = Math.Max(nodeRadius + padding, Math.Min(height - nodeRadius - padding,
                        node.Y + Math.Max(-5, Math.Min(5, fy))));
                }
            }
        }
    }
}
