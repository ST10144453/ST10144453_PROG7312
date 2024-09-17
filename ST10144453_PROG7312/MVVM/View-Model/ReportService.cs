using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class ReportService
    {
        private static ReportService _instance;
        public static ReportService Instance => _instance ?? (_instance = new ReportService());

        public ObservableCollection<ReportModel> Reports { get; private set; }

        public event EventHandler ReportsChanged;

        private ReportService()
        {
            Reports = new ObservableCollection<ReportModel>();
        }

        public void AddReport(ReportModel report)
        {
            Reports.Add(report);
            ReportsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
