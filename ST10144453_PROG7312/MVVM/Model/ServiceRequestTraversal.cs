using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestTraversal : IServiceRequestTraversal
    {
        private readonly Dictionary<Guid, List<(Guid TargetId, double Weight)>> adjacencyList;
        private readonly Dictionary<Guid, ServiceRequestModel> requests;

        public ServiceRequestTraversal()
        {
            adjacencyList = new Dictionary<Guid, List<(Guid TargetId, double Weight)>>();
            requests = new Dictionary<Guid, ServiceRequestModel>();
        }

        public IEnumerable<ServiceRequestModel> GetRelatedRequests(
            ServiceRequestModel selectedRequest, 
            bool isStaffUser,
            int maxDepth = 2)
        {
            if (selectedRequest == null) return Enumerable.Empty<ServiceRequestModel>();

            var relatedRequests = new HashSet<ServiceRequestModel>();
            var visited = new HashSet<Guid>();
            var queue = new Queue<(ServiceRequestModel Request, int Depth)>();

            queue.Enqueue((selectedRequest, 0));
            visited.Add(selectedRequest.RequestID);

            while (queue.Count > 0)
            {
                var (currentRequest, depth) = queue.Dequeue();

                // Add to related requests if user has permission
                if (isStaffUser || currentRequest.CreatedBy == selectedRequest.CreatedBy)
                {
                    relatedRequests.Add(currentRequest);
                }

                if (depth >= maxDepth) continue;

                // Find related requests based on various criteria
                var related = FindRelatedRequests(currentRequest)
                    .Where(r => !visited.Contains(r.RequestID));

                foreach (var request in related)
                {
                    visited.Add(request.RequestID);
                    queue.Enqueue((request, depth + 1));
                }
            }

            return relatedRequests.Where(r => r.RequestID != selectedRequest.RequestID);
        }

        private IEnumerable<ServiceRequestModel> FindRelatedRequests(ServiceRequestModel request)
        {
            return requests.Values.Where(r =>
                r.RequestID != request.RequestID &&
                (r.Category == request.Category ||
                 r.CreatedBy == request.CreatedBy ||
                 Math.Abs((r.RequestDate - request.RequestDate).TotalDays) <= 2 ||
                 r.Status == request.Status));
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

        public IEnumerable<ServiceRequestModel> GetRelatedRequests(
            ServiceRequestModel selectedRequest, 
            bool isStaffUser,
            int maxDepth = 2)
        {
            var relatedRequests = new HashSet<ServiceRequestModel>();
            var queue = new Queue<(Guid Id, int Depth)>();
            var visited = new HashSet<Guid>();

            queue.Enqueue((selectedRequest.RequestID, 0));
            visited.Add(selectedRequest.RequestID);

            while (queue.Count > 0)
            {
                var (currentId, depth) = queue.Dequeue();
                var currentRequest = requests[currentId];

                // Staff can see all related requests, non-staff only see their own
                if (isStaffUser || currentRequest.CreatedBy == selectedRequest.CreatedBy)
                {
                    relatedRequests.Add(currentRequest);
                }

                if (depth < maxDepth && adjacencyList.ContainsKey(currentId))
                {
                    foreach (var (targetId, weight) in adjacencyList[currentId]
                        .OrderByDescending(x => x.Weight))
                    {
                        if (!visited.Contains(targetId))
                        {
                            visited.Add(targetId);
                            queue.Enqueue((targetId, depth + 1));
                        }
                    }
                }
            }

            return relatedRequests;
        }
    }
}
