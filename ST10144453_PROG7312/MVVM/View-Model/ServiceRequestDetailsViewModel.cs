using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ST10144453_PROG7312.MVVM.View;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ServiceRequestDetailsViewModel : INotifyPropertyChanged
    {
        private ServiceRequestModel _request;
        private UserModel _currentUser;
        private ObservableCollection<ServiceRequestModel> _relatedRequests;
        private ICommand showRelatedRequestCommand;

        public ObservableCollection<ServiceRequestModel> RelatedRequests
        {
            get => _relatedRequests;
            set
            {
                _relatedRequests = value;
                OnPropertyChanged(nameof(RelatedRequests));
            }
        }

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

        public ICommand ShowRelatedRequestCommand
        {
            get
            {
                if (showRelatedRequestCommand == null)
                {
                    showRelatedRequestCommand = new RelayCommand<ServiceRequestModel>(ShowRelatedRequest);
                }
                return showRelatedRequestCommand;
            }
        }

        public ServiceRequestDetailsViewModel(ServiceRequestModel request, UserModel currentUser)
        {
            Request = request ?? new ServiceRequestModel();
            if (Request.AttachedFiles == null)
                Request.AttachedFiles = new List<AttachedFile>();
            if (Request.LinkedReports == null)
                Request.LinkedReports = new List<ReportModel>();
            _currentUser = currentUser;
            
            FindRelatedRequests(request);
        }

        private void FindRelatedRequests(ServiceRequestModel request)
        {
            if (request == null) return;

            var allRequests = ServiceRequestManager.Instance.GetAllRequests();
            var related = allRequests
                .Where(r => r.RequestID != request.RequestID)
                .Where(r => 
                    r.Category == request.Category || 
                    r.CreatedBy == request.CreatedBy || 
                    Math.Abs((r.RequestDate - request.RequestDate).TotalDays) <= 2 ||
                    r.Status == request.Status)
                .OrderByDescending(r => CalculateRelevanceScore(r, request))
                .Take(5);

            RelatedRequests = new ObservableCollection<ServiceRequestModel>(related);
        }

        private double CalculateRelevanceScore(ServiceRequestModel candidate, ServiceRequestModel reference)
        {
            double score = 0;
            if (candidate.Category == reference.Category) score += 4;
            if (candidate.Status == reference.Status) score += 2;
            if (candidate.CreatedBy == reference.CreatedBy) score += 3;
            
            var daysDifference = Math.Abs((candidate.RequestDate - reference.RequestDate).TotalDays);
            if (daysDifference <= 2)
                score += 2 * (1 - daysDifference/2);
            
            return score;
        }

        public void UpdateStatus(string newStatus)
        {
            if (_request != null && !string.IsNullOrEmpty(newStatus))
            {
                _request.Status = newStatus;
                ServiceRequestManager.Instance.UpdateRequest(_request);
            }
        }

        private void ShowRelatedRequest(ServiceRequestModel request)
        {
            if (request == null) return;
            var detailsPopup = new ServiceRequestDetailsPopup(request);
            detailsPopup.Owner = Application.Current.MainWindow;
            detailsPopup.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
