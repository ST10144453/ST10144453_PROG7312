//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    //============== Class: HomeViewModel ==============//
    /// <summary>
    /// This is the view model for the Home view.
    /// </summary>
    public class HomeViewModel : INotifyPropertyChanged
    {
        //++++++++++++++ Events: PropertyChanged ++++++++++++++//
        /// <summary>
        /// This event is raised when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        //++++++++++++++ Properties ++++++++++++++//
        /// <summary>
        /// This property holds the command to show the under development popup.
        /// </summary>
        public ICommand ShowUnderDevelopmentPopupCommand { get; private set; }

        /// <summary>
        /// This property holds the command to navigate to the report view.
        /// </summary>
        public ICommand NavigateReportCommand { get; private set; }

        private UserModel _user;

        public UserModel User { get; set; }

        public string Username => User?.userName;

        public HomeViewModel()
        {
            User = UserSession.CurrentUser; // Get the current user
            NavigateReportCommand = new RelayCommand(NavigateReport);
            ShowUnderDevelopmentPopupCommand = new RelayCommand(ShowUnderDevelopmentPopup);
        }


        //++++++++++++++ Methods: ShowUnderDevelopmentPopup ++++++++++++++//
        /// <summary>
        /// This method handles the ShowUnderDevelopmentPopup command.
        /// </summary>
        private void ShowUnderDevelopmentPopup()
        {
            var popup = new UnderDevelopmentPopup
            {
                Owner = Application.Current.MainWindow
            };
            popup.ShowDialog();
        }

        //++++++++++++++ Methods: OpenReport ++++++++++++++//
        /// <summary>
        /// This method handles the NavigateReport command.
        /// </summary>

        private void NavigateReport()
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow != null)
            {
                var reportSectionUserControl = new ReportSectionUserControl(User);
                mainWindow.MainContentControl.Content = reportSectionUserControl;
            }
        }



        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//