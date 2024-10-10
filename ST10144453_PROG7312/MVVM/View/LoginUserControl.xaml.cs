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
    /// Interaction logic for LoginUserControl.xaml
    /// </summary>
    public partial class LoginUserControl : UserControl
    {
        public LoginUserControl()
        {
            InitializeComponent();
        }


        private void TogglePasswordVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (TogglePasswordVisibility.IsChecked == true)
            {
                PlainTextPasswordBox.Text = PasswordBox.Password;
                PasswordBox.Visibility = Visibility.Collapsed;
                PlainTextPasswordBox.Visibility = Visibility.Visible;
            }
            else
            {
                PasswordBox.Password = PlainTextPasswordBox.Text;
                PasswordBox.Visibility = Visibility.Visible;
                PlainTextPasswordBox.Visibility = Visibility.Collapsed;
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var parent = Window.GetWindow(this) as MainWindow;
            if (parent != null)
            {
                parent.MainContentControl.Content = new LoginRegisterMenu();
            }
        }
    }
}
