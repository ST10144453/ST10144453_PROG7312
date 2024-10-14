using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ForumViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<ReportModel> _allReports; // Store all reports
        private ICollectionView _filteredReports; // Collection for filtered reports
        private ReportViewModel _reportViewModel; // Instance of ReportViewModel

        public ICollectionView FilteredReports
        {
            get => _filteredReports;
            private set
            {
                _filteredReports = value;
                OnPropertyChanged(nameof(FilteredReports));
            }
        }

        public ForumViewModel(ReportViewModel reportViewModel)
        {
            _reportViewModel = reportViewModel;
            _allReports = new ObservableCollection<ReportModel>();
            FilteredReports = CollectionViewSource.GetDefaultView(_allReports);
        }


        public void FilterReportsByCurrentUser()
        {
            if (_reportViewModel.CurrentUser == null)
            {
                FilteredReports.Filter = null; // No filter if no user
                return;
            }

            // Filter by current user's ID
            FilteredReports.Filter = report => ((ReportModel)report).CreatedBy.userID == _reportViewModel.CurrentUser.userID;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
