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
using System.Windows.Shapes;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for ServiceRequestSubmissionPopup.xaml
    /// </summary>
    public partial class ServiceRequestSubmissionPopup : Window
    {
        private readonly UserModel _user;

        public ServiceRequestSubmissionPopup(ServiceRequestModel request, UserModel user)
        {
            InitializeComponent();
            _user = user;
            DataContext = new ServiceRequestViewModel(request, this, _user);
        }
    }

}
