using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class SelectReportsDialog : Window
    {
        public List<ReportModel> SelectedReports { get; private set; }

        public SelectReportsDialog()
        {
            InitializeComponent();
            LoadReports();
        }

        private void LoadReports()
        {
            // Get reports from the ReportService
            var reports = ReportService.Instance.Reports.ToList();
            
            // Sort by date descending to show newest first
            reports = reports.OrderByDescending(r => r.reportDate).ToList();
            
            // Update the ListView
            ReportsListBox.ItemsSource = reports;
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedReports = ReportsListBox.SelectedItems.Cast<ReportModel>().ToList();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}