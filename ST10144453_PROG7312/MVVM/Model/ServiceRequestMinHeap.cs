using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestMinHeap : IServiceRequestHeap
    {
        private List<ServiceRequestModel> heap;
        private Dictionary<Guid, int> requestIndices;
        private Dictionary<Guid, int> priorityScores;

        public ServiceRequestMinHeap()
        {
            heap = new List<ServiceRequestModel>();
            requestIndices = new Dictionary<Guid, int>();
            priorityScores = new Dictionary<Guid, int>();
        }

        public void Insert(ServiceRequestModel request)
        {
            heap.Add(request);
            int index = heap.Count - 1;
            requestIndices[request.RequestID] = index;
            priorityScores[request.RequestID] = GetPriorityScore(request);
            HeapifyUp(index);
        }

        public ServiceRequestModel ExtractHighestPriority()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Heap is empty");

            var highestPriorityRequest = heap[0];
            var lastRequest = heap[heap.Count - 1];
            heap[0] = lastRequest;
            requestIndices[lastRequest.RequestID] = 0;
            heap.RemoveAt(heap.Count - 1);
            requestIndices.Remove(highestPriorityRequest.RequestID);
            priorityScores.Remove(highestPriorityRequest.RequestID);
            HeapifyDown(0);

            return highestPriorityRequest;
        }

        public void UpdatePriority(Guid requestId, int newPriority)
        {
            if (!requestIndices.ContainsKey(requestId))
                throw new KeyNotFoundException("Request not found");

            int index = requestIndices[requestId];
            int oldPriority = priorityScores[requestId];
            priorityScores[requestId] = newPriority;

            if (newPriority < oldPriority)
                HeapifyUp(index);
            else
                HeapifyDown(index);
        }

        public IEnumerable<ServiceRequestModel> GetPrioritizedRequests()
        {
            return heap.OrderBy(r => priorityScores[r.RequestID]).ToList();
        }

        private int GetPriorityScore(ServiceRequestModel request)
        {
            int score = 0;
            var age = (DateTime.Now - request.RequestDate).Days;

            // Age factor
            score += age > 7 ? 30 : (age > 3 ? 20 : 10);

            // Status factor
            score += request.Status == "Critical" ? 50 :
                     request.Status == "High" ? 40 :
                     request.Status == "Medium" ? 30 : 20;

            // Related requests factor (using category matches instead of LinkedRequests)
            var relatedRequests = heap.Count(r => r.Category == request.Category);
            score += relatedRequests * 5;

            return score;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (priorityScores[heap[index].RequestID] >= priorityScores[heap[parentIndex].RequestID])
                    break;

                // Swap elements
                var temp = heap[index];
                heap[index] = heap[parentIndex];
                heap[parentIndex] = temp;

                // Update indices
                requestIndices[heap[index].RequestID] = index;
                requestIndices[heap[parentIndex].RequestID] = parentIndex;

                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            int minIndex = index;
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;

            if (leftChild < heap.Count && 
                priorityScores[heap[leftChild].RequestID] < priorityScores[heap[minIndex].RequestID])
                minIndex = leftChild;

            if (rightChild < heap.Count && 
                priorityScores[heap[rightChild].RequestID] < priorityScores[heap[minIndex].RequestID])
                minIndex = rightChild;

            if (minIndex != index)
            {
                // Swap elements
                var temp = heap[index];
                heap[index] = heap[minIndex];
                heap[minIndex] = temp;

                // Update indices
                requestIndices[heap[index].RequestID] = index;
                requestIndices[heap[minIndex].RequestID] = minIndex;

                HeapifyDown(minIndex);
            }
        }
    }
}
