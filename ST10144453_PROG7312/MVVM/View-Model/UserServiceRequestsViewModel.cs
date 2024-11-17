using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.Core;
using System.Windows.Input;
using ST10144453_PROG7312.MVVM.View;
using System.Windows;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class UserServiceRequestsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ServiceRequestModel> _userServiceRequests;
        private readonly UserModel _currentUser;
        private bool _isStaff;
        private readonly ServiceRequestTree _requestTree;

        public ObservableCollection<ServiceRequestModel> UserServiceRequests
        {
            get => _userServiceRequests;
            set
            {
                _userServiceRequests = value;
                OnPropertyChanged(nameof(UserServiceRequests));
            }
        }

        public bool IsStaff
        {
            get => _isStaff;
            set
            {
                _isStaff = value;
                OnPropertyChanged(nameof(IsStaff));
            }
        }

        public ICommand NewRequestCommand { get; }

        public List<string> AvailableStatuses { get; } = new List<string>
        {
            "Pending",
            "In Progress",
            "Completed",
            "Rejected"
        };

        public UserServiceRequestsViewModel(UserModel currentUser)
        {
            _currentUser = currentUser;
            IsStaff = currentUser.isStaff;
            _requestTree = new ServiceRequestTree();
            NewRequestCommand = new RelayCommand(CreateNewRequest);
            
            // Initialize the collection
            UserServiceRequests = new ObservableCollection<ServiceRequestModel>();
            
            // Load the requests
            LoadServiceRequests();
        }

        private void LoadServiceRequests()
        {
            var allRequests = ServiceRequestManager.Instance.ServiceRequests;
            
            // If staff, show all requests, otherwise filter for current user
            var filteredRequests = IsStaff 
                ? allRequests 
                : allRequests.Where(r => r.CreatedBy == _currentUser.userName);
            
            UserServiceRequests.Clear();
            foreach (var request in filteredRequests)
            {
                UserServiceRequests.Add(request);
            }
        }

        private void CreateNewRequest()
        {
            var newRequest = new ServiceRequestModel
            {
                CreatedBy = _currentUser.userName,
                Status = "Pending"
            };
            
            var serviceRequestView = new ServiceRequestUserControl(newRequest);
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainContentControl.Content = serviceRequestView;
            }
        }

        public void UpdateRequestStatus(ServiceRequestModel request, string newStatus)
        {
            if (request != null && !string.IsNullOrEmpty(newStatus))
            {
                request.Status = newStatus;
                ServiceRequestManager.Instance.UpdateRequest(request);
                LoadServiceRequests(); // Refresh the list
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
