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

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class UserServiceRequestsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ServiceRequestModel> _userServiceRequests;
        private readonly UserModel _currentUser;
        private bool _isEmployee;
        private readonly ServiceRequestTree _requestTree;
        private readonly string _currentUsername;

        public ObservableCollection<ServiceRequestModel> UserServiceRequests
        {
            get => _userServiceRequests;
            set
            {
                _userServiceRequests = value;
                OnPropertyChanged(nameof(UserServiceRequests));
            }
        }

        public bool IsEmployee
        {
            get => _isEmployee;
            set
            {
                _isEmployee = value;
                OnPropertyChanged(nameof(IsEmployee));
            }
        }

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
            IsEmployee = currentUser.isStaff == false;
            _currentUsername = currentUser.userName;
            _requestTree = new ServiceRequestTree();
            LoadUserServiceRequests();
        }

        private void LoadUserServiceRequests()
        {
            var userRequests = _requestTree.GetRequestsByUser(_currentUsername);
            UserServiceRequests = new ObservableCollection<ServiceRequestModel>(userRequests);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
