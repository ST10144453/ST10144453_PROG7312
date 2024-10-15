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
        private CreateEventUserControl _createEventUserControl;

        public StaffMenu(UserModel user)
        {
            InitializeComponent();
            DataContext = new StaffMenuViewModel(user);

            _staffEventsDisplayUserControl = new StaffEventsDisplayUserControl();
            _createEventUserControl = new CreateEventUserControl();

            _staffEventsDisplayUserControl.CreateEventRequested += StaffEventsDisplayUserControl_CreateEventRequested;
            _createEventUserControl.EventCreated += CreateEventUserControl_EventCreated;

            ContentArea.Content = _staffEventsDisplayUserControl;
        }

        private void StaffEventsDisplayUserControl_CreateEventRequested(object sender, EventArgs e)
        {
            ContentArea.Content = _createEventUserControl;
        }

        private void CreateEventUserControl_EventCreated(object sender, EventArgs e)
        {
            ContentArea.Content = _staffEventsDisplayUserControl;
        }

        private void ShowReports_Click(object sender, RoutedEventArgs e)
        {
            // Handle showing reports
        }

        private void ManageEvents_Click(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = _staffEventsDisplayUserControl;
        }
    }

}
