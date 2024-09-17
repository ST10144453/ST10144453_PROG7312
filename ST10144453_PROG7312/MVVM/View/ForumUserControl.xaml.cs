using ST10144453_PROG7312.MVVM.View_Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using ST10144453_PROG7312.MVVM.Model;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class ForumUserControl : UserControl
    {
        public ForumUserControl()
        {
            InitializeComponent();
        }

        private void ViewPdfButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the button that was clicked
            var button = sender as Button;
            if (button == null)
                return;

            // Get the DataContext of the button's parent, which should be the ReportModel
            var report = button.DataContext as ReportModel;

            if (report != null)
            {
                var selectedPdfItem = report.Media.FirstOrDefault(item => item.IsPdf);

                if (selectedPdfItem != null)
                {
                    try
                    {
                        // Decode Base64 to bytes
                        byte[] pdfBytes = Convert.FromBase64String(selectedPdfItem.Base64String);
                        string tempPdfPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + ".pdf");
                        File.WriteAllBytes(tempPdfPath, pdfBytes);

                        // Open PDF file in default browser
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = tempPdfPath,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                
                else
                {
                    MessageBox.Show("No PDF file available in MediaItems.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Log detailed error information
                    Console.WriteLine("Detailed Error: No PDF file available in MediaItems.");
                    Console.WriteLine($"Report Name: {report.reportName}");
                    Console.WriteLine($"Media Count: {report.Media.Count}");
                    foreach (var mediaItem in report.Media)
                    {
                        Console.WriteLine($"MediaItem - IsPdf: {mediaItem.IsPdf}, Base64String length: {mediaItem.Base64String.Length}");
                    }
                }
            }
            else
            {
                MessageBox.Show("No report selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Console.WriteLine("Detailed Error: No report selected.");
            }
        }

        private void ViewWordButton_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the button that was clicked
            var button = sender as Button;
            if (button == null)
                return;

            // Get the DataContext of the button's parent, which should be the ReportModel
            var report = button.DataContext as ReportModel;

            if (report != null)
            {
                var selectedWordItem = report.Media.FirstOrDefault(item => item.IsWord);

                if (selectedWordItem != null)
                {
                    try
                    {
                        // Decode Base64 to bytes
                        byte[] wordBytes = Convert.FromBase64String(selectedWordItem.Base64String);
                        string tempWordPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString() + ".docx");
                        File.WriteAllBytes(tempWordPath, wordBytes);

                        // Open Word file in default browser
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = tempWordPath,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No Word file available in MediaItems.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Log detailed error information
                    Console.WriteLine("Detailed Error: No Word file available in MediaItems.");
                    Console.WriteLine($"Report Name: {report.reportName}");
                    Console.WriteLine($"Media Count: {report.Media.Count}");
                    foreach (var mediaItem in report.Media)
                    {
                        Console.WriteLine($"MediaItem - IsWord: {mediaItem.IsWord}, Base64String length: {mediaItem.Base64String.Length}");
                    }
                }

            }
        }

        

private void ForumUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Refresh the Reports collection when the view is loaded
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                CollectionViewSource.GetDefaultView(viewModel.Reports).Refresh();
            }
        }
    }
}