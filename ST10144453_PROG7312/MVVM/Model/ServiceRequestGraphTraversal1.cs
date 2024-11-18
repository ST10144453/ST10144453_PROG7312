using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ST10144453_PROG7312.MVVM.Model.ServiceRequestGraphHelper;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestGraphTraversal1 : IServiceRequestTraversal
    {
        private readonly Dictionary<string, HashSet<string>> adjacencyList;
        private readonly Dictionary<string, ServiceRequestModel> requests;

        public ServiceRequestGraphTraversal1()
        {
            adjacencyList = new Dictionary<string, HashSet<string>>();
            requests = new Dictionary<string, ServiceRequestModel>();
        }

        public IEnumerable<ServiceRequestModel> GetRelatedRequests(
            ServiceRequestModel selectedRequest,
            bool isStaffUser,
            int maxDepth = 2)
        {
            if (selectedRequest == null) return Enumerable.Empty<ServiceRequestModel>();

            var requestId = selectedRequest.RequestID.ToString();
            var relatedRequests = new HashSet<ServiceRequestModel>();
            var visited = new HashSet<string>();
            var queue = new Queue<(string Id, int Depth)>();

            queue.Enqueue((requestId, 0));
            visited.Add(requestId);

            while (queue.Count > 0)
            {
                var (currentId, depth) = queue.Dequeue();
                
                if (requests.TryGetValue(currentId, out var currentRequest))
                {
                    if (isStaffUser || currentRequest.CreatedBy == selectedRequest.CreatedBy)
                    {
                        relatedRequests.Add(currentRequest);
                    }

                    if (depth < maxDepth && adjacencyList.ContainsKey(currentId))
                    {
                        foreach (var neighborId in adjacencyList[currentId])
                        {
                            if (!visited.Contains(neighborId))
                            {
                                visited.Add(neighborId);
                                queue.Enqueue((neighborId, depth + 1));
                            }
                        }
                    }
                }
            }

            return relatedRequests.Where(r => r.RequestID.ToString() != requestId);
        }

        public void AddRequest(ServiceRequestModel request)
        {
            var requestId = request.RequestID.ToString();
            requests[requestId] = request;
            
            if (!adjacencyList.ContainsKey(requestId))
            {
                adjacencyList[requestId] = new HashSet<string>();
            }

            // Find and add relationships
            foreach (var existingRequest in requests.Values)
            {
                if (existingRequest.RequestID == request.RequestID) continue;

                if (AreRequestsRelated(request, existingRequest))
                {
                    var existingId = existingRequest.RequestID.ToString();
                    adjacencyList[requestId].Add(existingId);
                    adjacencyList[existingId].Add(requestId);
                }
            }
        }

        private bool AreRequestsRelated(ServiceRequestModel request1, ServiceRequestModel request2)
        {
            // Check various relationship criteria
            return request1.Category == request2.Category ||
                   request1.CreatedBy == request2.CreatedBy ||
                   Math.Abs((request1.RequestDate - request2.RequestDate).TotalDays) <= 2 ||
                   request1.Status == request2.Status;
        }
    }
}
