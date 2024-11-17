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
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ServiceRequestViewModel : INotifyPropertyChanged
    {
        private readonly ServiceRequestTree _requestTree;
        private ObservableCollection<ServiceRequestModel> _filteredRequests;
        private string _selectedCategory;
        private DateTime? _startDate;
        private DateTime? _endDate;
        private ObservableCollection<string> _categories;
        private string _firstName;
        private string _surname;
        private string _email;
        private string _phoneNumber;
        private string _category;
        private string _description;
        private string _additionalAddress;
        private string _preferredFeedbackMethod;
        private ObservableCollection<MediaItem> _supportingEvidence;
        private bool _isFormValid;
        private ICommand _addMediaCommand;
        private ICommand _selectReportsCommand;
        private ICommand _submitRequestCommand;
        private ServiceRequestModel _recentRequest;
        private readonly Window _popupWindow;

        public ObservableCollection<string> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public ObservableCollection<ServiceRequestModel> FilteredRequests
        {
            get => _filteredRequests;
            set
            {
                _filteredRequests = value;
                OnPropertyChanged(nameof(FilteredRequests));
            }
        }

        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
                ApplyFilters();
            }
        }

        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged(nameof(StartDate));
                ApplyFilters();
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
                ApplyFilters();
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
                ValidateForm();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
                ValidateForm();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
                ValidateForm();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                ValidateForm();
            }
        }

        public string Category
        {
            get => _category;
            set
            {
                _category = value;
                OnPropertyChanged(nameof(Category));
                ValidateForm();
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
                ValidateForm();
            }
        }

        public string AdditionalAddress
        {
            get => _additionalAddress;
            set
            {
                _additionalAddress = value;
                OnPropertyChanged(nameof(AdditionalAddress));
                ValidateForm();
            }
        }

        public string PreferredFeedbackMethod
        {
            get => _preferredFeedbackMethod;
            set
            {
                _preferredFeedbackMethod = value;
                OnPropertyChanged(nameof(PreferredFeedbackMethod));
            }
        }

        public ObservableCollection<MediaItem> SupportingEvidence
        {
            get => _supportingEvidence;
            set
            {
                _supportingEvidence = value;
                OnPropertyChanged(nameof(SupportingEvidence));
            }
        }

        public bool IsFormValid
        {
            get => _isFormValid;
            private set
            {
                _isFormValid = value;
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public ObservableCollection<string> FeedbackMethods { get; } = new ObservableCollection<string>
        {
            "Email",
            "Phone",
            "SMS"
        };

        public ICommand AddMediaCommand => _addMediaCommand ?? (_addMediaCommand = new RelayCommand(ExecuteAddMedia));
        public ICommand SelectReportsCommand => _selectReportsCommand ?? (_selectReportsCommand = new RelayCommand(ExecuteSelectReports));
        public ICommand SubmitRequestCommand => _submitRequestCommand ?? (_submitRequestCommand = new RelayCommand(ExecuteSubmitRequest, CanExecuteSubmitRequest));
        public ICommand NavigateToDashboardCommand { get; private set; }

        public ServiceRequestModel RecentRequest
        {
            get => _recentRequest;
            set
            {
                _recentRequest = value;
                OnPropertyChanged(nameof(RecentRequest));
            }
        }

        public string RequestDetails => RecentRequest != null
            ? $"Category: {RecentRequest.Category}\n" +
              $"Description: {RecentRequest.Description}\n" +
              $"Request ID: {RecentRequest.RequestID}\n" +
              $"Status: {RecentRequest.Status}\n" +
              $"Submitted: {RecentRequest.RequestDate}"
            : string.Empty;

        public ServiceRequestViewModel(ServiceRequestModel request, Window popupWindow)
        {
            _popupWindow = popupWindow;
            RecentRequest = request;
            NavigateToDashboardCommand = new RelayCommand(NavigateToDashboard);
            _requestTree = new ServiceRequestTree();
            Categories = new ObservableCollection<string>(ServiceRequestManager.Instance.GetAllCategories());
            InitializeData();
            SupportingEvidence = new ObservableCollection<MediaItem>();
            InitializeCommands();
            _addMediaCommand = new RelayCommand(ExecuteAddMedia);
            _selectReportsCommand = new RelayCommand(ExecuteSelectReports);
            _submitRequestCommand = new RelayCommand(ExecuteSubmitRequest, CanExecuteSubmitRequest);
        }

        private void InitializeData()
        {
            // Load existing service requests into the tree
            foreach (var request in ServiceRequestManager.Instance.GetAllRequests())
            {
                _requestTree.Insert(request);
            }
            
            FilteredRequests = new ObservableCollection<ServiceRequestModel>(_requestTree.GetAllRequests());
        }

        private void ApplyFilters()
        {
            IEnumerable<ServiceRequestModel> requests = _requestTree.GetAllRequests();

            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                requests = _requestTree.GetRequestsByCategory(SelectedCategory);
            }

            if (StartDate.HasValue || EndDate.HasValue)
            {
                var start = StartDate ?? DateTime.MinValue;
                var end = EndDate ?? DateTime.MaxValue;
                requests = _requestTree.GetRequestsByDateRange(start, end);
            }

            FilteredRequests = new ObservableCollection<ServiceRequestModel>(requests);
        }

        public void SubmitRequest(ServiceRequestModel request)
        {
            ServiceRequestManager.Instance.AddRequest(request);
            _requestTree.Insert(request);
            ApplyFilters(); // Refresh the filtered list
        }

        private void InitializeCommands()
        {
            _addMediaCommand = new RelayCommand(ExecuteAddMedia);
            _selectReportsCommand = new RelayCommand(ExecuteSelectReports);
            _submitRequestCommand = new RelayCommand(ExecuteSubmitRequest, CanExecuteSubmitRequest);
        }

        private void ExecuteAddMedia()
        {
            // Implement media addition logic
        }

        private void ExecuteSelectReports()
        {
            // Implement report selection logic
        }

        private void ExecuteSubmitRequest()
        {
            var request = new ServiceRequestModel
            {
                RequestID = Guid.NewGuid(),
                FirstName = FirstName,
                Surname = Surname,
                Email = Email,
                PhoneNumber = PhoneNumber,
                Category = Category,
                Description = Description,
                AdditionalAddress = AdditionalAddress,
                PreferredFeedbackMethod = PreferredFeedbackMethod,
                RequestDate = DateTime.Now,
                Status = "Pending",
                CreatedBy = UserSession.CurrentUser?.userName
            };

            ServiceRequestManager.Instance.AddRequest(request);
            RecentRequest = request;

            var popup = new ServiceRequestSubmissionPopup(request);
            popup.Owner = Application.Current.MainWindow;
            popup.ShowDialog();

            ClearForm();
        }

        private bool CanExecuteSubmitRequest()
        {
            return IsFormValid;
        }

        private void ValidateForm()
        {
            IsFormValid = !string.IsNullOrWhiteSpace(FirstName) &&
                         !string.IsNullOrWhiteSpace(Surname) &&
                         !string.IsNullOrWhiteSpace(Email) &&
                         !string.IsNullOrWhiteSpace(PhoneNumber) &&
                         !string.IsNullOrWhiteSpace(Category) &&
                         !string.IsNullOrWhiteSpace(Description);
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            Surname = string.Empty;
            Email = string.Empty;
            PhoneNumber = string.Empty;
            Category = null;
            Description = string.Empty;
            AdditionalAddress = string.Empty;
            PreferredFeedbackMethod = null;
            SupportingEvidence.Clear();
        }

        private void NavigateToDashboard()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                _popupWindow?.Close();
                var dashboardView = new UserDashboardUserControl(UserSession.CurrentUser);
                mainWindow.MainContentControl.Content = dashboardView;
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
