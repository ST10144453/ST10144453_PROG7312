using Microsoft.Win32;
using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class MediaItem
    {
        public string Base64String { get; set; }
        public bool IsImage { get; set; }
    }
    public class ReportViewModel : INotifyPropertyChanged
    {
        private string _issueName;
        private string _location;
        private string _selectedCategory;
        private string _description;
        private ObservableCollection<ReportModel> _reports;
        private ObservableCollection<string> _mediaPaths;
        private ObservableCollection<string> _savedMediaPaths;
        private ObservableCollection<string> _mediaBase64Strings;
        private ObservableCollection<string> _savedMediaBase64Strings;

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

        public ObservableCollection<ReportModel> Reports
        {
            get => _reports;
            set
            {
                if (_reports != value)
                {
                    _reports = value;
                    OnPropertyChanged(nameof(Reports));
                }
            }
        }

        public ObservableCollection<string> MediaPaths
        {
            get => _mediaPaths;
            set
            {
                if (_mediaPaths != value)
                {
                    _mediaPaths = value;
                    OnPropertyChanged(nameof(MediaPaths));
                }
            }
        }

        public ObservableCollection<string> SavedMediaPaths
        {
            get => _savedMediaPaths;
            set
            {
                if (_savedMediaPaths != value)
                {
                    _savedMediaPaths = value;
                    OnPropertyChanged(nameof(SavedMediaPaths));
                }
            }
        }

        public ObservableCollection<string> MediaBase64Strings
        {
            get => _mediaBase64Strings;
            set
            {
                if (_mediaBase64Strings != value)
                {
                    _mediaBase64Strings = value;
                    OnPropertyChanged(nameof(MediaBase64Strings));
                }
            }
        }

        public ObservableCollection<string> SavedMediaBase64Strings
        {
            get => _savedMediaBase64Strings;
            set
            {
                if (_savedMediaBase64Strings != value)
                {
                    _savedMediaBase64Strings = value;
                    OnPropertyChanged(nameof(SavedMediaBase64Strings));
                }
            }
        }

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

        public ICommand AttachMediaCommand { get; private set; }
        public ICommand SubmitCommand { get; private set; }
        public ICommand NavigateToHomeCommand { get; private set; }

        public ReportViewModel()
        {
            Reports = new ObservableCollection<ReportModel>();
            MediaPaths = new ObservableCollection<string>();
            SavedMediaPaths = new ObservableCollection<string>();
            MediaBase64Strings = new ObservableCollection<string>();
            SavedMediaBase64Strings = new ObservableCollection<string>();

            AttachMediaCommand = new RelayCommand(AttachMedia);
            SubmitCommand = new RelayCommand(Submit);
            NavigateToHomeCommand = new RelayCommand(NavigateToHome);
            MediaItems = new ObservableCollection<MediaItem>();

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
            byte[] fileBytes = File.ReadAllBytes(filePath);
            return Convert.ToBase64String(fileBytes);
        }

        private void DecodeBase64ToFile(string base64String, string outputFilePath)
        {
            byte[] fileBytes = Convert.FromBase64String(base64String);
            File.WriteAllBytes(outputFilePath, fileBytes);
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
                    MediaItems.Add(new MediaItem { Base64String = base64String, IsImage = isImage });
                }
            }
        }

        private bool IsImageFile(string filename)
        {
            string[] imageExtensions = { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
            return imageExtensions.Contains(Path.GetExtension(filename).ToLower());
        }

        private void Submit()
        {
            var newReport = new ReportModel
            {
                reportName = IssueName,
                reportLocation = Location,
                reportDescription = Description,
                reportCategory = SelectedCategory,
                Media = new List<string>()
            };

            string reportGuid = Guid.NewGuid().ToString();
            string reportFolder = Path.Combine("Files", reportGuid);
            Directory.CreateDirectory(reportFolder);

            foreach (var base64String in MediaBase64Strings)
            {
                string fileName = Guid.NewGuid().ToString();
                string destinationPath = Path.Combine(reportFolder, fileName);
                DecodeBase64ToFile(base64String, destinationPath);
                newReport.Media.Add(destinationPath);
                SavedMediaBase64Strings.Add(base64String);
            }

            Reports.Add(newReport);

            IssueName = string.Empty;
            Location = string.Empty;
            Description = string.Empty;
            SelectedCategory = null;
            MediaBase64Strings.Clear();

            Console.WriteLine("User input has been saved.");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
