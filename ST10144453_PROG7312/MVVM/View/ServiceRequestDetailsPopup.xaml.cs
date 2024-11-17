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
        private readonly ServiceRequestDetailsViewModel _viewModel;

        public ServiceRequestDetailsPopup(ServiceRequestModel request)
        {
            InitializeComponent();
            _viewModel = new ServiceRequestDetailsViewModel(request, UserSession.CurrentUser);
            DataContext = _viewModel;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                _viewModel.UpdateStatus(comboBox.SelectedItem as string);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is AttachedFile file)
            {
                try
                {
                    string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), file.FileName);
                    System.IO.File.WriteAllBytes(tempFilePath, Convert.FromBase64String(file.FileContent));

                    Process.Start(new ProcessStartInfo
                    {
                        FileName = tempFilePath,
                        UseShellExecute = true
                    });
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
                var reportDetailsWindow = new ReportDetailsWindow(report)
                {
                    Owner = this,
                    DataContext = new ReportDetailsViewModel(report)
                };
                reportDetailsWindow.ShowDialog();
            }
        }
    }
}
