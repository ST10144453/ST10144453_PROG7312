//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Windows;
using System.Windows.Controls;

namespace ST10144453_PROG7312.MVVM.View
{
    //============== Class: HomeView ==============//
    /// <summary>
    /// This is the code-behind for the HomeView class.
    /// </summary>
    public partial class HomeView : UserControl
    {
        //++++++++++++++ Constants ++++++++++++++//
        /// <summary>
        /// This constant holds the original width of the HomeView.
        /// </summary>
        private const double OriginalWidth = 1910;

        /// <summary>
        /// This constant holds the original height of the HomeView.
        /// </summary>
        private const double OriginalHeight = 1080;

        /// <summary>
        /// This constant holds the content width of the HomeView.
        /// </summary>
        private const double ContentWidth = 766;

        /// <summary>
        /// This constant holds the content height of the HomeView.
        /// </summary>
        private const double ContentHeight = 792;

        /// <summary>
        /// This constant holds the background width of the HomeView.
        /// </summary>
        private const double BackgroundWidth = 866;

        /// <summary>
        /// This constant holds the background height of the HomeView.
        /// </summary>
        private const double BackgroundHeight = 892;

        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        /// <summary>
        /// This method initializes the HomeView class.
        /// </summary>
        public HomeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        //++++++++++++++ Methods: OnLoaded ++++++++++++++//
        /// <summary>
        /// This method is called when the HomeView is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }

        //++++++++++++++ Methods: OnSizeChanged ++++++++++++++//
        /// <summary>
        /// This method is called when the size of the HomeView changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
        }

        //++++++++++++++ Methods: UpdateLayout ++++++++++++++//
        /// <summary>
        /// This method updates the layout of the HomeView.
        /// </summary>
        private new void UpdateLayout()
        {
            double scaleX = ActualWidth / OriginalWidth;
            double scaleY = ActualHeight / OriginalHeight;
            double scale = Math.Min(scaleX, scaleY);

            const double MinScale = 0.5; // Set your desired minimum scale (e.g., 50% of original size)
            scale = Math.Max(scale, MinScale);

            // Update ContentPanel size
            ContentPanel.Width = ContentWidth * scale;
            ContentPanel.Height = ContentHeight * scale;

            // Update MenuBackgroundBorder size
            MenuBackgroundBorder.Width = BackgroundWidth * scale;
            MenuBackgroundBorder.Height = BackgroundHeight * scale;

            // Update button sizes
            foreach (var button in ButtonStack.Children)
            {
                if (button is Button btn)
                {
                    btn.Height = 100 * scale;
                    btn.Width = 705 * scale;
                    btn.FontSize = 24 * scale;
                }
            }

            // Update welcome message font sizes
            welcomeMsg.FontSize = 48 * scale;
            manageMsg.FontSize = 64 * scale;
        }

        //++++++++++++++ Methods: CloseButton_Click ++++++++++++++//
        /// <summary>
        /// This method closes the HomeView.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//