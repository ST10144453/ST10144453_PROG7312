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
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;


namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private string _issueName;
        private string _location;
        private string _selectedCategory;
        private string _description;
        private bool _isIssueNameFilled;
        private bool _isLocationFilled;
        private bool _isCategorySelected;
        private bool _isDescriptionFilled;
        private double _progress;
        public double ProgressBarWidth => GetProgressBarWidth();


        public string IssueName
        {
            get => _issueName;
            set
            {
                if (_issueName != value)
                {
                    _issueName = value;
                    OnPropertyChanged();
                    IsIssueNameFilled = !string.IsNullOrWhiteSpace(value);
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
                    OnPropertyChanged();
                    IsLocationFilled = !string.IsNullOrWhiteSpace(value);
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
                    OnPropertyChanged();
                    IsCategorySelected = !string.IsNullOrWhiteSpace(value);
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
                    OnPropertyChanged();
                    IsDescriptionFilled = !string.IsNullOrWhiteSpace(value);
                }
            }
        }

        public bool IsIssueNameFilled
        {
            get => _isIssueNameFilled;
           set
            {
                if (_isIssueNameFilled != value)
                {
                    _isIssueNameFilled = value;
                    OnPropertyChanged();
                    UpdateProgress();
                }
            }
        }

        public bool IsLocationFilled
        {
            get => _isLocationFilled;
            set
            {
                if (_isLocationFilled != value)
                {
                    _isLocationFilled = value;
                    OnPropertyChanged();
                    UpdateProgress();
                }
            }
        }

        public bool IsCategorySelected
        {
            get => _isCategorySelected;
            set
            {
                if (_isCategorySelected != value)
                {
                    _isCategorySelected = value;
                    OnPropertyChanged();
                    UpdateProgress();
                }
            }
        }

        public bool IsDescriptionFilled
        {
            get => _isDescriptionFilled;
            set
            {
                if (_isDescriptionFilled != value)
                {
                    _isDescriptionFilled = value;
                    OnPropertyChanged();
                    UpdateProgress();
                }
            }
        }

        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ProgressBarWidth)); // Notify the width property change

                }
            }
        }

        


        public ObservableCollection<string> Categories { get; set; }

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
                    OnPropertyChanged();
                }
            }
        }

        public bool ContainsPdf => MediaItems.Any(m => m.IsPdf);
        public bool ContainsWord => MediaItems.Any(m => m.IsWord);

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
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
                    OnPropertyChanged();
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

            Categories = new ObservableCollection<string>
            {
                "Roads and Traffic",
                "Public Utilities",
                "Waste Management",
                "Parks and Recreation",
                "Public Safety",
                "Housing and Buildings",
                "Environmental Concerns",
                "Public Transportation",
                "Health and Sanitation",
                "Community Services",
                "Economic Development",
                "Education and Youth Services"
            };
        }

        private void NavigateToHome()
        {
            var reportWindow = Application.Current.Windows.OfType<ReportView>().FirstOrDefault();
            reportWindow?.Close();

            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.Show();
        }

        private string EncodeFileToBase64(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified file does not exist.", filePath);
            }

            string extension = Path.GetExtension(filePath).ToLower();
            var supportedExtensions = new List<string> { ".jpg", ".jpeg", ".png", ".pdf", ".txt", ".docx" };

            if (!supportedExtensions.Contains(extension))
            {
                throw new NotSupportedException("Unsupported file type.");
            }

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
            return outputFilePath;
        }

        private string DecodeBase64ToTextFile(string base64String, string outputFilePath)
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            string fileContent = Encoding.UTF8.GetString(fileBytes);
            File.WriteAllText(outputFilePath, fileContent);
            return fileContent;
        }

        private bool IsImageFile(string filename)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
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
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filename in openFileDialog.FileNames)
                {
                    string base64String = EncodeFileToBase64(filename);
                    bool isImage = IsImageFile(filename);
                    bool isPdf = IsPdfFile(filename);
                    bool isText = IsTextFile(filename);
                    bool isWord = IsWordFile(filename);

                    if (isImage || isPdf || isText || isWord)
                    {
                        MediaItems.Add(new MediaItem
                        {
                            Base64String = base64String,
                            IsImage = isImage,
                            IsText = isText,
                            IsPdf = isPdf,
                            IsWord = isWord
                        });
                    }
                    else
                    {
                        MessageBox.Show($"Unsupported file type: {filename}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show($"Failed to open the file. Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void UpdateProgress()
        {
            int filledFields = 0;
            if (IsIssueNameFilled) filledFields++;
            if (IsLocationFilled) filledFields++;
            if (IsCategorySelected) filledFields++;
            if (IsDescriptionFilled) filledFields++;

            double newProgress = (filledFields / 4.0) * 100; // Assuming there are 4 fields
            Progress = newProgress;
        }

        public double GetProgressBarWidth()
        {
            // Assuming the maximum width of the progress bar container is 300
            double maxWidth = 300;
            double progress = Progress; // Your progress value
            return (progress / 100) * maxWidth; // Adjust as needed
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

      
    }
}
