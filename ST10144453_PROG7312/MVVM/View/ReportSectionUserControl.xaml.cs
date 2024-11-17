using ST10144453_PROG7312.MVVM.View_Model;
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

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReportSectionUserControl.xaml
    /// </summary>
    public partial class ReportSectionUserControl : UserControl
    {
        /// <summary>
        /// This constant holds the duration of the animation.
        /// </summary>
        private ReportUserControl _reportIssueView;


        private AllReportsUserControl _allReportsUserControl;

        private UserModel _currentUser;
        public ReportSectionUserControl(UserModel user)
        {
            _currentUser = user;

            InitializeComponent();

            _reportIssueView = new ReportUserControl();
            _allReportsUserControl = new AllReportsUserControl(_currentUser);

            MainContentControl.Content = _reportIssueView;

            DataContext = new ReportViewModel(_currentUser);
        }

        //++++++++++++++ Methods: NavigateToReportIssue_Click ++++++++++++++//
        /// <summary>
        /// This method navigates to the ReportIssue view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToReportIssue_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = _reportIssueView;
        }


        private void NavigateToAllReports_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = _allReportsUserControl;
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
    }
}
