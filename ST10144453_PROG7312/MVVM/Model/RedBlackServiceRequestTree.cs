using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class RedBlackNode
{
    public ServiceRequestModel Data { get; set; }
    public RedBlackNode Left { get; set; }
    public RedBlackNode Right { get; set; }
    public RedBlackNode Parent { get; set; }
    public bool IsRed { get; set; }

    public RedBlackNode(ServiceRequestModel data)
    {
        Data = data;
        IsRed = true; // New nodes are red by default
    }
}

public class RedBlackServiceRequestTree : IServiceRequestTree
{
    private RedBlackNode root;
    private SortingStrategy currentStrategy;
    private readonly Dictionary<string, List<ServiceRequestModel>> categoryIndex;
    private readonly Dictionary<string, List<ServiceRequestModel>> userIndex;

    public RedBlackServiceRequestTree()
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

        // Create new node
        var node = new RedBlackNode(request);

        // Standard BST insert
        if (root == null)
        {
            root = node;
        }
        else
        {
            RedBlackNode current = root;
            RedBlackNode parent = null;

            while (current != null)
            {
                parent = current;
                if (CompareBasedOnStrategy(node.Data, current.Data) < 0)
                    current = current.Left;
                else
                    current = current.Right;
            }

            node.Parent = parent;

            if (CompareBasedOnStrategy(node.Data, parent.Data) < 0)
                parent.Left = node;
            else
                parent.Right = node;
        }

        // Fix Red-Black tree properties
        FixInsertion(node);
    }

    private void FixInsertion(RedBlackNode node)
    {
        while (node != root && node.Parent.IsRed)
        {
            if (node.Parent == node.Parent.Parent.Left)
            {
                var uncle = node.Parent.Parent.Right;
                
                if (uncle != null && uncle.IsRed)
                {
                    // Case 1: Uncle is red
                    node.Parent.IsRed = false;
                    uncle.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Right)
                    {
                        // Case 2: Uncle is black and node is right child
                        node = node.Parent;
                        LeftRotate(node);
                    }
                    // Case 3: Uncle is black and node is left child
                    node.Parent.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    RightRotate(node.Parent.Parent);
                }
            }
            else
            {
                // Same as above with "left" and "right" exchanged
                var uncle = node.Parent.Parent.Left;
                
                if (uncle != null && uncle.IsRed)
                {
                    node.Parent.IsRed = false;
                    uncle.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    node = node.Parent.Parent;
                }
                else
                {
                    if (node == node.Parent.Left)
                    {
                        node = node.Parent;
                        RightRotate(node);
                    }
                    node.Parent.IsRed = false;
                    node.Parent.Parent.IsRed = true;
                    LeftRotate(node.Parent.Parent);
                }
            }
        }
        root.IsRed = false; // Root must be black
    }

    private void LeftRotate(RedBlackNode x)
    {
        var y = x.Right;
        x.Right = y.Left;
        
        if (y.Left != null)
            y.Left.Parent = x;
            
        y.Parent = x.Parent;
        
        if (x.Parent == null)
            root = y;
        else if (x == x.Parent.Left)
            x.Parent.Left = y;
        else
            x.Parent.Right = y;
            
        y.Left = x;
        x.Parent = y;
    }

    private void RightRotate(RedBlackNode y)
    {
        var x = y.Left;
        y.Left = x.Right;
        
        if (x.Right != null)
            x.Right.Parent = y;
            
        x.Parent = y.Parent;
        
        if (y.Parent == null)
            root = x;
        else if (y == y.Parent.Right)
            y.Parent.Right = x;
        else
            y.Parent.Left = x;
            
        x.Right = y;
        y.Parent = x;
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
        // TODO: Implement Red-Black tree deletion
        if (categoryIndex.ContainsKey(request.Category))
            categoryIndex[request.Category].Remove(request);
        if (userIndex.ContainsKey(request.CreatedBy))
            userIndex[request.CreatedBy].Remove(request);
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

    private void InOrderTraversal(RedBlackNode node, List<ServiceRequestModel> results)
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

    public string GetTreeType() => "Red-Black Tree";

    public string GetTreeDescription() => 
        "A self-balancing binary search tree where each node has an extra color " +
        "property (red or black). The tree maintains balance through a set of " +
        "properties that ensure no path is more than twice as long as any other, " +
        "providing guaranteed O(log n) operations.";
}
}
