using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10144453_PROG7312.MVVM.Model
{
    public class ReportManager
    {
        private static ReportManager _instance;
        private static readonly object _lock = new object();

        public ObservableCollection<ReportModel> Reports { get; private set; }

        private ReportManager()
        {
            Reports = new ObservableCollection<ReportModel>();
            // Add any initial dummy data here if needed.
            Reports.Add(new ReportModel
            {
                reportName = "Issue 1",
                reportLocation = "Location 1",
                reportDescription = "Description 1",
                reportCategory = "Category 1",
            });

            Reports.Add(new ReportModel
            {
                reportName = "Issue 2",
                reportLocation = "Location 2",
                reportDescription = "Description 2",
                reportCategory = "Category 2",
            });
        }

        public static ReportManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new ReportManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public void AddReport(ReportModel report)
        {
            Reports.Add(report);
        }
    }
}
