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
        private IServiceRequestTree _currentTree;
        private TreeType _selectedTreeType;
        private SortingStrategy _selectedSortStrategy;
        private ObservableCollection<ServiceRequestModel> _filteredRequests;
        private string _treeDescription;
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
        public UserModel CurrentUser { get; private set; }
        private string _title;
        private ServiceRequestModel selectedRequest;
        private ObservableCollection<ServiceRequestModel> relatedRequests;
        private readonly IServiceRequestTraversal requestTraversal;
        private ICommand viewRelatedRequestCommand;
        private bool _isTitleFilled;
        private bool _isLocationFilled;
        private bool _isCategorySelected;
        private bool _isDescriptionFilled;

        public ObservableCollection<TreeType> AvailableTreeTypes { get; }
        public ObservableCollection<SortingStrategy> AvailableSortStrategies { get; }

        public TreeType SelectedTreeType
        {
            get => _selectedTreeType;
            set
            {
                _selectedTreeType = value;
                UpdateTreeImplementation();
                OnPropertyChanged(nameof(SelectedTreeType));
            }
        }

        public SortingStrategy SelectedSortStrategy
        {
            get => _selectedSortStrategy;
            set
            {
                _selectedSortStrategy = value;
                if (_currentTree != null)
                {
                    _currentTree.SetSortingStrategy(value);
                    RefreshRequests();
                }
                OnPropertyChanged(nameof(SelectedSortStrategy));
            }
        }

        public string TreeDescription
        {
            get => _treeDescription;
            set
            {
                _treeDescription = value;
                OnPropertyChanged(nameof(TreeDescription));
            }
        }

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

        public ICommand AddMediaCommand { get; private set; }
        public ICommand SelectReportsCommand { get; private set; }
        public ICommand SubmitRequestCommand { get; private set; }
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

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
                ValidateForm();
            }
        }

        public ServiceRequestModel SelectedRequest
        {
            get => selectedRequest;
            set
            {
                selectedRequest = value;
                if (value != null)
                {
                    UpdateRelatedRequests(value);
                }
                OnPropertyChanged(nameof(SelectedRequest));
            }
        }

        public ObservableCollection<ServiceRequestModel> RelatedRequests
        {
            get => relatedRequests;
            set
            {
                relatedRequests = value;
                OnPropertyChanged(nameof(RelatedRequests));
            }
        }

        public ICommand ViewRelatedRequestCommand
        {
            get
            {
                if (viewRelatedRequestCommand == null)
                {
                    viewRelatedRequestCommand = new RelayCommand<ServiceRequestModel>(ShowRequestDetails);
                }
                return viewRelatedRequestCommand;
            }
        }

        public bool IsTitleFilled
        {
            get => _isTitleFilled;
            set
            {
                _isTitleFilled = value;
                OnPropertyChanged(nameof(IsTitleFilled));
                OnPropertyChanged(nameof(IsFormValid));
            }
        }

        public bool IsLocationFilled
        {
            get => _isLocationFilled;
            set
            {
                _isLocationFilled = value;
                OnPropertyChanged(nameof(IsLocationFilled));
                ValidateForm();
            }
        }

        public bool IsCategorySelected
        {
            get => _isCategorySelected;
            set
            {
                _isCategorySelected = value;
                OnPropertyChanged(nameof(IsCategorySelected));
                ValidateForm();
            }
        }

        public bool IsDescriptionFilled
        {
            get => _isDescriptionFilled;
            set
            {
                _isDescriptionFilled = value;
                OnPropertyChanged(nameof(IsDescriptionFilled));
                ValidateForm();
            }
        }

        public ServiceRequestViewModel(ServiceRequestModel request, Window popupWindow, UserModel currentUser)
        {
            _popupWindow = popupWindow;
            CurrentUser = currentUser;
            RecentRequest = request;
            
            // Initialize collections
            Categories = new ObservableCollection<string>
            {
                "Water & Sanitation",
                "Electricity",
                "Roads & Transport",
                "Parks & Recreation",
                "Waste Management",
                "Public Safety",
                "Housing",
                "Environmental Issues",
                "Other"
            };

            FeedbackMethods = new ObservableCollection<string>
            {
                "Email",
                "Phone",
                "SMS",
                "WhatsApp"
            };

            // Initialize commands
            SubmitRequestCommand = new RelayCommand(ExecuteSubmitRequest, CanExecuteSubmitRequest);
            NavigateToDashboardCommand = new RelayCommand(NavigateToDashboard);
            
            // Initialize properties from request
            Title = request.Title;
            FirstName = request.FirstName;
            Surname = request.Surname;
            Email = request.Email;
            PhoneNumber = request.PhoneNumber;
            Category = request.Category;
            Description = request.Description;
            AdditionalAddress = request.AdditionalAddress;
            PreferredFeedbackMethod = request.PreferredFeedbackMethod;
        }

        private void InitializeData()
        {
            // Load existing service requests into the tree
            foreach (var request in ServiceRequestManager.Instance.GetAllRequests())
            {
                _currentTree.Insert(request);
            }
            
            FilteredRequests = new ObservableCollection<ServiceRequestModel>(_currentTree.GetAllRequests());
        }

        private void ApplyFilters()
        {
            IEnumerable<ServiceRequestModel> requests = _currentTree.GetAllRequests();

            if (!string.IsNullOrEmpty(SelectedCategory))
            {
                requests = _currentTree.GetRequestsByCategory(SelectedCategory);
            }

            if (StartDate.HasValue || EndDate.HasValue)
            {
                var start = StartDate ?? DateTime.MinValue;
                var end = EndDate ?? DateTime.MaxValue;
                requests = _currentTree.GetRequestsByDateRange(start, end);
            }

            FilteredRequests = new ObservableCollection<ServiceRequestModel>(requests);
        }

        public void SubmitRequest(ServiceRequestModel request)
        {
            ServiceRequestManager.Instance.AddRequest(request);
            _currentTree.Insert(request);
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
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files (*.*)|*.*|Images (*.jpg, *.png)|*.jpg;*.png|Documents (*.pdf, *.doc)|*.pdf;*.doc"
            };

            if (dialog.ShowDialog() == true)
            {
                foreach (string filename in dialog.FileNames)
                {
                    try
                    {
                        var mediaItem = new MediaItem
                        {
                            FilePath = filename,
                            FileName = System.IO.Path.GetFileName(filename)
                        };
                        SupportingEvidence.Add(mediaItem);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding media: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ExecuteSelectReports()
        {
            var reportSelector = new SelectReportsDialog(CurrentUser);
            if (reportSelector.ShowDialog() == true)
            {
                var selectedReports = reportSelector.SelectedReports;
                foreach (var report in selectedReports)
                {
                    LinkReport(report);
                }
            }
        }

        private void ExecuteSubmitRequest()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Please enter a title for the service request.", "Missing Title", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var request = new ServiceRequestModel
            {
                Title = Title,
                Category = Category,
                Description = Description,
                RequestDate = DateTime.Now,
                Status = "Pending",
                CreatedBy = UserSession.CurrentUser?.userName,
                FirstName = FirstName,
                Surname = Surname,
                Email = Email,
                PhoneNumber = PhoneNumber,
                AdditionalAddress = AdditionalAddress,
                PreferredFeedbackMethod = PreferredFeedbackMethod,
                AttachedFiles = RecentRequest?.AttachedFiles ?? new List<AttachedFile>(),
                LinkedReports = RecentRequest?.LinkedReports ?? new List<ReportModel>()
            };

            // Save the request
            ServiceRequestManager.Instance.AddRequest(request);
            
            // Update RecentRequest
            RecentRequest = request;
            OnPropertyChanged(nameof(RecentRequest));
            
            // Show submission popup
            var submissionPopup = new ServiceRequestSubmissionPopup(request, CurrentUser);
            submissionPopup.Owner = _popupWindow;
            submissionPopup.ShowDialog();

            // Close the current window
            _popupWindow?.Close();
        }


        private bool CanExecuteSubmitRequest()
        {
            return IsFormValid;
        }

        private void ValidateForm()
        {
            IsFormValid = !string.IsNullOrWhiteSpace(Title) &&
                          !string.IsNullOrWhiteSpace(Category) &&
                          !string.IsNullOrWhiteSpace(Description) &&
                          !string.IsNullOrWhiteSpace(FirstName) &&
                          !string.IsNullOrWhiteSpace(Surname) &&
                          !string.IsNullOrWhiteSpace(Email) &&
                          !string.IsNullOrWhiteSpace(PhoneNumber) &&
                          !string.IsNullOrWhiteSpace(PreferredFeedbackMethod);
            
            OnPropertyChanged(nameof(IsFormValid));
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
                var dashboardView = new UserDashboardUserControl(CurrentUser);
                mainWindow.MainContentControl.Content = dashboardView;
                
                // Close all popup windows
                foreach (Window window in Application.Current.Windows)
                {
                    if (window != mainWindow && window is ServiceRequestSubmissionPopup)
                    {
                        window.Close();
                    }
                }
            }
        }

        public void AttachFile(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                var fileBytes = File.ReadAllBytes(filePath);
                var base64Content = Convert.ToBase64String(fileBytes);

                var attachedFile = new AttachedFile
                {
                    FileName = fileInfo.Name,
                    FileContent = base64Content,
                    FileType = fileInfo.Extension
                };

                if (RecentRequest.AttachedFiles == null)
                    RecentRequest.AttachedFiles = new List<AttachedFile>();
                    
                RecentRequest.AttachedFiles.Add(attachedFile);
                OnPropertyChanged(nameof(RecentRequest));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error attaching file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void LinkReport(ReportModel report)
        {
            if (RecentRequest.LinkedReports == null)
                RecentRequest.LinkedReports = new List<ReportModel>();
                
            if (!RecentRequest.LinkedReports.Contains(report))
            {
                RecentRequest.LinkedReports.Add(report);
                OnPropertyChanged(nameof(RecentRequest));
            }
        }

        private void UpdateTreeImplementation()
        {
            // Store existing requests
            var existingRequests = _currentTree?.GetAllRequests().ToList() ?? new List<ServiceRequestModel>();

            // Create new tree
            _currentTree = ServiceRequestTreeFactory.CreateTree(SelectedTreeType);
            
            // Set sorting strategy
            _currentTree.SetSortingStrategy(SelectedSortStrategy);
            
            // Repopulate tree
            foreach (var request in existingRequests)
            {
                _currentTree.Insert(request);
            }

            // Update description
            TreeDescription = _currentTree.GetTreeDescription();

            // Refresh display
            RefreshRequests();
        }

        private void RefreshRequests()
        {
            FilteredRequests = new ObservableCollection<ServiceRequestModel>(_currentTree.GetAllRequests());
        }

        private void UpdateRelatedRequests(ServiceRequestModel request)
        {
            var related = requestTraversal.GetRelatedRequests(
                request,
                UserSession.CurrentUser?.isStaff ?? false);
                
            RelatedRequests = new ObservableCollection<ServiceRequestModel>(
                related.Where(r => r.RequestID != request.RequestID));
        }

        private void FindRelatedRequests(ServiceRequestModel request)
        {
            if (request == null) return;

            var related = FilteredRequests
                .Where(r => r.RequestID != request.RequestID)
                .Where(r => 
                    r.Category == request.Category || // Same category
                    r.CreatedBy == request.CreatedBy || // Same user
                    Math.Abs((r.RequestDate - request.RequestDate).TotalDays) <= 2 || // Within 2 days
                    r.Status == request.Status) // Same status
                .OrderByDescending(r => CalculateRelevanceScore(r, request))
                .Take(5); // Show top 5 most relevant

            RelatedRequests = new ObservableCollection<ServiceRequestModel>(related);
        }

        private double CalculateRelevanceScore(ServiceRequestModel candidate, ServiceRequestModel reference)
        {
            double score = 0;

            // Category match (highest weight)
            if (candidate.Category == reference.Category)
                score += 4;

            // Status match
            if (candidate.Status == reference.Status)
                score += 2;

            // Created by same user
            if (candidate.CreatedBy == reference.CreatedBy)
                score += 3;

            // Time proximity (0-2 points based on how close the dates are)
            var daysDifference = Math.Abs((candidate.RequestDate - reference.RequestDate).TotalDays);
            if (daysDifference <= 2)
                score += 2 * (1 - daysDifference/2);

            return score;
        }

        public void ShowRequestDetails(ServiceRequestModel request)
        {
            if (request == null) return;
            RecentRequest = request;
            FindRelatedRequests(request);
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnTitleTextChanged(string newTitle)
        {
            Title = newTitle;
            IsTitleFilled = !string.IsNullOrWhiteSpace(newTitle);
            ValidateForm();
        }

        public void InitializeCategories()
        {
            Categories = new ObservableCollection<string>
            {
                "Water & Sanitation",
                "Electricity",
                "Roads & Transport",
                "Parks",
                "Waste Management",
                "Public Safety",
                "Housing",
                "Other"
            };
        }
    }
}
