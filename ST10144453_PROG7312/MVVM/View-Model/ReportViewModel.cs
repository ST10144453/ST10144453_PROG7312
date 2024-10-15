//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
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

        //~~~~~~~~~~~~~ Methods: Default Constructor ~~~~~~~~~~~~~//
        /// <summary>
        /// This constructor initializes the ReportViewModel class.
        /// </summary>
        public ReportViewModel()
        {
            AttachMediaCommand = new RelayCommand(AttachMedia);
            SubmitCommand = new RelayCommand(Submit);
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            MediaItems = new ObservableCollection<MediaItem>();
            Reports = ReportManager.Instance.Reports;

            CurrentUser = UserSession.CurrentUser; // Get the current user

            if (CurrentUser == null)
            {
                Debug.WriteLine("CurrentUser is null in ReportViewModel constructor");
            }
            else
            {
                Debug.WriteLine($"CurrentUser is set in ReportViewModel constructor: {CurrentUser.userName}");

            }
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

            Console.WriteLine($"ReportViewModel initialized with user: {CurrentUser?.userName}");

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
            string[] textExtensions = { ".txt", ".csv", ".log" };
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
            // Validate input fields
            if (string.IsNullOrWhiteSpace(IssueName))
            {
                MessageBox.Show("Please enter a report name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Location))
            {
                MessageBox.Show("Please enter a location.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(SelectedCategory))
            {
                MessageBox.Show("Please select a category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Description))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (CurrentUser == null)
            {
                MessageBox.Show("Error: No user is currently logged in.", "User Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Create new report model
            var newReport = new ReportModel
            {
                reportName = IssueName,
                reportLocation = Location,
                reportDescription = Description,
                reportCategory = SelectedCategory,
                Media = new List<Model.MediaItem>(),
                reportDate = DateTime.Now,
                CreatedBy = CurrentUser.userName // Link the report to the current user

            };

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

            ReportManager.Instance.AddReport(newReport);
            OnPropertyChanged(nameof(Reports));

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

      


        //++++++++++++++ Methods: OnPropertyChanged ++++++++++++++//
        /// <summary>
        /// This method is called when a property is changed.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//