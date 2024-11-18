using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestMinHeap : IServiceRequestHeap
    {
        private readonly List<ServiceRequestModel> heap;
        private readonly Dictionary<Guid, int> requestIndices;
        private readonly Dictionary<Guid, int> priorityScores;
        private Dictionary<string, int> categoryCount;
        private Dictionary<string, TimeSpan> averageResponseTimes;
        private int totalRequests;
        private DateTime lastOptimization;

        public ServiceRequestMinHeap()
        {
            heap = new List<ServiceRequestModel>();
            requestIndices = new Dictionary<Guid, int>();
            priorityScores = new Dictionary<Guid, int>();
            categoryCount = new Dictionary<string, int>();
            averageResponseTimes = new Dictionary<string, TimeSpan>();
            lastOptimization = DateTime.Now;
        }

        public void Insert(ServiceRequestModel request)
        {
            heap.Add(request);
            int index = heap.Count - 1;
            requestIndices[request.RequestID] = index;

            // Calculate and store priority score
            int score = CalculatePriorityScore(request);
            priorityScores[request.RequestID] = score;

            // Update statistics
            totalRequests++;
            if (!categoryCount.ContainsKey(request.Category))
                categoryCount[request.Category] = 0;
            categoryCount[request.Category]++;

            HeapifyUp(index);
            OptimizeIfNeeded();
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

            if (heap.Count > 0)
                HeapifyDown(0);

            return highestPriorityRequest;
        }

        public void UpdatePriority(Guid requestId, int newPriority)
        {
            if (!requestIndices.ContainsKey(requestId))
                throw new KeyNotFoundException("Request not found in heap");

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
            var sortedRequests = heap.OrderBy(r => priorityScores[r.RequestID]).ToList();
            return sortedRequests;
        }

        private void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (priorityScores[heap[index].RequestID] >= priorityScores[heap[parentIndex].RequestID])
                    break;

                SwapNodes(index, parentIndex);
                index = parentIndex;
            }
        }

        private void HeapifyDown(int index)
        {
            while (true)
            {
                int smallest = index;
                int leftChild = 2 * index + 1;
                int rightChild = 2 * index + 2;

                if (leftChild < heap.Count &&
                    priorityScores[heap[leftChild].RequestID] < priorityScores[heap[smallest].RequestID])
                    smallest = leftChild;

                if (rightChild < heap.Count &&
                    priorityScores[heap[rightChild].RequestID] < priorityScores[heap[smallest].RequestID])
                    smallest = rightChild;

                if (smallest == index)
                    break;

                SwapNodes(index, smallest);
                index = smallest;
            }
        }

        private void SwapNodes(int i, int j)
        {
            var temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;

            requestIndices[heap[i].RequestID] = i;
            requestIndices[heap[j].RequestID] = j;
        }

        private int CalculatePriorityScore(ServiceRequestModel request)
        {
            int score = 0;
            var age = (DateTime.Now - request.RequestDate).Days;

            // Time-based priority (more urgent as time passes)
            score += age > 14 ? 100 :
                    age > 7 ? 75 :
                    age > 3 ? 50 :
                    age > 1 ? 25 : 10;

            // Status-based priority
            switch (request.Status)
            {
                case "Critical":
                    score += 200;
                    break;
                case "High":
                    score += 150;
                    break;
                case "Medium":
                    score += 100;
                    break;
                case "Low":
                    score += 50;
                    break;
                case "Pending":
                    score += 75;
                    break;
                case "In Progress":
                    score += 25;
                    break;
                default:
                    score += 0;
                    break;
            }

            // Category-based priority (essential services get higher priority)
            switch (request.Category)
            {
                case "Water & Sanitation":
                case "Electricity":
                    score += 100;
                    break;
                case "Public Safety":
                    score += 90;
                    break;
                case "Roads & Transport":
                    score += 80;
                    break;
                case "Waste Management":
                    score += 70;
                    break;
                case "Housing":
                    score += 60;
                    break;
                case "Parks & Recreation":
                    score += 50;
                    break;
                default:
                    score += 40;
                    break;
            }

            // Time of day factor (for emergency services)
            var hour = DateTime.Now.Hour;
            if (request.Category == "Public Safety" || request.Category == "Water & Sanitation" || request.Category == "Electricity")
            {
                score += 50;
            }

            return score;
        }

        public class ServiceRequestStatistics
        {
            public int TotalRequests { get; set; }
            public Dictionary<string, int> RequestsByCategory { get; set; }
            public Dictionary<string, TimeSpan> AverageResponseTimes { get; set; }
            public int CurrentHighestPriority { get; set; }
            public int TotalPendingRequests { get; set; }
            public double AveragePriorityScore { get; set; }
            public double HeapOptimizedResponseTime { get; set; }
            public double StandardResponseTime { get; set; }
            public int OptimizedPriorityScore { get; set; }
            public int StandardPriorityScore { get; set; }
        }

        public ServiceRequestStatistics GetStatistics()
        {
            var standardRequests = heap.OrderBy(r => r.RequestDate).ToList();
            var optimizedRequests = GetPrioritizedRequests().ToList();

            double standardScore = CalculateStandardScore(standardRequests);
            double optimizedScore = CalculateOptimizedScore(optimizedRequests);

            return new ServiceRequestStatistics
            {
                TotalRequests = totalRequests,
                RequestsByCategory = new Dictionary<string, int>(categoryCount),
                AverageResponseTimes = new Dictionary<string, TimeSpan>(averageResponseTimes),
                CurrentHighestPriority = heap.Any() ? priorityScores[heap[0].RequestID] : 0,
                TotalPendingRequests = heap.Count,
                AveragePriorityScore = heap.Any() ? priorityScores.Values.Average() : 0,
                HeapOptimizedResponseTime = CalculateAverageResponseTime(optimizedRequests),
                StandardResponseTime = CalculateAverageResponseTime(standardRequests),
                OptimizedPriorityScore = (int)optimizedScore,
                StandardPriorityScore = (int)standardScore
            };
        }

        private double CalculateStandardScore(List<ServiceRequestModel> requests)
        {
            if (!requests.Any()) return 0;
            return requests.Average(r => CalculatePriorityScore(r));
        }

        private double CalculateOptimizedScore(List<ServiceRequestModel> requests)
        {
            if (!requests.Any()) return 0;
            double score = 0;
            for (int i = 0; i < requests.Count; i++)
            {
                score += CalculatePriorityScore(requests[i]) * (requests.Count - i);
            }
            return score / requests.Count;
        }

        private double CalculateAverageResponseTime(List<ServiceRequestModel> requests)
        {
            if (!requests.Any()) return 0;
            var criticalRequests = requests.Where(r => r.Status == "Critical").ToList();
            return criticalRequests.Any() ? 
                criticalRequests.Average(r => (DateTime.Now - r.RequestDate).TotalMinutes) : 0;
        }

        private void OptimizeIfNeeded()
        {
            if ((DateTime.Now - lastOptimization).TotalHours >= 1 || 
                totalRequests % 50 == 0) // Every 50 requests
            {
                RebalanceAllPriorities();
                lastOptimization = DateTime.Now;
            }
        }

        private void RebalanceAllPriorities()
        {
            foreach (var request in heap)
            {
                int newScore = CalculatePriorityScore(request);
                UpdatePriority(request.RequestID, newScore);
            }
        }
    }
}
