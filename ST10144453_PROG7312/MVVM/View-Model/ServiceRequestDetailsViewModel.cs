using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ServiceRequestDetailsViewModel : INotifyPropertyChanged
    {
        private  ServiceRequestModel _request;
        private  UserModel _currentUser;

        public ServiceRequestModel Request
        {
            get => _request;
            private set => _request = value;
        }
        public bool IsStaff => _currentUser.isStaff;

        public List<string> AvailableStatuses { get; } = new List<string>
            {
                "Pending",
                "In Progress",
                "Completed",
                "Rejected"
            };

        public ServiceRequestDetailsViewModel(ServiceRequestModel request, UserModel currentUser)
        {
            Request = request ?? new ServiceRequestModel();
            if (Request.AttachedFiles == null)
                Request.AttachedFiles = new List<AttachedFile>();
            if (Request.LinkedReports == null)
                Request.LinkedReports = new List<ReportModel>();
            _currentUser = currentUser;
        }

        public void UpdateStatus(string newStatus)
        {
            if (_request != null && !string.IsNullOrEmpty(newStatus))
            {
                _request.Status = newStatus;
                ServiceRequestManager.Instance.UpdateRequest(_request);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
