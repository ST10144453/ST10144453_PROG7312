using ST10144453_PROG7312.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ST10144453_PROG7312.MVVM.Model;
using static ST10144453_PROG7312.MVVM.Model.ServiceRequestMinHeap;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ServiceRequestOptimizationViewModel : INotifyPropertyChanged, IDisposable
    {
        private ServiceRequestStatistics statistics;
        private Timer optimizationTimer;
        private DateTime lastOptimizationTime;
        private bool isCurrentlyOptimizing;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCurrentlyOptimizing
        {
            get => isCurrentlyOptimizing;
            private set
            {
                isCurrentlyOptimizing = value;
                OnPropertyChanged(nameof(IsCurrentlyOptimizing));
                OnPropertyChanged(nameof(OptimizationMetrics));
                OnPropertyChanged(nameof(LastOptimizationStatus));
            }
        }

        public string LastOptimizationStatus =>
            lastOptimizationTime == default ?
            "No optimization performed yet" :
            $"Last optimized: {lastOptimizationTime:HH:mm:ss}";

        public string OptimizationMetrics =>
            $"System Optimization Comparison:\n\n" +
            $"Heap-Optimized:\n" +
            $"• Response Time: {FormatTime(statistics?.HeapOptimizedResponseTime ?? 0)}\n" +
            $"• Priority Score: {statistics?.OptimizedPriorityScore ?? 0}\n" +
            $"• Pending Requests: {statistics?.TotalPendingRequests ?? 0}\n\n" +
            $"Standard Processing:\n" +
            $"• Response Time: {FormatTime(statistics?.StandardResponseTime ?? 0)}\n" +
            $"• Priority Score: {statistics?.StandardPriorityScore ?? 0}\n\n" +
            $"Improvement:\n" +
            $"• Response Time: {CalculateImprovement(statistics?.StandardResponseTime ?? 0, statistics?.HeapOptimizedResponseTime ?? 0):F1}%\n" +
            $"• Priority Handling: {CalculateImprovement(statistics?.StandardPriorityScore ?? 0, statistics?.OptimizedPriorityScore ?? 0):F1}%";

        private string FormatTime(double minutes)
        {
            if (minutes == 0)
            {
                return "N/A";
            }
            else if (minutes < 60)
            {
                return $"{minutes:F1} min";
            }
            else
            {
                return $"{(minutes / 60):F1} hrs";
            }
        }

        private double CalculateImprovement(double standard, double optimized)
        {
            if (standard == 0) return 0;
            return ((optimized - standard) / standard) * 100;
        }

        private async void UpdateStatistics()
        {
            IsCurrentlyOptimizing = true;

            try
            {
                await Task.Run(() =>
                {
                    statistics = ServiceRequestManager.Instance.GetOptimizationStatistics();
                    lastOptimizationTime = DateTime.Now;
                });
            }
            finally
            {
                IsCurrentlyOptimizing = false;
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ServiceRequestOptimizationViewModel()
        {
            UpdateStatistics(); // Initial update
            optimizationTimer = new Timer(state =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    UpdateStatistics();
                });
            }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        }

        public void Dispose()
        {
            optimizationTimer?.Dispose();
        }
    }
}
