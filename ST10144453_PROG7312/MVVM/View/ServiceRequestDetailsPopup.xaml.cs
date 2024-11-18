using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ST10144453_PROG7312.MVVM.View_Model;
using ST10144453_PROG7312.MVVM.View;
using System.Diagnostics;
using System.IO;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for ServiceRequestDetailsPopup.xaml
    /// </summary>
    public partial class ServiceRequestDetailsPopup : Window
    {
        private readonly ServiceRequestDetailsViewModel viewModel;

        public ServiceRequestDetailsPopup(ServiceRequestModel request)
        {
            InitializeComponent();
            viewModel = new ServiceRequestDetailsViewModel(request, UserSession.CurrentUser);
            DataContext = viewModel;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is string newStatus)
            {
                viewModel.UpdateStatus(newStatus);
            }
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string filePath)
            {
                try
                {
                    Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void OpenReport_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ReportModel report)
            {
                var reportDetails = new Window
                {
                    Title = "Report Details",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                var content = new TextBlock
                {
                    Text = $"Report Name: {report.reportName}\nDate: {report.reportDate}",
                    Margin = new Thickness(10),
                    TextWrapping = TextWrapping.Wrap
                };

                reportDetails.Content = content;
                reportDetails.ShowDialog();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
