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
        public ObservableCollection<ReportModel> SelectedReports { get; } = new ObservableCollection<ReportModel>();
        private readonly UserModel _currentUser;

        public SelectReportsDialog(UserModel currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadReports();
        }

        private void LoadReports()
        {
            var allReports = ReportManager.Instance.Reports;
            var filteredReports = allReports.Where(r => r.CreatedBy == _currentUser?.userName).ToList();
            ReportsListBox.ItemsSource = filteredReports; // Ensure ReportsListView is defined in XAML
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ReportModel report in ReportsListBox.SelectedItems)
            {
                SelectedReports.Add(report);
            }
            DialogResult = true;
            Close();
        }
    }
}