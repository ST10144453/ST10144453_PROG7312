using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ServiceRequestModel : INotifyPropertyChanged
    {
        private string _status;
        private string _firstName;
        private string _surname;
        private string _email;
        private string _phoneNumber;
        private string _additionalAddress;
        private string _preferredFeedbackMethod;

        private List<AttachedFile> _attachedFiles = new List<AttachedFile>();
        private List<ReportModel> _linkedReports = new List<ReportModel>();

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
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
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
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

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
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
            }
        }

        public string PreferredFeedbackMethod
        {
            get => _preferredFeedbackMethod;
            set
            {
                _preferredFeedbackMethod = value;
                OnPropertyChanged();
            }
        }

        public string Category { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime RequestDate { get; set; }
        public Guid RequestID { get; set; }
    public string Title { get; set; }

        public List<AttachedFile> AttachedFiles
        {
            get => _attachedFiles;
            set
            {
                _attachedFiles = value;
                OnPropertyChanged();
            }
        }

        public List<ReportModel> LinkedReports
        {
            get => _linkedReports;
            set
            {
                _linkedReports = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AttachedFile
    {
        public string FileName { get; set; }
        public string FileContent { get; set; } // Base64 encoded file content
        public string FileType { get; set; }
    }
}
