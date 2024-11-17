using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReportDetailsWindow.xaml
    /// </summary>
    public partial class ReportDetailsWindow : Window
    {
        private readonly ReportModel _report;

        public ReportDetailsWindow(ReportModel report)
        {
            InitializeComponent();
            _report = report;
            DataContext = new { Report = report };
        }

        /// <summary>
        /// Handles the click event for viewing media files
        /// </summary>
        private void ViewMedia_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is MediaItem mediaItem)
            {
                try
                {
                    // Create a temporary file with the correct extension
                    string extension = ".tmp";
                    if (mediaItem.IsImage) extension = ".png";
                    if (mediaItem.IsPdf) extension = ".pdf";
                    if (mediaItem.IsWord) extension = ".docx";
                    if (mediaItem.IsText) extension = ".txt";

                    string tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}{extension}");
                    File.WriteAllBytes(tempFilePath, Convert.FromBase64String(mediaItem.Base64String));

                    // Open the file with the default application
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = tempFilePath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening media file: {ex.Message}",
                                  "Error",
                                  MessageBoxButton.OK,
                                  MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// Handles the click event for the close button
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
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
