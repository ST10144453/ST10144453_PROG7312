//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ST10144453_PROG7312.MVVM.View
{
    //============== Class: ForumUserControl ==============//
    /// <summary>
    /// This is the code-behind for the ForumUserControl class.
    /// </summary>
    public partial class ForumUserControl : UserControl
    {
        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        /// <summary>
        /// This method initializes the ForumUserControl class.
        /// </summary>
        public ForumUserControl()
        {
            InitializeComponent();
        }

        //++++++++++++++ Methods: ViewMediaButton_Click ++++++++++++++//
        /// <summary>
        /// This method opens the media file associated with the selected report.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMediaButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null)
                return;

            var report = button.DataContext as ReportModel;

            if (report != null)
            {
                foreach (var mediaItem in report.Media)
                {
                    if (mediaItem.IsPdf || mediaItem.IsWord || mediaItem.IsText || mediaItem.IsImage)
                    {
                        try
                        {
                            byte[] fileBytes = Convert.FromBase64String(mediaItem.Base64String);
                            string tempFilePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), Guid.NewGuid().ToString());

                            string fileExtension = string.Empty;

                            if (mediaItem.IsPdf)
                            {
                                fileExtension = ".pdf";
                            }
                            else if (mediaItem.IsWord)
                            {
                                fileExtension = ".docx";
                            }
                            else if (mediaItem.IsText)
                            {
                                fileExtension = ".txt";
                            }
                            else if (mediaItem.IsImage)
                            {
                                fileExtension = ".png"; // or ".jpg", depending on the image format
                            }

                            tempFilePath += fileExtension;

                            File.WriteAllBytes(tempFilePath, fileBytes);

                            Process.Start(new ProcessStartInfo
                            {
                                FileName = tempFilePath,
                                UseShellExecute = true
                            });

                            return;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }

                MessageBox.Show("No suitable media file available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("No report selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //++++++++++++++ Methods: ForumUserControl_Loaded ++++++++++++++//
        /// <summary>
        /// This method refreshes the reports collection view source when the user control is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForumUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                CollectionViewSource.GetDefaultView(viewModel.Reports).Refresh();
            }
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//