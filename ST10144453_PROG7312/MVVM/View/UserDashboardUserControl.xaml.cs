using ST10144453_PROG7312.MVVM.Model;
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

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserDashboardView.xaml
    /// </summary>
    public partial class UserDashboardUserControl : UserControl
    {
        private UserModel _currentUser;

        private AllReportsUserControl _allReportsUserControl;
        public UserDashboardUserControl(UserModel user)
        {
            InitializeComponent();
            _currentUser = user;
            _allReportsUserControl = new AllReportsUserControl(_currentUser);
        }

        private void NavigateAllReports(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = new AllReportsUserControl(_currentUser);
        }

        private void NavigateHome_Click(object sender, RoutedEventArgs e)
        {
            // Assuming MainContentControl is the container in the parent window or control
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                var mainContentControl = parentWindow.FindName("MainContentControl") as ContentControl;
                if (mainContentControl != null)
                {
                    mainContentControl.Content = new HomeView(_currentUser);
                }
            }
        }

        private void NavigateToEvents(object sender, RoutedEventArgs e)
        {

        }

        private void NavigateServiceRequests(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = new UserServiceRequestsDisplay(_currentUser);
        }
    }
}
