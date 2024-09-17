using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ForumViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ReportModel> Reports => ReportService.Instance.Reports;

        public ForumViewModel()
        {
            ReportService.Instance.ReportsChanged += (sender, args) => OnPropertyChanged(nameof(Reports));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // INotifyPropertyChanged implementation
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
