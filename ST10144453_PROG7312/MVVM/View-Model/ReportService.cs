//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.ObjectModel;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    //============== Class: ReportService ==============//
    /// <summary>
    /// This class holds the base implementation for the ReportService class.
    /// </summary>
    public class ReportService
    {
        //++++++++++++++ Declarations ++++++++++++++//
        /// <summary>
        /// This property holds the instance of the ReportService class.
        /// </summary>
        private static ReportService _instance;

        /// <summary>
        /// This property holds the instance of the ReportService class.
        /// </summary>
        public static ReportService Instance => _instance ?? (_instance = new ReportService());

        /// <summary>
        /// This property holds the reports.
        /// </summary>
        public ObservableCollection<ReportModel> Reports { get; private set; }

        /// <summary>
        /// This event is raised when the reports are changed.
        /// </summary>
        public event EventHandler ReportsChanged;

        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        /// <summary>
        /// This method initializes the ReportService class.
        /// </summary>
        private ReportService()
        {
            Reports = new ObservableCollection<ReportModel>();
        }

        //++++++++++++++ Methods: AddReport ++++++++++++++//
        /// <summary>
        /// This method adds a report to the reports.
        /// </summary>
        /// <param name="report"></param>
        public void AddReport(ReportModel report)
        {
            Reports.Add(report);
            ReportsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//