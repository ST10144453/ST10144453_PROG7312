//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    //============== Class: ForumViewModel ==============//
    /// <summary>
    /// This class holds the base implementation for the ForumViewModel class.
    /// </summary>
    public class ForumViewModel : INotifyPropertyChanged
    {
        //++++++++++++++ Properties: Reports ++++++++++++++//
        /// <summary>
        /// This property holds the reports.
        /// </summary>
        public ObservableCollection<ReportModel> Reports => ReportService.Instance.Reports;

        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        public ForumViewModel()
        {
            ReportService.Instance.ReportsChanged += (sender, args) => OnPropertyChanged(nameof(Reports));
        }

        //++++++++++++++ Events: PropertyChanged ++++++++++++++//
        /// <summary>
        /// This event is raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //++++++++++++++ Methods: OnPropertyChanged ++++++++++++++//
        /// <summary>
        /// This method is called when a property is changed.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//