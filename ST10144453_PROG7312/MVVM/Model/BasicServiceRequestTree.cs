using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class BasicServiceRequestNode
{
    public ServiceRequestModel Data { get; set; }
    public List<BasicServiceRequestNode> Children { get; set; }

    public BasicServiceRequestNode(ServiceRequestModel data)
    {
        Data = data;
        Children = new List<BasicServiceRequestNode>();
    }
}

public class BasicServiceRequestTree : IServiceRequestTree
{
    private BasicServiceRequestNode root;
    private SortingStrategy currentStrategy;
    private readonly Dictionary<string, List<ServiceRequestModel>> categoryIndex;
    private readonly Dictionary<string, List<ServiceRequestModel>> userIndex;

    public BasicServiceRequestTree()
    {
        root = null;
        categoryIndex = new Dictionary<string, List<ServiceRequestModel>>();
        userIndex = new Dictionary<string, List<ServiceRequestModel>>();
        currentStrategy = SortingStrategy.ByDate;
    }

    public void Insert(ServiceRequestModel request)
    {
        if (root == null)
        {
            root = new BasicServiceRequestNode(request);
            return;
        }

        // Index by category
        if (!categoryIndex.ContainsKey(request.Category))
            categoryIndex[request.Category] = new List<ServiceRequestModel>();
        categoryIndex[request.Category].Add(request);

        // Index by user
        if (!userIndex.ContainsKey(request.CreatedBy))
            userIndex[request.CreatedBy] = new List<ServiceRequestModel>();
        userIndex[request.CreatedBy].Add(request);

        // Add to tree based on current sorting strategy
        InsertBasedOnStrategy(request);
    }

    private void InsertBasedOnStrategy(ServiceRequestModel request)
    {
        var currentNode = root;
        var comparison = CompareBasedOnStrategy(request, currentNode.Data);

        while (true)
        {
            if (comparison < 0)
            {
                if (currentNode.Children.Count == 0 || 
                    CompareBasedOnStrategy(request, currentNode.Children[0].Data) >= 0)
                {
                    currentNode.Children.Insert(0, new BasicServiceRequestNode(request));
                    break;
                }
                currentNode = currentNode.Children[0];
            }
            else
            {
                currentNode.Children.Add(new BasicServiceRequestNode(request));
                break;
            }
            comparison = CompareBasedOnStrategy(request, currentNode.Data);
        }
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

    public IEnumerable<ServiceRequestModel> GetAllRequests()
    {
        var results = new List<ServiceRequestModel>();
        if (root != null)
            TraverseTree(root, results);
        return results;
    }

    private void TraverseTree(BasicServiceRequestNode node, List<ServiceRequestModel> results)
    {
        results.Add(node.Data);
        foreach (var child in node.Children)
        {
            TraverseTree(child, results);
        }
    }

    public string GetTreeType() => "Basic Tree";

    public string GetTreeDescription() => 
        "A simple hierarchical tree structure where each node can have multiple children. " +
        "Requests are organized based on the selected sorting strategy, with parent-child " +
        "relationships determined by the sorting criteria.";

    // Implement other interface methods...
    public void Delete(ServiceRequestModel request) { /* Implementation */ }
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
