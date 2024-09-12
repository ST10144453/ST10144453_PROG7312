using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ST10144453_PROG7312.MVVM.View
{
    public partial class HomeView : UserControl
    {
        private const double OriginalWidth = 1910;
        private const double OriginalHeight = 1080;
        private const double ContentWidth = 766;
        private const double ContentHeight = 792;
        private const double BackgroundWidth = 866;
        private const double BackgroundHeight = 892;

        public HomeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
        }

        private void UpdateLayout()
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


        
    }
}