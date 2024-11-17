using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class UserServiceRequestsDisplay : UserControl
    {
        public UserServiceRequestsDisplay(UserModel user)
        {
            InitializeComponent();
            DataContext = new UserServiceRequestsViewModel(user);
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is ServiceRequestModel request)
            {
                var viewModel = DataContext as UserServiceRequestsViewModel;
                viewModel?.UpdateRequestStatus(request, comboBox.SelectedItem as string);
            }
        }

        private void ServiceRequest_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.DataContext is ServiceRequestModel request)
            {
                var detailsPopup = new ServiceRequestDetailsPopup(request);
                detailsPopup.Owner = Window.GetWindow(this);
                detailsPopup.ShowDialog();
            }
        }
    }
}