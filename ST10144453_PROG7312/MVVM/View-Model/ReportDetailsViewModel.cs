using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ST10144453_PROG7312.MVVM.Model;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ReportDetailsViewModel : INotifyPropertyChanged
    {
        private ReportModel _report;

        public ReportModel Report
        {
            get => _report;
            set
            {
                _report = value;
                OnPropertyChanged(nameof(Report));
            }
        }

        public ReportDetailsViewModel(ReportModel report)
        {
            Report = report;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
