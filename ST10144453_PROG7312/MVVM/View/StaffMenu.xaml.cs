using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for StaffMenu.xaml
    /// </summary>
    public partial class StaffMenu : UserControl
    {
        private StaffEventsDisplayUserControl _staffEventsDisplayUserControl;
        private AllReportsUserControl _allReportsUserControl;
        private UserServiceRequestsDisplay _serviceRequestsDisplay;
        private CreateEventUserControl _createEventUserControl;

        public StaffMenu(UserModel user)
        {
            InitializeComponent();
            DataContext = new StaffMenuViewModel(user);

            // Initialize all user controls
            _staffEventsDisplayUserControl = new StaffEventsDisplayUserControl();
            _allReportsUserControl = new AllReportsUserControl(null);
            _serviceRequestsDisplay = new UserServiceRequestsDisplay(user);
            _createEventUserControl = new CreateEventUserControl();

            // Wire up events
            _staffEventsDisplayUserControl.CreateEventRequested += StaffEventsDisplayUserControl_CreateEventRequested;
            _createEventUserControl.EventCreated += CreateEventUserControl_EventCreated;

            // Set initial content
            ContentArea.Content = _allReportsUserControl;
        }

        private void ShowReports_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _allReportsUserControl;
        }

        private void ManageEvents_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _staffEventsDisplayUserControl;
        }

        private void ShowServiceRequests_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _serviceRequestsDisplay;
        }

        private void StaffEventsDisplayUserControl_CreateEventRequested(object sender, EventArgs e)
        {
            ContentArea.Content = _createEventUserControl;
        }

        private void CreateEventUserControl_EventCreated(object sender, EventArgs e)
        {
            ContentArea.Content = _staffEventsDisplayUserControl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Clear the current user session
            UserSession.CurrentUser = null;

            // Get the main window
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to the login/register menu using the MainContentControl
                var loginRegisterMenu = new LoginRegisterMenu();
                mainWindow.MainContentControl.Content = loginRegisterMenu;
            }
        }

        private void ShowDataStructures_Click(object sender, RoutedEventArgs e)
{
    ContentArea.Content = new ServiceRequestStructureView();
}
    }

}
