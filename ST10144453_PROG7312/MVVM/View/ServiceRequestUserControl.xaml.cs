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
    /// Interaction logic for ServiceRequestUserControl.xaml
    /// </summary>
    public partial class ServiceRequestUserControl : UserControl
    {
        private readonly UserModel _currentUser;

        public ServiceRequestUserControl(ServiceRequestModel request, UserModel currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            DataContext = new ServiceRequestViewModel(request, (Window)this.Parent);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Multiselect = true,
                Filter = "All Files|*.pdf;*.doc;*.docx;*.txt;*.jpg;*.jpeg;*.png|PDF Files|*.pdf|Word Documents|*.doc;*.docx|Text Files|*.txt|Image Files|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var viewModel = DataContext as ServiceRequestViewModel;
                foreach (string filename in openFileDialog.FileNames)
                {
                    viewModel?.AttachFile(filename);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var selectReportsWindow = new SelectReportsDialog
            {
                Owner = Window.GetWindow(this)
            };

            if (selectReportsWindow.ShowDialog() == true)
            {
                var viewModel = DataContext as ServiceRequestViewModel;
                foreach (var report in selectReportsWindow.SelectedReports)
                {
                    viewModel?.LinkReport(report);
                }
            }
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


    
      
