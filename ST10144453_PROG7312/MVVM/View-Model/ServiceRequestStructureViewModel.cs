using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View.Visualizations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ServiceRequestStructureViewModel : INotifyPropertyChanged
    {
        private IServiceRequestStructure currentStructure;
        private ObservableCollection<ServiceRequestModel> displayedRequests;
        private ServiceRequestStructureType selectedStructureType;
        private BaseVisualizationControl currentVisualization;

        public ServiceRequestStructureViewModel()
        {
            AvailableStructures = new ObservableCollection<ServiceRequestStructureType>
            {
                ServiceRequestStructureType.BasicTree,
                ServiceRequestStructureType.AVLTree,
                ServiceRequestStructureType.MinHeap,
                ServiceRequestStructureType.Graph,
                ServiceRequestStructureType.MST
            };
            displayedRequests = new ObservableCollection<ServiceRequestModel>();
            selectedStructureType = ServiceRequestStructureType.BasicTree;
            UpdateStructureImplementation();
        }

        public BaseVisualizationControl CurrentVisualization
        {
            get => currentVisualization;
            set
            {
                currentVisualization = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ServiceRequestStructureType> AvailableStructures { get; }
        public ObservableCollection<ServiceRequestModel> DisplayedRequests
        {
            get => displayedRequests;
            set
            {
                displayedRequests = value;
                OnPropertyChanged();
            }
        }

        public ServiceRequestStructureType SelectedStructureType
        {
            get => selectedStructureType;
            set
            {
                selectedStructureType = value;
                UpdateStructureImplementation();
                OnPropertyChanged();
            }
        }

        public string CurrentStructureDescription => currentStructure?.GetStructureDescription() ?? string.Empty;

        private void UpdateStructureImplementation()
        {
            var requests = currentStructure?.GetAllRequests().ToList() ?? new List<ServiceRequestModel>();

            var treeViz = new TreeVisualizationControl();
            treeViz.UpdateVisualization(requests);
            CurrentVisualization = treeViz;

            DisplayedRequests = new ObservableCollection<ServiceRequestModel>(requests);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
