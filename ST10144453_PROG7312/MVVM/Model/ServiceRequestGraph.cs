using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestGraph
    {
        private Dictionary<string, ServiceRequestGraphNode> nodes;

        public ServiceRequestGraph()
        {
            nodes = new Dictionary<string, ServiceRequestGraphNode>();
        }

        public void AddNode(ServiceRequestModel request)
        {
            if (!nodes.ContainsKey(request.RequestID.ToString()))
            {
                nodes.Add(request.RequestID.ToString(), new ServiceRequestGraphNode(request));
            }
        }

        public void AddEdge(string fromId, string toId)
        {
            if (nodes.ContainsKey(fromId) && nodes.ContainsKey(toId))
            {
                nodes[fromId].Adjacent[toId] = nodes[toId];
                nodes[toId].Adjacent[fromId] = nodes[fromId]; // For undirected graph
            }
        }

        public IEnumerable<ServiceRequestModel> BreadthFirstSearch(string startId)
        {
            if (!nodes.ContainsKey(startId)) yield break;

            var visited = new HashSet<string>();
            var queue = new Queue<ServiceRequestGraphNode>();

            queue.Enqueue(nodes[startId]);
            visited.Add(startId);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current.Data;

                foreach (var adjacent in current.Adjacent.Values)
                {
                    string adjacentId = adjacent.Data.RequestID.ToString();
                    if (!visited.Contains(adjacentId))
                    {
                        visited.Add(adjacentId);
                        queue.Enqueue(adjacent);
                    }
                }
            }
        }

        public IEnumerable<ServiceRequestModel> DepthFirstSearch(string startId)
        {
            if (!nodes.ContainsKey(startId)) yield break;

            var visited = new HashSet<string>();
            var stack = new Stack<ServiceRequestGraphNode>();

            stack.Push(nodes[startId]);
            visited.Add(startId);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current.Data;

                foreach (var adjacent in current.Adjacent.Values)
                {
                    string adjacentId = adjacent.Data.RequestID.ToString();
                    if (!visited.Contains(adjacentId))
                    {
                        visited.Add(adjacentId);
                        stack.Push(adjacent);
                    }
                }
            }
        }

        public void BuildRelationships(IEnumerable<ServiceRequestModel> requests)
        {
            // Clear existing nodes
            nodes.Clear();

            // Add all nodes first
            foreach (var request in requests)
            {
                AddNode(request);
            }

            // Create edges based on meaningful relationships
            var requestList = requests.ToList();
            for (int i = 0; i < requestList.Count; i++)
            {
                var current = requestList[i];
                var currentId = current.RequestID.ToString();

                // Connect requests with same category
                var sameCategory = requestList
                    .Where(r => r.RequestID != current.RequestID && r.Category == current.Category)
                    .Take(2); // Limit connections to prevent over-complexity

                foreach (var related in sameCategory)
                {
                    AddEdge(currentId, related.RequestID.ToString());
                }

                // Connect requests by same user
                var sameUser = requestList
                    .Where(r => r.RequestID != current.RequestID && r.CreatedBy == current.CreatedBy)
                    .Take(2);

                foreach (var related in sameUser)
                {
                    AddEdge(currentId, related.RequestID.ToString());
                }

                // Connect requests with similar dates (within 2 days)
                var similarDates = requestList
                    .Where(r => r.RequestID != current.RequestID &&
                               Math.Abs((r.RequestDate - current.RequestDate).TotalDays) <= 2)
                    .Take(2);

                foreach (var related in similarDates)
                {
                    AddEdge(currentId, related.RequestID.ToString());
                }
            }
        }

        public Dictionary<string, ServiceRequestGraphNode> GetNodes() => nodes;
    }
}
