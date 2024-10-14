//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

namespace ST10144453_PROG7312.MVVM.View
{
    //============== Class: ReportUserControl ==============//
    /// <summary>
    /// Interaction logic for ReportUserControl.xaml
    /// </summary>
    public partial class ReportUserControl : UserControl
    {
        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        /// <summary>
        /// This method initializes the ReportUserControl class.
        /// </summary>
        public ReportUserControl()
        {
            InitializeComponent();
            UserModel currentUser = new UserModel(); // Create a new instance of UserModel
            DataContext = new ReportViewModel(); // Pass the currentUser instance to the ReportViewModel constructor
        }

        //++++++++++++++ Methods: OnIssueNameTextChanged ++++++++++++++//
        /// <summary>
        /// This method is called when the IssueName text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIssueNameTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsIssueNameFilled = !string.IsNullOrEmpty(viewModel.IssueName);
            }
        }

        //++++++++++++++ Methods: OnLocationTextChanged ++++++++++++++//
        /// <summary>
        /// This method is called when the Location text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLocationTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsLocationFilled = !string.IsNullOrEmpty(viewModel.Location);
            }
        }

        //++++++++++++++ Methods: OnCategorySelectionChanged ++++++++++++++//
        /// <summary>
        /// This method is called when the Category selection is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsCategorySelected = viewModel.SelectedCategory != null;
            }
        }

        //++++++++++++++ Methods: OnDescriptionTextChanged ++++++++++++++//
        /// <summary>
        /// This method is called when the Description text is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDescriptionTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = DataContext as ReportViewModel;
            if (viewModel != null)
            {
                viewModel.IsDescriptionFilled = !string.IsNullOrEmpty(viewModel.Description);
            }
        }

        //++++++++++++++ Methods: ProgressBar_Loaded ++++++++++++++//
        /// <summary>
        /// This method is called when the ProgressBar is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressBar_Loaded(object sender, RoutedEventArgs e)
        {
            var progressBar = sender as ProgressBar;
            if (progressBar != null)
            {
                // Bind the Width property of PART_Indicator to the ActualWidth of the ProgressBar
                Binding widthBinding = new Binding("ActualWidth")
                {
                    Source = progressBar,
                    Mode = BindingMode.OneWay
                };

                // Set up the Width binding for PART_Indicator
                var indicator = (Rectangle)progressBar.Template.FindName("PART_Indicator", progressBar);
                if (indicator != null)
                {
                    indicator.SetBinding(Rectangle.WidthProperty, widthBinding);
                }
            }
        }

        // Define the OnMediaDrop method
        private void OnMediaDrop(object sender, DragEventArgs e)
        {
            // Handle the drop event
        }

        // Define the OnMediaDragEnter method
        private void OnMediaDragEnter(object sender, DragEventArgs e)
        {
            // Handle the drag enter event
        }

        // Define the OnMediaDragLeave method
        private void OnMediaDragLeave(object sender, DragEventArgs e)
        {
            // Handle the drag leave event
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//
