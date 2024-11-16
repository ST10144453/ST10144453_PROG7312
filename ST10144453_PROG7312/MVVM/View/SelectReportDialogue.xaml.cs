using ST10144453_PROG7312.MVVM.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class SelectReportsDialog : Window
    {
        public ObservableCollection<ReportModel> AvailableReports { get; private set; }
        public ObservableCollection<ReportModel> SelectedReports { get; private set; }

        public SelectReportsDialog(ObservableCollection<ReportModel> availableReports,
                                 ObservableCollection<ReportModel> currentlySelected)
        {
            InitializeComponent();
            AvailableReports = availableReports;
            SelectedReports = new ObservableCollection<ReportModel>(currentlySelected);
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (ReportModel report in ReportsListView.SelectedItems)
            {
                if (!SelectedReports.Contains(report))
                {
                    SelectedReports.Add(report);
                }
            }
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