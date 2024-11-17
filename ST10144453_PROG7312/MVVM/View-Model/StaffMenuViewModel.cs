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

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class StaffMenuViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _email;
        private string _profilePhoto;
        private ObservableCollection<ReportModel> _allReports;
        private ObservableCollection<ServiceRequestModel> _serviceRequests;

        public ObservableCollection<ReportModel> AllReports
        {
            get => _allReports;
            set
            {
                _allReports = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ServiceRequestModel> ServiceRequests
        {
            get => _serviceRequests;
            set
            {
                _serviceRequests = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string ProfilePhoto
        {
            get => _profilePhoto;
            set
            {
                _profilePhoto = value;
                OnPropertyChanged();
            }
        }

        // Public parameterless constructor
        public StaffMenuViewModel()
        {
        }

        public StaffMenuViewModel(UserModel user)
        {
            Username = user.userName;
            Email = user.email;
            ProfilePhoto = user.profilePhoto;

            // Load all reports and service requests
            LoadAllReports();
            LoadAllServiceRequests();
        }

        private void LoadAllReports()
        {
            AllReports = new ObservableCollection<ReportModel>(ReportManager.Instance.Reports);
        }

        private void LoadAllServiceRequests()
        {
            ServiceRequests = new ObservableCollection<ServiceRequestModel>(ServiceRequestManager.Instance.GetAllRequests());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
