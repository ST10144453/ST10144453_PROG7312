using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class UserDashboardHomeViewModel : INotifyPropertyChanged
    {
        private readonly UserModel _currentUser;
        private ObservableCollection<ReportModel> _recentReports;
        private ObservableCollection<ServiceRequestModel> _recentRequests;
        private ObservableCollection<EventModel> _recommendedEvents;
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand ChangePasswordCommand { get; private set; }

        public ObservableCollection<ReportModel> Reports { get; private set; }

        public UserDashboardHomeViewModel(UserModel user)
        {
            _currentUser = user;
            Reports = ReportManager.Instance.Reports ?? new ObservableCollection<ReportModel>();
            LoadUserData();
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        public string Username
        {
            get => _currentUser.userName;
            set
            {
                _currentUser.userName = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Email
        {
            get => _currentUser.email;
            set
            {
                _currentUser.email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string UserProfilePhoto => _currentUser.profilePhoto;

        public ObservableCollection<ReportModel> RecentReports
        {
            get => _recentReports;
            set
            {
                _recentReports = value;
                OnPropertyChanged(nameof(RecentReports));
            }
        }

        public ObservableCollection<ServiceRequestModel> RecentRequests
        {
            get => _recentRequests;
            set
            {
                _recentRequests = value;
                OnPropertyChanged(nameof(RecentRequests));
            }
        }

        public ObservableCollection<EventModel> RecommendedEvents
        {
            get => _recommendedEvents;
            set
            {
                _recommendedEvents = value;
                OnPropertyChanged(nameof(RecommendedEvents));
            }
        }

        public bool HasReports => RecentReports?.Any() ?? false;
        public bool HasRequests => RecentRequests?.Any() ?? false;
        public bool HasEvents => RecommendedEvents?.Any() ?? false;

        private void LoadUserData()
        {
            try
            {
                var userReports = Reports
                    .Where(r => r.CreatedBy == _currentUser.userName)
                    .OrderByDescending(r => r.reportDate)
                    .Take(4)
                    .ToList();

                RecentReports = new ObservableCollection<ReportModel>(userReports);

                var requests = ServiceRequestManager.Instance.GetAllRequests()
                    .Where(r => r.CreatedBy == _currentUser.userName)
                    .OrderByDescending(r => r.RequestDate)
                    .Take(4)
                    .Select((request, index) =>
                    {
                        request.Title = $"Service Request #{index + 1}";
                        return request;
                    })
                    .ToList();
                RecentRequests = new ObservableCollection<ServiceRequestModel>(requests);

                var eventsViewModel = new EventsViewModel();
                var events = eventsViewModel.RecommendedEvents
                    .Where(e => e.eventDate >= DateTime.Now)
                    .OrderBy(e => e.eventDate)
                    .Take(4);
                RecommendedEvents = new ObservableCollection<EventModel>(events);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading user data: {ex.Message}");
                RecentReports = new ObservableCollection<ReportModel>();
                RecentRequests = new ObservableCollection<ServiceRequestModel>();
                RecommendedEvents = new ObservableCollection<EventModel>();
            }
        }

        private void SaveChanges()
        {
            try
            {
                UserSession.CurrentUser = _currentUser;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving changes: {ex.Message}");
            }
        }

       
        public void UpdateProfilePhoto(string base64Photo)
        {
            _currentUser.profilePhoto = base64Photo;
            OnPropertyChanged(nameof(UserProfilePhoto));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
