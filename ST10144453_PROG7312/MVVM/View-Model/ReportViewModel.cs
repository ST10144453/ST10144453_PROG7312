using Microsoft.Win32;
using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private string _issueName;
        private string _location;
        private string _selectedCategory;
        private string _description;

        public string IssueName
        {
            get => _issueName;
            set
            {
                if (_issueName != value)
                {
                    _issueName = value;
                    OnPropertyChanged(nameof(IssueName));
                }
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                if (_location != value)
                {
                    _location = value;
                    OnPropertyChanged(nameof(Location));
                }
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public ObservableCollection<ReportModel> Reports { get; set; }

        private ObservableCollection<MediaItem> _mediaItems;

        public ObservableCollection<MediaItem> MediaItems
        {
            get => _mediaItems;
            set
            {
                if (_mediaItems != value)
                {
                    _mediaItems = value;
                    OnPropertyChanged(nameof(MediaItems));
                }
            }
        }

        public bool ContainsPdf => MediaItems.Any(m => m.IsPdf);
        public bool ContainsWord => MediaItems.Any(m => m.IsWord);

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private MediaItem _selectedMediaItem;
        public MediaItem SelectedMediaItem
        {
            get => _selectedMediaItem;
            set
            {
                if (_selectedMediaItem != value)
                {
                    _selectedMediaItem = value;
                    OnPropertyChanged(nameof(SelectedMediaItem));
                }
            }
        }
        public ICommand AttachMediaCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
        public ICommand NavigateToHomeCommand { get; private set; }

        public ReportViewModel()
        {
            AttachMediaCommand = new RelayCommand(AttachMedia);
            SubmitCommand = new RelayCommand(Submit);
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            MediaItems = new ObservableCollection<MediaItem>();

            Reports = ReportManager.Instance.Reports;
        }

        private void NavigateToHome()
        {
            Window reportWindow = Application.Current.Windows.OfType<ReportView>().FirstOrDefault();
            reportWindow?.Close();

            Window mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.Show();
        }

        private string EncodeFileToBase64(string filePath)
        {
            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            // Get the file extension
            string extension = Path.GetExtension(filePath).ToLower();

            // Supported file extensions
            List<string> supportedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf", ".txt", ".docx" };

            // Check if the file extension is supported
            if (!supportedExtensions.Contains(extension))
            {
                throw new NotSupportedException("Unsupported file type.");
            }

            // Read file content and convert to Base64
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        private string EncodeTextFileToBase64(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            byte[] fileBytes = Encoding.UTF8.GetBytes(fileContent);
            return Convert.ToBase64String(fileBytes);
        }

        private string DecodeBase64ToFile(string base64String, string outputFilePath)
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(outputFilePath, fileBytes);
            return outputFilePath; // Return the file path
        }

        private string DecodeBase64ToTextFile(string base64String, string outputFilePath)
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            string fileContent = Encoding.UTF8.GetString(fileBytes);
            File.WriteAllText(outputFilePath, fileContent);
            return fileContent; // Return the content of the text file
        }

        private bool IsImageFile(string filename)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", };
            return imageExtensions.Contains(Path.GetExtension(filename).ToLower());
        }

        private bool IsTextFile(string filename)
        {
            string[] textExtensions = { ".txt", ".csv", ".log" };
            return textExtensions.Contains(Path.GetExtension(filename).ToLower());
        }
        private bool IsPdfFile(string filename)
        {
            return Path.GetExtension(filename).ToLower() == ".pdf";
        }

        private bool IsWordFile(string filename)
        {
            string[] wordExtensions = { ".doc", ".docx" };
            return wordExtensions.Contains(Path.GetExtension(filename).ToLower());
        }

        private void AttachMedia()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string filename in openFileDialog.FileNames)
                {
                    string base64String = EncodeFileToBase64(filename);
                    bool isImage = IsImageFile(filename);
                    bool isPdf = IsPdfFile(filename);
                    bool isText = IsTextFile(filename);
                    bool isWord = IsWordFile(filename); // Check if it's a Word document

                    if (isImage || isPdf || isText || isWord)
                    {
                        MediaItems.Add(new MediaItem
                        {
                            Base64String = base64String,
                            IsImage = isImage,
                            IsText = isText,
                            IsPdf = isPdf,
                            IsWord = isWord // Ensure this property is set
                        });
                    }
                    else
                    {
                        MessageBox.Show("Unsupported file type: " + filename, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        public void TryOpenFile(string base64String, string fileExtension)
        {
            try
            {
                byte[] fileBytes = Convert.FromBase64String(base64String);
                string tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + fileExtension);
                File.WriteAllBytes(tempFilePath, fileBytes);

                Process.Start(new ProcessStartInfo
                {
                    FileName = tempFilePath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Submit()
        {
            var newReport = new ReportModel
            {
                reportName = IssueName,
                reportLocation = Location,
                reportDescription = Description,
                reportCategory = SelectedCategory,
                Media = new List<Model.MediaItem>(),
                reportDate = DateTime.Now // Fix: Assign DateTime.Now directly without ToString()
            };

            string reportGuid = Guid.NewGuid().ToString();
            string reportFolder = Path.Combine("Files", reportGuid);
            Directory.CreateDirectory(reportFolder);

            foreach (var mediaItem in MediaItems)
            {
                string fileName = Guid.NewGuid().ToString();
                string destinationPath = Path.Combine(reportFolder, fileName);

                if (mediaItem.IsImage)
                {
                    string filePath = DecodeBase64ToFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsImage = true, IsPdf = false });
                }
                else if (mediaItem.IsPdf)
                {
                    string filePath = DecodeBase64ToFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsPdf = true, IsImage = false, IsText = false });
                }
                else if (mediaItem.IsWord)
                {
                    string filePath = DecodeBase64ToFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsPdf = false, IsImage = false, IsText = false, IsWord = true });
                }
                else
                {
                    string textContent = DecodeBase64ToTextFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsImage = false, IsText = true, IsPdf = false });
                }
            }

            ReportManager.Instance.AddReport(newReport);

            OnPropertyChanged(nameof(Reports));

            // Debug output
            Console.WriteLine("New report added:");
            Console.WriteLine($"Name: {newReport.reportName}");
            Console.WriteLine($"Location: {newReport.reportLocation}");
            Console.WriteLine($"Category: {newReport.reportCategory}");
            Console.WriteLine($"Description: {newReport.reportDescription}");
            Console.WriteLine($"Media count: {newReport.Media.Count}");

            foreach (var mediaItem in MediaItems)
            {
                Console.WriteLine($"Base64String length: {mediaItem.Base64String.Length}");
                Console.WriteLine($"IsImage: {mediaItem.IsImage}");
                Console.WriteLine($"IsText: {mediaItem.IsText}");
                Console.WriteLine($"IsPdf: {mediaItem.IsPdf}");
            }

            // Clear inputs
            IssueName = string.Empty;
            Location = string.Empty;
            Description = string.Empty;
            SelectedCategory = string.Empty;
            MediaItems.Clear();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}