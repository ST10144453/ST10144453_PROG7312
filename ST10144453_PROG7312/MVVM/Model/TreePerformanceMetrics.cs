using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class TreePerformanceMetrics
{
    public class OperationMetrics
    {
        public long ExecutionTime { get; set; }
        public int ComparisonCount { get; set; }
        public int RotationCount { get; set; }
    }

    private Dictionary<string, OperationMetrics> _metrics;
    private readonly string _treeType;

    public TreePerformanceMetrics(string treeType)
    {
        _treeType = treeType;
        _metrics = new Dictionary<string, OperationMetrics>();
    }

    public void RecordOperation(string operation, Action action)
    {
        var metrics = new OperationMetrics();
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        action.Invoke();
        
        stopwatch.Stop();
        metrics.ExecutionTime = stopwatch.ElapsedTicks;

        if (!_metrics.ContainsKey(operation))
            _metrics[operation] = metrics;
        else
        {
            _metrics[operation].ExecutionTime = 
                (_metrics[operation].ExecutionTime + metrics.ExecutionTime) / 2;
        }
    }

    public string GenerateReport()
    {
        var report = new StringBuilder();
        report.AppendLine($"Performance Metrics for {_treeType}");
        report.AppendLine("================================");

        foreach (var metric in _metrics)
        {
            report.AppendLine($"Operation: {metric.Key}");
            report.AppendLine($"Average Execution Time: {metric.Value.ExecutionTime} ticks");
            report.AppendLine();
        }

        return report.ToString();
    }
}
}
