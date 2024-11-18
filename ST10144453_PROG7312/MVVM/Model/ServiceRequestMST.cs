using ST10144453_PROG7312.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestMST
    {
        private readonly Dictionary<Guid, List<(Guid TargetId, double Weight)>> adjacencyList;
        private readonly Dictionary<Guid, ServiceRequestModel> requests;

        public ServiceRequestMST()
        {
            adjacencyList = new Dictionary<Guid, List<(Guid TargetId, double Weight)>>();
            requests = new Dictionary<Guid, ServiceRequestModel>();
        }

        public void AddRequest(ServiceRequestModel request)
        {
            requests[request.RequestID] = request;
            if (!adjacencyList.ContainsKey(request.RequestID))
            {
                adjacencyList[request.RequestID] = new List<(Guid TargetId, double Weight)>();
            }
        }

        public void AddEdge(Guid sourceId, Guid targetId, double weight)
        {
            if (!adjacencyList.ContainsKey(sourceId))
            {
                adjacencyList[sourceId] = new List<(Guid TargetId, double Weight)>();
            }
            adjacencyList[sourceId].Add((targetId, weight));
        }

        public List<(Guid Source, Guid Target, double Weight)> FindMinimumSpanningTree()
        {
            if (!requests.Any()) return new List<(Guid, Guid, double)>();

            var mst = new List<(Guid, Guid, double)>();
            var visited = new HashSet<Guid>();
            
            var pq = new PriorityQueue<(Guid Source, Guid Target, double Weight)>();

            // Start with first request
            var startId = requests.Keys.First();
            visited.Add(startId);

            // Add all edges from start vertex
            if (adjacencyList.ContainsKey(startId))
            {
                foreach (var edge in adjacencyList[startId])
                {
                    // Convert weight to negative priority so lower weights have higher priority
                    int priority = -(int)(edge.Weight * 1000); // Multiply by 1000 to preserve decimal precision
                    pq.Enqueue((startId, edge.TargetId, edge.Weight), priority);
                }
            }

            while (!pq.IsEmpty && visited.Count < requests.Count)
            {
                var current = pq.Dequeue();
                Guid source = current.Source;
                Guid target = current.Target;
                double weight = current.Weight;

                if (visited.Contains(target)) continue;

                visited.Add(target);
                mst.Add((source, target, weight));

                if (adjacencyList.ContainsKey(target))
                {
                    foreach (var edge in adjacencyList[target])
                    {
                        if (!visited.Contains(edge.TargetId))
                        {
                            // Convert weight to negative priority so lower weights have higher priority
                            int priority = -(int)(edge.Weight * 1000); // Multiply by 1000 to preserve decimal precision
                            pq.Enqueue((target, edge.TargetId, edge.Weight), priority);
                        }
                    }
                }
            }

            return mst;
        }

        public Dictionary<Guid, ServiceRequestModel> GetRequests()
        {
            return requests;
        }
    }
}
