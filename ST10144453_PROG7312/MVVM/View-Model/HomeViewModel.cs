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

        public UserModel User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public string Username => User?.userName;

        public HomeViewModel(UserModel user)
        {
            User = user;
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
            // Get the main window (Home or Main Menu window)
            var mainWindow = Application.Current.MainWindow;

            // Create the new ReportView window
            var reportWindow = new ReportView
            {
                Owner = mainWindow
            };

            // Hide the main window before showing the new window
            mainWindow.Visibility = Visibility.Collapsed;

            // Show the report window
            reportWindow.Closed += (s, e) =>
            {
                // Re-show the main window when the report window closes
                mainWindow.Visibility = Visibility.Visible;
            };
            reportWindow.ShowDialog(); // Or use Show() if you don't want modal behavior
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//