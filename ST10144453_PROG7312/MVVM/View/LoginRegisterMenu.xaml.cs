using ST10144453_PROG7312.MVVM.View_Model;
using ST10144453_PROG7312.MVVM.ViewMode;
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
    /// Interaction logic for LoginRegisterMenu.xaml
    /// </summary>
    public partial class LoginRegisterMenu : UserControl
    {
        public LoginRegisterMenu()
        {
            InitializeComponent();
            DataContext = new LoginRegisterViewModel();

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this) as MainWindow;
            if (parent != null)
            {
                parent.MainContentControl.Content = new LoginUserControl();
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this) as MainWindow;
            if (parent != null)
            {
                parent.MainContentControl.Content = new RegisterUserControl();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
