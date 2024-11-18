using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestTraversal
    {
        private readonly Dictionary<Guid, List<(Guid TargetId, double Weight)>> adjacencyList;
        private readonly Dictionary<Guid, ServiceRequestModel> requests;

        public ServiceRequestTraversal()
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

        public void AddConnection(Guid sourceId, Guid targetId, double weight)
        {
            if (!adjacencyList.ContainsKey(sourceId))
            {
                adjacencyList[sourceId] = new List<(Guid TargetId, double Weight)>();
            }
            adjacencyList[sourceId].Add((targetId, weight));
        }

        public IEnumerable<ServiceRequestModel> BreadthFirstSearch(Guid startRequestId)
        {
            if (!requests.ContainsKey(startRequestId))
                return Enumerable.Empty<ServiceRequestModel>();

            var visited = new HashSet<Guid>();
            var queue = new Queue<Guid>();
            var result = new List<ServiceRequestModel>();

            queue.Enqueue(startRequestId);
            visited.Add(startRequestId);

            while (queue.Count > 0)
            {
                var currentId = queue.Dequeue();
                var request = requests[currentId];
                result.Add(request);

                if (adjacencyList.ContainsKey(currentId))
                {
                    foreach (var (TargetId, _) in adjacencyList[currentId])
                    {
                        if (!visited.Contains(TargetId))
                        {
                            visited.Add(TargetId);
                            queue.Enqueue(TargetId);
                        }
                    }
                }
            }

            return result;
        }

        public IEnumerable<ServiceRequestModel> DepthFirstSearch(Guid startRequestId)
        {
            if (!requests.ContainsKey(startRequestId))
                return Enumerable.Empty<ServiceRequestModel>();

            var visited = new HashSet<Guid>();
            var result = new List<ServiceRequestModel>();
            DFSRecursive(startRequestId, visited, result);
            return result;
        }

        private void DFSRecursive(Guid currentId, HashSet<Guid> visited, List<ServiceRequestModel> result)
        {
            visited.Add(currentId);
            result.Add(requests[currentId]);

            if (adjacencyList.ContainsKey(currentId))
            {
                foreach (var (TargetId, _) in adjacencyList[currentId])
                {
                    if (!visited.Contains(TargetId))
                    {
                        DFSRecursive(TargetId, visited, result);
                    }
                }
            }
        }
    }
}
