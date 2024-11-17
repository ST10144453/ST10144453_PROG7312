using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class BinarySearchServiceRequestTree : IServiceRequestTree
    {
        private ServiceRequestNode root;
        private SortingStrategy currentStrategy;
        private readonly Dictionary<string, List<ServiceRequestModel>> categoryIndex;
        private readonly Dictionary<string, List<ServiceRequestModel>> userIndex;

        public BinarySearchServiceRequestTree()
        {
            root = null;
            categoryIndex = new Dictionary<string, List<ServiceRequestModel>>();
            userIndex = new Dictionary<string, List<ServiceRequestModel>>();
            currentStrategy = SortingStrategy.ByDate;
        }

        public void Insert(ServiceRequestModel request)
        {
            // Update indices
            if (!categoryIndex.ContainsKey(request.Category))
                categoryIndex[request.Category] = new List<ServiceRequestModel>();
            categoryIndex[request.Category].Add(request);

            if (!userIndex.ContainsKey(request.CreatedBy))
                userIndex[request.CreatedBy] = new List<ServiceRequestModel>();
            userIndex[request.CreatedBy].Add(request);

            // Insert into BST
            root = InsertRec(root, request);
        }

        private ServiceRequestNode InsertRec(ServiceRequestNode node, ServiceRequestModel request)
        {
            if (node == null)
                return new ServiceRequestNode(request);

            int comparison = CompareBasedOnStrategy(request, node.Data);

            if (comparison < 0)
                node.Left = InsertRec(node.Left, request);
            else
                node.Right = InsertRec(node.Right, request);

            return node;
        }

        private int CompareBasedOnStrategy(ServiceRequestModel a, ServiceRequestModel b)
        {
            switch (currentStrategy)
            {
                case SortingStrategy.ByDate:
                    return a.RequestDate.CompareTo(b.RequestDate);
                case SortingStrategy.ByPriority:
                    return GetPriorityScore(b).CompareTo(GetPriorityScore(a));
                case SortingStrategy.ByCategory:
                    return string.Compare(a.Category, b.Category, StringComparison.Ordinal);
                case SortingStrategy.ByStatus:
                    return string.Compare(a.Status, b.Status, StringComparison.Ordinal);
                default:
                    return 0;
            }
        }


        public void Delete(ServiceRequestModel request)
        {
            root = DeleteRec(root, request);

            // Update indices
            if (categoryIndex.ContainsKey(request.Category))
                categoryIndex[request.Category].Remove(request);
            if (userIndex.ContainsKey(request.CreatedBy))
                userIndex[request.CreatedBy].Remove(request);
        }

        private ServiceRequestNode DeleteRec(ServiceRequestNode node, ServiceRequestModel request)
        {
            if (node == null)
                return null;

            int comparison = CompareBasedOnStrategy(request, node.Data);

            if (comparison < 0)
                node.Left = DeleteRec(node.Left, request);
            else if (comparison > 0)
                node.Right = DeleteRec(node.Right, request);
            else
            {
                // Node with only one child or no child
                if (node.Left == null)
                    return node.Right;
                if (node.Right == null)
                    return node.Left;

                // Node with two children: Get the inorder successor (smallest in right subtree)
                node.Data = MinValue(node.Right);
                node.Right = DeleteRec(node.Right, node.Data);
            }

            return node;
        }

        private ServiceRequestModel MinValue(ServiceRequestNode node)
        {
            ServiceRequestModel minv = node.Data;
            while (node.Left != null)
            {
                minv = node.Left.Data;
                node = node.Left;
            }
            return minv;
        }

        public void SetSortingStrategy(SortingStrategy strategy)
        {
            if (currentStrategy != strategy)
            {
                currentStrategy = strategy;
                RebuildTree();
            }
        }

        private void RebuildTree()
        {
            var allRequests = GetAllRequests().ToList();
            root = null;
            foreach (var request in allRequests)
            {
                Insert(request);
            }
        }

        public IEnumerable<ServiceRequestModel> GetAllRequests()
        {
            var results = new List<ServiceRequestModel>();
            InOrderTraversal(root, results);
            return results;
        }

        private void InOrderTraversal(ServiceRequestNode node, List<ServiceRequestModel> results)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, results);
                results.Add(node.Data);
                InOrderTraversal(node.Right, results);
            }
        }

        private int GetPriorityScore(ServiceRequestModel request)
        {
            int score = 0;
            var age = (DateTime.Now - request.RequestDate).Days;

            if (age > 7) score += 3;
            else if (age > 3) score += 2;
            else score += 1;

            if (request.Status == "Pending") score += 2;

            return score;
        }

        public string GetTreeType() => "Binary Search Tree";

        public string GetTreeDescription() =>
            "A binary tree where each node has at most two children. For each node, " +
            "all elements in the left subtree are less than the node, and all elements " +
            "in the right subtree are greater than the node. This provides efficient " +
            "searching and maintains sorted order.";

        public IEnumerable<ServiceRequestModel> GetRequestsByCategory(string category)
            => categoryIndex.ContainsKey(category) ? categoryIndex[category] : Enumerable.Empty<ServiceRequestModel>();

        public IEnumerable<ServiceRequestModel> GetRequestsByDateRange(DateTime startDate, DateTime endDate)
            => GetAllRequests().Where(r => r.RequestDate >= startDate && r.RequestDate <= endDate);

        public IEnumerable<ServiceRequestModel> GetRequestsByUser(string username)
        => userIndex.ContainsKey(username) ? userIndex[username] : Enumerable.Empty<ServiceRequestModel>();

        public IEnumerable<ServiceRequestModel> GetRequestsByPriority()
            => GetAllRequests().OrderByDescending(r => GetPriorityScore(r));
    }
}
