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
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    //============== Class: ReportViewModel ==============//
    /// <summary>
    /// This class holds the base implementation for the ReportViewModel class.
    /// </summary>
    public class ReportViewModel : INotifyPropertyChanged
    {
        //++++++++++++++ Declarations ++++++++++++++//
        /// <summary>
        /// This property holds the issue name.
        /// </summary>
        private string _issueName;

        /// <summary>
        /// This property holds the location.
        /// </summary>
        private string _location;

        /// <summary>
        /// This property holds the selected category.
        /// </summary>
        private string _selectedCategory;

        /// <summary>
        /// This property holds the description.
        /// </summary>
        private string _description;

        /// <summary>
        /// This property determines whether the issue name is filled.
        /// </summary>
        private bool _isIssueNameFilled;

        /// <summary>
        /// This property determines whether the location is filled.
        /// </summary>
        private bool _isLocationFilled;

        /// <summary>
        /// This property determines whether the category is selected.
        /// </summary>
        private bool _isCategorySelected;

        /// <summary>
        /// This property determines whether the description is filled.
        /// </summary>
        private bool _isDescriptionFilled;

        /// <summary>
        /// This property holds the progress.
        /// </summary>
        private double _progress;

        /// <summary>
        /// This property holds the media items.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isPrivate;

        public UserModel CurrentUser { get; set; }

        private ObservableCollection<ReportModel> _filteredReports;
        public ObservableCollection<ReportModel> FilteredReports
        {
            get => _filteredReports;
            set
            {
                _filteredReports = value;
                OnPropertyChanged(nameof(FilteredReports));
            }
        }

        public bool IsPrivate
        {
            get => _isPrivate;
            set
            {
                if (_isPrivate != value)
                {
                    _isPrivate = value;
                    OnPropertyChanged();
                }
            }
        }


        /// <summary>
        /// This property holds the media items.
        /// </summary>
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

        /// <summary>
        /// This property holds the progress value.
        /// </summary>
        private double _progressValue;

        /// <summary>
        /// This property holds the progress value.
        /// </summary>
        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                if (_progressValue != value)
                {
                    _progressValue = value;
                    OnPropertyChanged(nameof(ProgressValue));
                }
            }
        }

        /// <summary>
        /// This property holds the location.
        /// </summary>
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

        /// <summary>
        /// This property holds the selected category.
        /// </summary>
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

        /// <summary>
        /// This property holds the description.
        /// </summary>
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

        /// <summary>
        /// This property determines whether the issue name is filled.
        /// </summary>
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

        /// <summary>
        /// This property determines whether the location is filled.
        /// </summary>
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

        /// <summary>
        /// This property determines whether the category is selected.
        /// </summary>
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

        /// <summary>
        /// This property determines whether the description is filled.
        /// </summary>
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

        /// <summary>
        /// This property holds the progress.
        /// </summary>
        public double Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// This property holds the categories.
        /// </summary>
        public ObservableCollection<string> Categories { get; set; }

        /// <summary>
        /// This property holds the reports.
        /// </summary>
        public ObservableCollection<ReportModel> Reports { get; set; }

        /// <summary>
        /// This property holds the media items.
        /// </summary>
        private ObservableCollection<MediaItem> _mediaItems;

        /// <summary>
        /// This property holds the media items.
        /// </summary>
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

        /// <summary>
        /// This property determines whether the media items contain an image.
        /// </summary>
        public bool ContainsPdf => MediaItems.Any(m => m.IsPdf);

        /// <summary>
        /// This property determines whether the media items contain a text file.
        /// </summary>
        public bool ContainsWord => MediaItems.Any(m => m.IsWord);

        /// <summary>
        /// This property determines whether the media items contain a text file.
        /// </summary>
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

        /// <summary>
        /// This property holds the selected media item.
        /// </summary>
        private MediaItem _selectedMediaItem;

        /// <summary>
        /// This property holds the selected media item. 
        /// </summary>
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

        /// <summary>
        /// This property holds the selected report.
        /// </summary>
        public ICommand AttachMediaCommand { get; private set; }

        /// <summary>
        /// This property holds the selected report.
        /// </summary>
        public ICommand SubmitCommand { get; private set; }

        /// <summary>
        /// This property holds the selected report.
        /// </summary>
        public ICommand NavigateToHomeCommand { get; private set; }

        public ObservableCollection<MediaItem> UploadedFiles
        {
            get => MediaItems;
            set
            {
                MediaItems = value;
                OnPropertyChanged();
            }
        }

        //~~~~~~~~~~~~~ Methods: Default Constructor ~~~~~~~~~~~~~//
        /// <summary>
        /// This constructor initializes the ReportViewModel class.
        /// </summary>
        public ReportViewModel(UserModel currentUser = null)
        {
            CurrentUser = currentUser;
            Categories = new ObservableCollection<string>
            {
                "Infrastructure",
                "Safety",
                "Environmental",
                "Community",
                "Other"
            };
            MediaItems = new ObservableCollection<MediaItem>();
            
            // Initialize Reports from ReportManager
            Reports = ReportManager.Instance.Reports ?? new ObservableCollection<ReportModel>();
            Debug.WriteLine($"ReportViewModel initialized with {Reports.Count} reports");
            
            FilterReportsByCurrentUser();

            AttachMediaCommand = new RelayCommand(AttachMedia);
            SubmitCommand = new RelayCommand(Submit);
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
        }

        private void FilterReportsByCurrentUser()
        {
            Debug.WriteLine($"Filtering reports for user: {CurrentUser?.userName}");
            Debug.WriteLine($"Total reports available: {Reports?.Count ?? 0}");
            
            if (Reports == null)
            {
                FilteredReports = new ObservableCollection<ReportModel>();
                return;
            }

            if (CurrentUser != null)
            {
                var filteredList = Reports.Where(r => 
                {
                    Debug.WriteLine($"Comparing report creator: {r.CreatedBy} with current user: {CurrentUser.userName}");
                    return r.CreatedBy == CurrentUser.userName;
                }).ToList();
                
                FilteredReports = new ObservableCollection<ReportModel>(filteredList);
                Debug.WriteLine($"Filtered reports count after filtering: {FilteredReports.Count}");
            }
            else
            {
                FilteredReports = new ObservableCollection<ReportModel>(Reports);
                Debug.WriteLine("No user provided, showing all reports");
            }
        }

        //++++++++++++++ Methods: NavigateToHome ++++++++++++++//
        /// <summary>
        /// This method handles the NavigateToHome command.
        /// </summary>
        private void NavigateToHome()
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            mainWindow?.Show();
        }

        //++++++++++++++ Methods: EncodeFileToBase64 ++++++++++++++//
        /// <summary>
        /// This method encodes a file to a base64 string.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
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

        //++++++++++++++ Methods: EncodeTextFileToBase64 ++++++++++++++//
        /// <summary>
        /// This method encodes a text file to a base64 string.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string EncodeTextFileToBase64(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            byte[] fileBytes = Encoding.UTF8.GetBytes(fileContent);
            return Convert.ToBase64String(fileBytes);
        }

        //++++++++++++++ Methods: DecodeBase64ToFile ++++++++++++++//
        /// <summary>
        /// This method decodes a base64 string to a file.
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="outputFilePath"></param>
        /// <returns></returns>
        private string DecodeBase64ToFile(string base64String, string outputFilePath)
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(outputFilePath, fileBytes);
            return outputFilePath;
        }

        //++++++++++++++ Methods: DecodeBase64ToTextFile ++++++++++++++//
        /// <summary>
        /// This method decodes a base64 string to a text file.
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="outputFilePath"></param>
        /// <returns></returns>
        private string DecodeBase64ToTextFile(string base64String, string outputFilePath)
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            string fileContent = Encoding.UTF8.GetString(fileBytes);
            File.WriteAllText(outputFilePath, fileContent);
            return fileContent;
        }

        //++++++++++++++ Methods: IsImageFile ++++++++++++++//
        /// <summary>
        /// This method determines if the file is an image.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool IsImageFile(string filename)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg" };
            return imageExtensions.Contains(Path.GetExtension(filename).ToLower());
        }

        //++++++++++++++ Methods: IsTextFile ++++++++++++++//
        /// <summary>
        /// This method determines if the file is a text file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool IsTextFile(string filename)
        {
            string[] textExtensions = { ".txt"};
            return textExtensions.Contains(Path.GetExtension(filename).ToLower());
        }

        //++++++++++++++ Methods: IsPdfFile ++++++++++++++//
        /// <summary>
        /// This method determines if the file is a PDF file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool IsPdfFile(string filename)
        {
            return Path.GetExtension(filename).ToLower() == ".pdf";
        }

        //++++++++++++++ Methods: IsWordFile ++++++++++++++//
        /// <summary>
        /// This method determines if the file is a Word file.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool IsWordFile(string filename)
        {
            string[] wordExtensions = { ".doc", ".docx" };
            return wordExtensions.Contains(Path.GetExtension(filename).ToLower());
        }

        //++++++++++++++ Methods: AttachMedia ++++++++++++++//
        /// <summary>
        /// This method attaches media to the report.
        /// </summary>
        private void AttachMedia()
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg|Text Files (*.txt;*.csv;*.log)|*.txt;*.csv;*.log|PDF Files (*.pdf)|*.pdf|Word Files (*.doc;*.docx)|*.doc;*.docx|All Files (*.*)|*.*"
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

        //++++++++++++++ Methods: TryOpenFile ++++++++++++++//
        /// <summary>
        /// This method tries to open a file.
        /// </summary>
        /// <param name="base64String"></param>
        /// <param name="fileExtension"></param>
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

        //++++++++++++++ Methods: Submit ++++++++++++++//
        /// <summary>
        /// This method submits the report.
        /// </summary>
        private void Submit()
        {
            bool isValid = true;

            // Reset all error states
            ShowIssueNameError = false;
            ShowLocationError = false;
            ShowCategoryError = false;
            ShowDescriptionError = false;

            // Validate Issue Name
            if (string.IsNullOrWhiteSpace(IssueName))
            {
                IssueNameError = "Please enter a report name";
                ShowIssueNameError = true;
                isValid = false;
            }

            // Validate Location
            if (string.IsNullOrWhiteSpace(Location))
            {
                LocationError = "Please enter a location";
                ShowLocationError = true;
                isValid = false;
            }

            // Validate Category
            if (string.IsNullOrWhiteSpace(SelectedCategory))
            {
                CategoryError = "Please select a category";
                ShowCategoryError = true;
                isValid = false;
            }

            // Validate Description
            if (string.IsNullOrWhiteSpace(Description))
            {
                DescriptionError = "Please enter a description";
                ShowDescriptionError = true;
                isValid = false;
            }

            // Check if user is logged in
            if (CurrentUser == null)
            {
                MessageBox.Show("Error: No user is currently logged in.", "User Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!isValid)
            {
                return;
            }

            Debug.WriteLine($"Creating report with user: {CurrentUser?.userName}");
            var newReport = new ReportModel
            {
                reportName = IssueName,
                reportLocation = Location,
                reportCategory = SelectedCategory,
                reportDescription = Description,
                reportDate = DateTime.Now,
                CreatedBy = CurrentUser?.userName,
                Media = new List<MediaItem>(MediaItems)
            };

            // Add the report using ReportManager
            ReportManager.Instance.AddReport(newReport);
            Debug.WriteLine($"Added report to ReportManager. Current count: {ReportManager.Instance.Reports.Count}");

            string reportGuid = Guid.NewGuid().ToString();
            string reportFolder = Path.Combine("Files", reportGuid);
            Directory.CreateDirectory(reportFolder);

            // Handle media items
            foreach (var mediaItem in MediaItems)
            {
                string fileName = Guid.NewGuid().ToString();
                string destinationPath = Path.Combine(reportFolder, fileName);

                if (mediaItem.IsImage)
                {
                    DecodeBase64ToFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsImage = true });
                }
                else if (mediaItem.IsPdf)
                {
                    DecodeBase64ToFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsPdf = true });
                }
                else if (mediaItem.IsWord)
                {
                    DecodeBase64ToFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsWord = true });
                }
                else
                {
                    DecodeBase64ToTextFile(mediaItem.Base64String, destinationPath);
                    newReport.Media.Add(new Model.MediaItem { Base64String = mediaItem.Base64String, IsText = true });
                }
            }

            ReportService.Instance.AddReport(newReport); 
            OnPropertyChanged(nameof(Reports));
    RefreshReports();

            Debug.WriteLine($"New report added by user: {CurrentUser?.userName}");


            // Debug output
            Debug.WriteLine($"New report added:\nName: {newReport.reportName}\nLocation: {newReport.reportLocation}\nCategory: {newReport.reportCategory}\nDescription: {newReport.reportDescription}\nMedia count: {MediaItems.Count}\nUser: {CurrentUser?.userName}");


            foreach (var mediaItem in MediaItems)
            {
                Console.WriteLine($"Base64String length: {mediaItem.Base64String.Length}");
                Console.WriteLine($"IsImage: {mediaItem.IsImage}");
                Console.WriteLine($"IsText: {mediaItem.IsText}");
                Console.WriteLine($"IsPdf: {mediaItem.IsPdf}");
                Console.WriteLine($"IsWord: {mediaItem.IsWord}");
            }

            // Clear inputs
            IssueName = string.Empty;
            Location = string.Empty;
            Description = string.Empty;
            SelectedCategory = string.Empty;
            MediaItems.Clear();

            // Refresh the reports
            Reports = ReportManager.Instance.Reports;
            FilterReportsByCurrentUser();
        }

        //++++++++++++++ Methods: UpdateProgress ++++++++++++++//
        /// <summary>
        /// This method updates the progress.
        /// </summary>
        private void UpdateProgress()
        {
            int filledFields = 0;
            if (IsIssueNameFilled) filledFields++;
            if (IsLocationFilled) filledFields++;
            if (IsCategorySelected) filledFields++;
            if (IsDescriptionFilled) filledFields++;

            double newProgress = (filledFields / 4.0) * 100;
            Progress = newProgress;
        }

      public void RefreshReports()
{
    Reports = ReportManager.Instance.Reports;
    Debug.WriteLine($"Reports refreshed. Total count: {Reports.Count}");
    FilterReportsByCurrentUser();
}


        //++++++++++++++ Methods: OnPropertyChanged ++++++++++++++//
        /// <summary>
        /// This method is called when a property is changed.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Add these properties after the existing private fields
        private string _issueNameError;
        private string _locationError;
        private string _categoryError;
        private string _descriptionError;
        private bool _showIssueNameError;
        private bool _showLocationError;
        private bool _showCategoryError;
        private bool _showDescriptionError;

        // Add these public properties
        public string IssueNameError
        {
            get => _issueNameError;
            set
            {
                _issueNameError = value;
                OnPropertyChanged();
            }
        }

        public string LocationError
        {
            get => _locationError;
            set
            {
                _locationError = value;
                OnPropertyChanged();
            }
        }

        public string CategoryError
        {
            get => _categoryError;
            set
            {
                _categoryError = value;
                OnPropertyChanged();
            }
        }

        public string DescriptionError
        {
            get => _descriptionError;
            set
            {
                _descriptionError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowIssueNameError
        {
            get => _showIssueNameError;
            set
            {
                _showIssueNameError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowLocationError
        {
            get => _showLocationError;
            set
            {
                _showLocationError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowCategoryError
        {
            get => _showCategoryError;
            set
            {
                _showCategoryError = value;
                OnPropertyChanged();
            }
        }

        public bool ShowDescriptionError
        {
            get => _showDescriptionError;
            set
            {
                _showDescriptionError = value;
                OnPropertyChanged();
            }
        }
    }
}