using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class TreeComparisonTool
    {
        private Dictionary<TreeType, IServiceRequestTree> _trees;
        private Dictionary<TreeType, TreePerformanceMetrics> _metrics;

        public TreeComparisonTool()
        {
            _trees = new Dictionary<TreeType, IServiceRequestTree>();
            _metrics = new Dictionary<TreeType, TreePerformanceMetrics>();

            foreach (TreeType type in Enum.GetValues(typeof(TreeType)))
            {
                _trees[type] = ServiceRequestTreeFactory.CreateTree(type);
                _metrics[type] = new TreePerformanceMetrics(type.ToString());
            }
        }

        public void RunComparison(List<ServiceRequestModel> testData)
        {
            foreach (var tree in _trees)
            {
                var metrics = _metrics[tree.Key];

                // Test insertion
                metrics.RecordOperation("Bulk Insert", () =>
                {
                    foreach (var request in testData)
                    {
                        tree.Value.Insert(request);
                    }
                });

                // Test search
                metrics.RecordOperation("Search", () =>
                {
                    foreach (var request in testData.Take(10))
                    {
                        tree.Value.GetRequestsByCategory(request.Category);
                    }
                });

                // Test sorting
                metrics.RecordOperation("Sort", () =>
                {
                    tree.Value.SetSortingStrategy(SortingStrategy.ByPriority);
                    tree.Value.GetAllRequests().ToList();
                });
            }
        }

        public string GenerateComparisonReport()
        {
            var report = new StringBuilder();
            report.AppendLine("Tree Implementation Comparison Report");
            report.AppendLine("====================================");

            foreach (var metric in _metrics)
            {
                report.AppendLine();
                report.AppendLine(metric.Value.GenerateReport());
            }

            return report.ToString();
        }
    }
}
