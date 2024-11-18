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
        private readonly ServiceRequestViewModel _serviceRequestViewModel;
        private TreeType _selectedTreeType;
        private SortingStrategy _selectedSortStrategy;
        private string _treeDescription;
        private ObservableCollection<ServiceRequestModel> _displayedRequests;
        public TreeSelectionViewModel TreeSelectionViewModel { get; }

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

        public TreeType SelectedTreeType
        {
            get => _selectedTreeType;
            set
            {
                _selectedTreeType = value;
                OnPropertyChanged(nameof(SelectedTreeType));
            }
        }

        public SortingStrategy SelectedSortStrategy
        {
            get => _selectedSortStrategy;
            set
            {
                _selectedSortStrategy = value;
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

        public ObservableCollection<ServiceRequestModel> DisplayedRequests
        {
            get => _displayedRequests;
            set
            {
                _displayedRequests = value;
                OnPropertyChanged(nameof(DisplayedRequests));
            }
        }

        public UserServiceRequestsViewModel(UserModel currentUser)
        {
            _currentUser = currentUser;
            IsStaff = currentUser.isStaff;
            _serviceRequestViewModel = new ServiceRequestViewModel(new ServiceRequestModel(), null, currentUser);

            if (_isStaff)
            {
                _serviceRequestViewModel.SelectedTreeType = TreeType.AVL;
                _serviceRequestViewModel.SelectedSortStrategy = SortingStrategy.ByDate;
            }

            NewRequestCommand = new RelayCommand(CreateNewRequest);
            UserServiceRequests = new ObservableCollection<ServiceRequestModel>();
            TreeSelectionViewModel = new TreeSelectionViewModel();
            if (_isStaff)
            {
                TreeSelectionViewModel.SelectedTreeType = TreeType.AVL;
                TreeSelectionViewModel.SelectedSortStrategy = SortingStrategy.ByDate;
            }

            // Add dummy data
            AddDummyData();
            LoadServiceRequests();
        }

        private void AddDummyData()
        {
            var dummyRequests = new List<ServiceRequestModel>
            {
                new ServiceRequestModel { Title="Main Road Water Leak",Category = "Water & Sanitation", Description = "Major water leak on Main Road", Status = "Pending", RequestDate = DateTime.Now.AddDays(-1), CreatedBy = "john.doe" },
                new ServiceRequestModel { Title="Power Outage!!!", Category = "Electricity", Description = "Power outage in Central District", Status = "In Progress", RequestDate = DateTime.Now.AddDays(-5), CreatedBy = "jane.smith" },
                new ServiceRequestModel { Title="I almost slid off the road!",Category = "Roads", Description = "Pothole on Oak Avenue", Status = "Completed", RequestDate = DateTime.Now.AddDays(-10), CreatedBy = "bob.wilson" },
                new ServiceRequestModel { Title="Pine street is flooding.",Category = "Water & Sanitation", Description = "Blocked drain on Pine Street", Status = "Pending", RequestDate = DateTime.Now.AddDays(-2), CreatedBy = "sarah.jones" },
                new ServiceRequestModel { Title="I can't take my nigth walks.",Category = "Electricity", Description = "Street light malfunction", Status = "In Progress", RequestDate = DateTime.Now.AddDays(-3), CreatedBy = "mike.brown" },
                new ServiceRequestModel { Title="The park is turning into a jungle!",Category = "Parks", Description = "Overgrown vegetation in Community Park", Status = "Pending", RequestDate = DateTime.Now.AddDays(-7), CreatedBy = "lisa.green" },
                new ServiceRequestModel { Title="I waited 40 min to go!",Category = "Roads", Description = "Traffic light not working", Status = "In Progress", RequestDate = DateTime.Now.AddDays(-4), CreatedBy = "tom.white" },
                new ServiceRequestModel { Title="It's starting to get ral smelly...",Category = "Water & Sanitation", Description = "Sewage overflow", Status = "Completed", RequestDate = DateTime.Now.AddDays(-15), CreatedBy = "emma.black" },
                new ServiceRequestModel { Title="MY CHILD ALMOST DIED!!!!", Category = "Electricity", Description = "Exposed electrical wires", Status = "Pending", RequestDate = DateTime.Now.AddDays(-6), CreatedBy = "david.gray" },
                new ServiceRequestModel { Title="Timmy broke his arm.",Category = "Parks", Description = "Broken playground equipment", Status = "In Progress", RequestDate = DateTime.Now.AddDays(-8), CreatedBy = "amy.taylor" }
            };

            foreach (var request in dummyRequests)
            {
                ServiceRequestManager.Instance.AddRequest(request);
            }
        }

        private void LoadServiceRequests()
        {
            var allRequests = IsStaff 
                ? ServiceRequestManager.Instance.GetPrioritizedRequests()
                : ServiceRequestManager.Instance.ServiceRequests.Where(r => r.CreatedBy == _currentUser.userName);
            
            UserServiceRequests = new ObservableCollection<ServiceRequestModel>(allRequests);
        }

        private void CreateNewRequest()
        {
            var newRequest = new ServiceRequestModel
            {
                CreatedBy = _currentUser.userName,
                Status = "Pending"
            };

            var serviceRequestView = new ServiceRequestUserControl(newRequest, _currentUser);
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
