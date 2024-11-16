using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using ST10144453_PROG7312.MVVM.View;
using Microsoft.Maps.MapControl.WPF;
using System.Text.RegularExpressions;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ServiceRequestViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _surname;
        private string _email;
        private string _phoneNumber;
        private string _category;
        private string _description;
        private string _plottingPoint;
        private string _additionalAddress;
        private string _preferredFeedbackMethod;
        private ObservableCollection<ReportModel> _availableReports;
        private ObservableCollection<ReportModel> _selectedReports;
        private ObservableCollection<MediaItem> _mediaItems;
        private UserModel _currentUser;
        private string _selectedCategory;
        private bool _isFormValid;
        private Location _selectedLocation;
        private Pushpin _locationPin;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SubmitRequestCommand { get; private set; }
        public ICommand AddMediaCommand { get; private set; }
        public ICommand SelectReportsCommand { get; private set; }
        public ICommand RemoveMediaCommand { get; private set; }
        public ICommand RemoveReportCommand { get; private set; }

        public ServiceRequestViewModel()
        {
            InitializeCollections();
            InitializeCommands();
            _currentUser = UserSession.CurrentUser;
        }

        private void InitializeCollections()
        {
            _availableReports = new ObservableCollection<ReportModel>(ReportManager.Instance.Reports);
            _selectedReports = new ObservableCollection<ReportModel>();
            _mediaItems = new ObservableCollection<MediaItem>();

            ReportManager.Instance.Reports.CollectionChanged += (s, e) =>
            {
                RefreshAvailableReports();
            };
        }

        private void InitializeCommands()
        {
            SubmitRequestCommand = new RelayCommand(SubmitRequest, CanSubmitRequest);
            AddMediaCommand = new RelayCommand(AddMedia);
            SelectReportsCommand = new RelayCommand(SelectReports);
            RemoveMediaCommand = new RelayCommand<MediaItem>(RemoveMedia);
            RemoveReportCommand = new RelayCommand<ReportModel>(RemoveReport);
        }

        public ObservableCollection<MediaItem> MediaItems
        {
            get => _mediaItems;
            set
            {
                _mediaItems = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReportModel> SelectedReports
        {
            get => _selectedReports;
            set
            {
                _selectedReports = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReportModel> AvailableReports
        {
            get => _availableReports;
            set
            {
                _availableReports = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public ObservableCollection<string> Categories { get; } = new ObservableCollection<string>
            {
                "Infrastructure",
                "Public Safety",
                "Environmental",
                "Community Services",
                "Other"
            };

        public ObservableCollection<string> FeedbackMethods { get; } = new ObservableCollection<string>
            {
                "Email",
                "Phone",
                "SMS"
            };

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public bool IsFormValid
        {
            get => _isFormValid;
            set
            {
                _isFormValid = value;
                OnPropertyChanged();
            }
        }

        public string AdditionalAddress
        {
            get => _additionalAddress;
            set
            {
                _additionalAddress = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public string PreferredFeedbackMethod
        {
            get => _preferredFeedbackMethod;
            set
            {
                _preferredFeedbackMethod = value;
                OnPropertyChanged();
                ValidateForm();
            }
        }

        public ObservableCollection<MediaItem> SupportingEvidence
        {
            get => MediaItems;
            set
            {
                MediaItems = value;
                OnPropertyChanged();
            }
        }

        private bool CanSubmitRequest()
        {
            return IsFormValid;
        }

        private void ValidateForm()
        {
            IsFormValid = !string.IsNullOrWhiteSpace(FirstName) &&
                         !string.IsNullOrWhiteSpace(Surname) &&
                         !string.IsNullOrWhiteSpace(Email) &&
                         !string.IsNullOrWhiteSpace(SelectedCategory) &&
                         !string.IsNullOrWhiteSpace(Description) &&
                         IsValidEmail(Email) &&
                         IsValidPhoneNumber(PhoneNumber);

            (SubmitRequestCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }

        private bool IsValidEmail(string email)
        {
            return email.Contains("@");
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            // Remove any whitespace or special characters from the phone number
            phoneNumber = Regex.Replace(phoneNumber, @"[^0-9]", "");

            // Check if the phone number consists of only numbers
            return Regex.IsMatch(phoneNumber, @"^[0-9]+$");
        }

        private async void AddMedia()
        {
            var fileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files|*.png;*.jpg;*.jpeg;*.pdf;*.doc;*.docx|Images|*.png;*.jpg;*.jpeg|Documents|*.pdf;*.doc;*.docx"
            };

            if (fileDialog.ShowDialog() == true)
            {
                foreach (string filename in fileDialog.FileNames)
                {
                    var mediaItem = await MediaItem.FromFileAsync(filename);
                    if (mediaItem != null)
                    {
                        MediaItems.Add(mediaItem);
                    }
                }
            }
        }

        private void SelectReports()
        {
            var dialog = new SelectReportsDialog(_availableReports, _selectedReports);
            if (dialog.ShowDialog() == true)
            {
                foreach (var report in dialog.SelectedReports)
                {
                    if (!_selectedReports.Contains(report))
                    {
                        _selectedReports.Add(report);
                    }
                }
            }
        }

        private void RemoveMedia(MediaItem item)
        {
            if (item != null)
            {
                MediaItems.Remove(item);
            }
        }

        private void RemoveReport(ReportModel report)
        {
            if (report != null)
            {
                SelectedReports.Remove(report);
            }
        }

        private void SubmitRequest()
        {
            var request = new ServiceRequestModel
            {
                FirstName = FirstName,
                Surname = Surname,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Category = SelectedCategory,
                Description = Description,
                PlottingPoint = _plottingPoint,
                AdditionalAddress = _additionalAddress,
                PreferredFeedbackMethod = _preferredFeedbackMethod,
                LinkedReports = new List<ReportModel>(_selectedReports),
                SupportingEvidence = new List<MediaItem>(_mediaItems),
                CreatedBy = _currentUser?.userName,
                Status = "Pending"
            };

            ServiceRequestManager.Instance.AddServiceRequest(request);
            ClearForm();
        }

        private void RefreshAvailableReports()
        {
            var currentlySelectedReports = new HashSet<Guid>(_selectedReports.Select(r => r.reportID));
            AvailableReports = new ObservableCollection<ReportModel>(
                ReportManager.Instance.Reports.Where(r => !currentlySelectedReports.Contains(r.reportID))
            );
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            SelectedCategory = string.Empty;
            Description = string.Empty;
            _plottingPoint = string.Empty;
            _additionalAddress = string.Empty;
            _preferredFeedbackMethod = string.Empty;
            MediaItems.Clear();
            SelectedReports.Clear();
            RefreshAvailableReports();
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}