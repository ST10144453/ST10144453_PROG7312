using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ST10144453_PROG7312.Core;
using System.Windows;
using ST10144453_PROG7312.MVVM.View;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class TreeSelectionViewModel : INotifyPropertyChanged
    {
        private IServiceRequestTree _currentTree;
        private TreeType _selectedTreeType;
        private SortingStrategy _selectedSortStrategy;
        private string _treeDescription;
        private ObservableCollection<ServiceRequestModel> _displayedRequests;
        public RelayCommand UpdateCommand { get; private set; }
        public ObservableCollection<TreeType> AvailableTreeTypes { get; }
        public ObservableCollection<SortingStrategy> AvailableSortStrategies { get; }

        public TreeSelectionViewModel()
        {
            _displayedRequests = new ObservableCollection<ServiceRequestModel>();
            AvailableTreeTypes = new ObservableCollection<TreeType>
            {
                TreeType.Basic,
                TreeType.BinarySearch,
                TreeType.AVL,
                TreeType.RedBlack
            };
            AvailableSortStrategies = new ObservableCollection<SortingStrategy>
            {
                SortingStrategy.ByDate,
                SortingStrategy.ByCategory,
                SortingStrategy.ByStatus,
                SortingStrategy.ByPriority
            };
            _selectedTreeType = TreeType.Basic;
            _selectedSortStrategy = SortingStrategy.ByDate;
            UpdateCommand = new RelayCommand(ExecuteUpdate);
            UpdateTreeImplementation();
        }

        public TreeType SelectedTreeType
        {
            get => _selectedTreeType;
            set
            {
                _selectedTreeType = value;
                OnPropertyChanged(nameof(SelectedTreeType));
                UpdateTreeDescription();
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
            private set
            {
                _treeDescription = value;
                OnPropertyChanged(nameof(TreeDescription));
            }
        }

        public ObservableCollection<ServiceRequestModel> DisplayedRequests
        {
            get => _displayedRequests;
            private set
            {
                _displayedRequests = value;
                OnPropertyChanged(nameof(DisplayedRequests));
            }
        }

        private void UpdateTreeImplementation()
        {
            // Store existing requests
            var existingRequests = ServiceRequestManager.Instance.GetAllRequests();

            // Create new tree
            _currentTree = ServiceRequestTreeFactory.CreateTree(SelectedTreeType);
            
            // Set sorting strategy
            _currentTree.SetSortingStrategy(SelectedSortStrategy);
            
            // Populate tree with all requests
            foreach (var request in existingRequests)
            {
                _currentTree.Insert(request);
            }

            UpdateTreeDescription();
        }

        private void UpdateTreeDescription()
        {
            switch (SelectedTreeType)
            {
                case TreeType.Basic:
                    TreeDescription = "Basic Tree: A simple hierarchical structure where each node can have multiple children. Suitable for representing organizational hierarchies.";
                    break;
                case TreeType.BinarySearch:
                    TreeDescription = "Binary Search Tree: A binary tree where left child is smaller and right child is larger than parent. Efficient for sorted data.";
                    break;
                case TreeType.AVL:
                    TreeDescription = "AVL Tree: Self-balancing binary search tree where heights of left and right subtrees differ by at most one. Provides O(log n) operations.";
                    break;
                case TreeType.RedBlack:
                    TreeDescription = "Red-Black Tree: Self-balancing binary search tree with color properties ensuring balance. Efficient for frequent insertions and deletions.";
                    break;
                default:
                    TreeDescription = string.Empty;
                    break;
            }
        }

        private void RefreshDisplayedRequests()
        {
            if (_currentTree == null) return;

            var requests = _currentTree.GetAllRequests();
            IEnumerable<ServiceRequestModel> sortedRequests;

            switch (_selectedSortStrategy)
            {
                case SortingStrategy.ByDate:
                    sortedRequests = requests.OrderByDescending(r => r.RequestDate);
                    break;
                case SortingStrategy.ByCategory:
                    sortedRequests = requests.OrderBy(r => r.Category);
                    break;
                case SortingStrategy.ByStatus:
                    sortedRequests = requests.OrderBy(r => r.Status);
                    break;
                case SortingStrategy.ByPriority:
                    sortedRequests = requests.OrderByDescending(r => GetPriorityScore(r));
                    break;
                default:
                    sortedRequests = requests;
                    break;
            }

            DisplayedRequests = new ObservableCollection<ServiceRequestModel>(sortedRequests);
        }

        private int GetPriorityScore(ServiceRequestModel request)
        {
            int score = 0;
            var age = (DateTime.Now - request.RequestDate).Days;
            
            if (age > 7) score += 3;
            else if (age > 3) score += 2;
            else score += 1;

            if (request.Status == "Pending") score += 2;
            
            return score;
        }

        private void ExecuteUpdate()
        {
            UpdateTreeImplementation();
            RefreshDisplayedRequests();
            UpdateGraphView();
        }

        private void UpdateGraphView()
        {
            if (Application.Current.MainWindow.FindName("GraphControl") is ServiceRequestGraphControl graphControl)
            {
                graphControl.Requests = DisplayedRequests;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
