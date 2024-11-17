//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System.Collections.ObjectModel;

namespace ST10144453_PROG7312.MVVM.Model
{
    //============== Class: ReportManager ==============//
    /// <summary>
    /// This class creates the singleton instance of the ReportManager class.
    /// </summary>
    public class ReportManager
    {
        //++++++++++++++ Declarations ++++++++++++++//
        /// <summary>
        /// This instance holds the singleton instance of the ReportManager class.
        /// </summary>
        private static ReportManager _instance;

        /// <summary>
        /// This object is used to lock the singleton instance of the ReportManager class.
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// This collection holds the reports.
        /// </summary>
        private ObservableCollection<ReportModel> _reports;

        //~~~~~~~~~~~~~ Methods: Default Constructor ~~~~~~~~~~~~~//
        /// <summary>
        /// This constructor initializes the ReportManager class.
        /// </summary>
        private ReportManager()
        {
            _reports = new ObservableCollection<ReportModel>();
        }

        //~~~~~~~~~~~~~ Properties: Instance ~~~~~~~~~~~~~//
        /// <summary>
        /// This property gets the singleton instance of the ReportManager class.
        /// </summary>
        public static ReportManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ReportManager();
                }
                return _instance;
            }
        }

        //++++++++++++++ Methods: AddReport ++++++++++++++//
        /// <summary>
        /// This method adds a report to the collection of reports.
        /// </summary>
        /// <param name="report"></param>
        public void AddReport(ReportModel report)
        {
            if (_reports == null)
            {
                _reports = new ObservableCollection<ReportModel>();
            }
            _reports.Add(report);
        }

        //++++++++++++++ Properties: Reports ++++++++++++++//
        /// <summary>
        /// This property gets the collection of reports.
        /// </summary>
        public ObservableCollection<ReportModel> Reports
        {
            get => _reports;
            set
            {
                _reports = value;
            }
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//