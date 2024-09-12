using ST10144453_PROG7312.MVVM.View;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ST10144453_PROG7312.Core;

namespace ST10144453_PROG7312.MVVM.View_Model
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ShowUnderDevelopmentPopupCommand { get; private set; }
        public ICommand NavigateReportCommand { get; private set; }


        public HomeViewModel()
        {
            // Initialize the command in the constructor
            ShowUnderDevelopmentPopupCommand = new RelayCommand(ShowUnderDevelopmentPopup);

            NavigateReportCommand = new RelayCommand(OpenReport);

        }

        private void ShowUnderDevelopmentPopup()
        {
            var popup = new UnderDevelopmentPopup
            {
                Owner = Application.Current.MainWindow
            };
            popup.ShowDialog();
        }

        private void OpenReport()
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


    }
}
