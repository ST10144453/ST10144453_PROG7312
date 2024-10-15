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
using ST10144453_PROG7312.MVVM.View_Model;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for StaffEventsDisplayUserControl.xaml
    /// </summary>
    public partial class StaffEventsDisplayUserControl : UserControl
    {
        public event EventHandler CreateEventRequested;

        public StaffEventsDisplayUserControl()
        {
            InitializeComponent();
            DataContext = new CreateEventViewModel();
        }

        private void CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            CreateEventRequested?.Invoke(this, EventArgs.Empty);
          
        }

    }
}
