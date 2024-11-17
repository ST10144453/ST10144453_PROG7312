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
using ST10144453_PROG7312.Core;
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using Microsoft.Win32;
using System.IO;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserDashboardHomeControl.xaml
    /// </summary>
    public partial class UserDashboardHomeControl : UserControl
    {
        private readonly UserModel _currentUser;
        
        public UserDashboardHomeControl(UserModel user)
        {
            InitializeComponent();
            _currentUser = user;
            DataContext = new UserDashboardHomeViewModel(user);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReportCard_Click(object sender, RoutedEventArgs e)
        {

        }
       
    }
}
