using ST10144453_PROG7312.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestTree : IServiceRequestTree
    {
        private ServiceRequestNode root;
        private SortingStrategy currentStrategy;

        public ServiceRequestTree()
        {
            root = null;
            currentStrategy = SortingStrategy.ByDate;
        }

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

        public void Insert(ServiceRequestModel request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            root = InsertRec(root, request);
        }

        private ServiceRequestNode InsertRec(ServiceRequestNode node, ServiceRequestModel request)
        {
            if (node == null)
                return new ServiceRequestNode(request);

            // Sort by creation date
            if (request.RequestDate.CompareTo(node.Data.RequestDate) < 0)
                node.Left = InsertRec(node.Left, request);
            else if (request.RequestDate.CompareTo(node.Data.RequestDate) > 0)
                node.Right = InsertRec(node.Right, request);
            else
                return node;

            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            int balance = GetBalance(node);

            // Left Left Case
            if (balance > 1 && request.RequestDate.CompareTo(node.Left.Data.RequestDate) < 0)
                return RightRotate(node);

            // Right Right Case
            if (balance < -1 && request.RequestDate.CompareTo(node.Right.Data.RequestDate) > 0)
                return LeftRotate(node);

            // Left Right Case
            if (balance > 1 && request.RequestDate.CompareTo(node.Left.Data.RequestDate) > 0)
            {
                node.Left = LeftRotate(node.Left);
                return RightRotate(node);
            }

            // Right Left Case
            if (balance < -1 && request.RequestDate.CompareTo(node.Right.Data.RequestDate) < 0)
            {
                node.Right = RightRotate(node.Right);
                return LeftRotate(node);
            }

            return node;
        }

        public IEnumerable<ServiceRequestModel> GetAllRequests()
        {
            var requests = new List<ServiceRequestModel>();
            InOrderTraversal(root, requests);
            return requests;
        }

        private void InOrderTraversal(ServiceRequestNode node, List<ServiceRequestModel> requests)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, requests);
                requests.Add(node.Data);
                InOrderTraversal(node.Right, requests);
            }
        }

        public IEnumerable<ServiceRequestModel> GetRequestsByCategory(string category)
        {
            var requests = new List<ServiceRequestModel>();
            GetRequestsByCategoryRec(root, category, requests);
            return requests;
        }

        private void GetRequestsByCategoryRec(ServiceRequestNode node, string category, List<ServiceRequestModel> requests)
        {
            if (node != null)
            {
                GetRequestsByCategoryRec(node.Left, category, requests);
                if (node.Data.Category == category)
                    requests.Add(node.Data);
                GetRequestsByCategoryRec(node.Right, category, requests);
            }
        }

        public IEnumerable<ServiceRequestModel> GetRequestsByDateRange(DateTime startDate, DateTime endDate)
        {
            var requests = new List<ServiceRequestModel>();
            GetRequestsByDateRangeRec(root, startDate, endDate, requests);
            return requests;
        }

        private void GetRequestsByDateRangeRec(ServiceRequestNode node, DateTime startDate, DateTime endDate, List<ServiceRequestModel> requests)
        {
            if (node == null) return;

            if (node.Data.RequestDate >= startDate)
                GetRequestsByDateRangeRec(node.Left, startDate, endDate, requests);

            if (node.Data.RequestDate >= startDate && node.Data.RequestDate <= endDate)
                requests.Add(node.Data);

            if (node.Data.RequestDate <= endDate)
                GetRequestsByDateRangeRec(node.Right, startDate, endDate, requests);
        }

        public IEnumerable<ServiceRequestModel> GetRequestsByUser(string username)
        {
            var requests = new List<ServiceRequestModel>();
            GetRequestsByUserRec(root, username, requests);
            return requests;
        }

        private void GetRequestsByUserRec(ServiceRequestNode node, string username, List<ServiceRequestModel> requests)
        {
            if (node != null)
            {
                GetRequestsByUserRec(node.Left, username, requests);
                if (node.Data.CreatedBy == username)
                    requests.Add(node.Data);
                GetRequestsByUserRec(node.Right, username, requests);
            }
        }

        private void AddToPriorityQueue(ServiceRequestNode node, PriorityQueue<ServiceRequestModel> queue)
        {
            if (node != null)
            {
                AddToPriorityQueue(node.Left, queue);
                queue.Enqueue(node.Data, GetPriorityScore(node.Data));
                AddToPriorityQueue(node.Right, queue);
            }
        }

        public IEnumerable<ServiceRequestModel> GetRequestsByPriority()
        {
            var priorityQueue = new PriorityQueue<ServiceRequestModel>();
            AddToPriorityQueue(root, priorityQueue);

            var result = new List<ServiceRequestModel>();
            while (!priorityQueue.IsEmpty)
            {
                result.Add(priorityQueue.Dequeue());
            }
            return result;
        }


        private int GetPriorityScore(ServiceRequestModel request)
        {
            // Calculate priority based on age and status
            int score = 0;
            var age = (DateTime.Now - request.RequestDate).Days;
            
            if (age > 7) score += 3;
            else if (age > 3) score += 2;
            else score += 1;

            if (request.Status == "Pending") score += 2;
            
            return score;
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

        public string GetTreeType() => "AVL Tree";

        public string GetTreeDescription() => 
            "A self-balancing binary search tree where the heights of two child " +
            "subtrees of any node differ by at most one. This ensures O(log n) " +
            "operations through automatic rebalancing.";

        public void Delete(ServiceRequestModel request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            root = DeleteRec(root, request);
        }

        private ServiceRequestNode DeleteRec(ServiceRequestNode root, ServiceRequestModel request)
        {
            // Standard BST delete
            if (root == null)
                return root;

            // Compare dates for navigation
            if (request.RequestDate.CompareTo(root.Data.RequestDate) < 0)
                root.Left = DeleteRec(root.Left, request);
            else if (request.RequestDate.CompareTo(root.Data.RequestDate) > 0)
                root.Right = DeleteRec(root.Right, request);
            else
            {
                // Node with only one child or no child
                if (root.Left == null)
                    return root.Right;
                else if (root.Right == null)
                    return root.Left;

                // Node with two children: Get the inorder successor (smallest in the right subtree)
                root.Data = GetMinValue(root.Right);

                // Delete the inorder successor
                root.Right = DeleteRec(root.Right, root.Data);
            }

            // If tree had only one node
            if (root == null)
                return root;

            // Update height
            root.Height = Math.Max(GetHeight(root.Left), GetHeight(root.Right)) + 1;

            // Get balance factor
            int balance = GetBalance(root);

            // Left Left Case
            if (balance > 1 && GetBalance(root.Left) >= 0)
                return RightRotate(root);

            // Left Right Case
            if (balance > 1 && GetBalance(root.Left) < 0)
            {
                root.Left = LeftRotate(root.Left);
                return RightRotate(root);
            }

            // Right Right Case
            if (balance < -1 && GetBalance(root.Right) <= 0)
                return LeftRotate(root);

            // Right Left Case
            if (balance < -1 && GetBalance(root.Right) > 0)
            {
                root.Right = RightRotate(root.Right);
                return LeftRotate(root);
            }

            return root;
        }

        private ServiceRequestModel GetMinValue(ServiceRequestNode node)
        {
            ServiceRequestModel minValue = node.Data;
            while (node.Left != null)
            {
                minValue = node.Left.Data;
                node = node.Left;
            }
            return minValue;
        }
    }
}
