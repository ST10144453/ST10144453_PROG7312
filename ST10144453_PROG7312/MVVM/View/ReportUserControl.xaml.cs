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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReportUserControl.xaml
    /// </summary>
    public partial class ReportUserControl : UserControl
    {
        public ReportUserControl()
        {
            InitializeComponent();
        }

        private void OnIssueNameTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsIssueNameFilled = !string.IsNullOrEmpty(viewModel.IssueName);
            }
        }

        private void OnLocationTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsLocationFilled = !string.IsNullOrEmpty(viewModel.Location);
            }
        }

        private void OnCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsCategorySelected = viewModel.SelectedCategory != null;
            }
        }

        private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsDescriptionFilled = !string.IsNullOrEmpty(viewModel.Description);
            }
        }
    }
}