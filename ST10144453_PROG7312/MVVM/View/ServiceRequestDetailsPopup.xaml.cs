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
using System.Windows.Shapes;
using ST10144453_PROG7312.MVVM.View_Model;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for ServiceRequestDetailsPopup.xaml
    /// </summary>
    public partial class ServiceRequestDetailsPopup : Window
    {
        private readonly ServiceRequestDetailsViewModel _viewModel;

        public ServiceRequestDetailsPopup(ServiceRequestModel request)
        {
            InitializeComponent();
            _viewModel = new ServiceRequestDetailsViewModel(request, UserSession.CurrentUser);
            DataContext = _viewModel;
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                _viewModel.UpdateStatus(comboBox.SelectedItem as string);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
