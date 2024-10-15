using ST10144453_PROG7312.MVVM.Model;
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

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for EventsViewUserControl.xaml
    /// </summary>
    public partial class EventsViewUserControl : UserControl
    {

        private UserModel _currentUser;

       
        public EventsViewUserControl(UserModel user)
        {
            _currentUser = user;

            InitializeComponent();


            DataContext = new EventsViewModel(); // Set the DataContext
        }

        private void NavigateToReportIssue_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavigateToAllReports_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavigateHome_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
