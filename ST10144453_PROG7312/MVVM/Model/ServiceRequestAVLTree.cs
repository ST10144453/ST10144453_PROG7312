using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    internal class ServiceRequestAVLTree : IServiceRequestTree
    {
        private ServiceRequestNode root;
        private SortingStrategy currentStrategy;
        private readonly Dictionary<string, List<ServiceRequestModel>> categoryIndex;
        private readonly Dictionary<string, List<ServiceRequestModel>> userIndex;

        public ServiceRequestAVLTree()
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

            root = InsertRec(root, request);
        }

        private ServiceRequestNode InsertRec(ServiceRequestNode node, ServiceRequestModel request)
        {
            if (node == null)
                return new ServiceRequestNode(request);

            int comparison = CompareBasedOnStrategy(request, node.Data);

            if (comparison < 0)
                node.Left = InsertRec(node.Left, request);
            else if (comparison > 0)
                node.Right = InsertRec(node.Right, request);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && CompareBasedOnStrategy(request, node.Left.Data) < 0)
                return RightRotate(node);

            // Right Right Case
            if (balance < -1 && CompareBasedOnStrategy(request, node.Right.Data) > 0)
                return LeftRotate(node);

            // Left Right Case
            if (balance > 1 && CompareBasedOnStrategy(request, node.Left.Data) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && CompareBasedOnStrategy(request, node.Right.Data) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        private int CompareBasedOnStrategy(ServiceRequestModel a, ServiceRequestModel b)
        {
            switch (currentStrategy)
            {
                case SortingStrategy.ByDate:
                    return a.RequestDate.CompareTo(b.RequestDate);
                case SortingStrategy.ByCategory:
                    return a.Category.CompareTo(b.Category);
                case SortingStrategy.ByStatus:
                    return a.Status.CompareTo(b.Status);
                default:
                    return GetPriorityScore(a).CompareTo(GetPriorityScore(b));
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

        public void Delete(ServiceRequestModel request)
        {
            if (request == null) return;
            root = DeleteRec(root, request);
        }

        private ServiceRequestNode DeleteRec(ServiceRequestNode node, ServiceRequestModel request)
        {
            if (node == null) return null;

            int comparison = CompareBasedOnStrategy(request, node.Data);

            if (comparison < 0)
                node.Left = DeleteRec(node.Left, request);
            else if (comparison > 0)
                node.Right = DeleteRec(node.Right, request);
            else
            {
                if (node.Left == null || node.Right == null)
                {
                    ServiceRequestNode temp = null;
                    if (temp == node.Left)
                        temp = node.Right;
                    else
                        temp = node.Left;

                    if (temp == null)
                        return null;
                    else
                        node = temp;
                }
                else
                {
                    ServiceRequestNode temp = MinValueNode(node.Right);
                    node.Data = temp.Data;
                    node.Right = DeleteRec(node.Right, temp.Data);
                }
            }

            return node;
        }

        private ServiceRequestNode MinValueNode(ServiceRequestNode node)
        {
            ServiceRequestNode current = node;
            while (current.Left != null)
                current = current.Left;
            return current;
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

        public IEnumerable<ServiceRequestModel> GetRequestsByCategory(string category)
            => categoryIndex.ContainsKey(category) ? categoryIndex[category] : Enumerable.Empty<ServiceRequestModel>();

        public IEnumerable<ServiceRequestModel> GetRequestsByDateRange(DateTime startDate, DateTime endDate)
            => GetAllRequests().Where(r => r.RequestDate >= startDate && r.RequestDate <= endDate);

        public IEnumerable<ServiceRequestModel> GetRequestsByUser(string username)
            => userIndex.ContainsKey(username) ? userIndex[username] : Enumerable.Empty<ServiceRequestModel>();

        public IEnumerable<ServiceRequestModel> GetRequestsByPriority()
            => GetAllRequests().OrderByDescending(r => GetPriorityScore(r));

        public void SetSortingStrategy(SortingStrategy strategy)
        {
            currentStrategy = strategy;
            RebuildTree();
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

        public string GetTreeType() => "AVL Tree";

        public string GetTreeDescription() =>
            "A self-balancing binary search tree where the heights of two child " +
            "subtrees of any node differ by at most one. This ensures O(log n) " +
            "operations through automatic rebalancing.";

        private int GetHeight(ServiceRequestNode node)
        {
            return node?.Height ?? 0;
        }

        private int GetBalance(ServiceRequestNode node)
        {
            return node == null ? 0 : GetHeight(node.Left) - GetHeight(node.Right);
        }

        private ServiceRequestNode RightRotate(ServiceRequestNode y)
        {
            ServiceRequestNode x = y.Left;
            ServiceRequestNode T2 = x.Right;

            x.Right = y;
            y.Left = T2;

            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x;
        }

        private ServiceRequestNode LeftRotate(ServiceRequestNode x)
        {
            ServiceRequestNode y = x.Right;
            ServiceRequestNode T2 = y.Left;

            y.Left = x;
            x.Right = T2;

            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y;
        }
    }
}
