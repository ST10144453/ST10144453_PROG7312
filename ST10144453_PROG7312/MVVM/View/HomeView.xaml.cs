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
        private const int BlobCount = 4; // Number of blobs
        private const double OverlapThreshold = 0.3; // Allowable overlap percentage
        private const double BlobSize = 2000; // Adjust size as needed
        private const double Margin = 300; // Increase margin to reduce overlap

        public HomeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
            StartAnimation();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateLayout();
            StartAnimation();
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


        private void SetInitialBlobPositions()
        {
            double windowWidth = ActualWidth;
            double windowHeight = ActualHeight;

            // Ensure blobs start within the visible window area with margins
            Canvas.SetLeft(BlobImage1, Margin);
            Canvas.SetTop(BlobImage1, Margin);

            Canvas.SetLeft(BlobImage2, Margin);
            Canvas.SetTop(BlobImage2, Margin);

            Canvas.SetLeft(BlobImage3, Margin);
            Canvas.SetTop(BlobImage3, Margin);

            Canvas.SetLeft(BlobImage4, Margin);
            Canvas.SetTop(BlobImage4, Margin);
        }



        private void StartAnimation()
        {
            double windowWidth = ActualWidth;
            double windowHeight = ActualHeight;

            AnimateBlob(BlobImage1, Canvas.LeftProperty, -BlobSize + Margin, windowWidth - Margin, 10);
            AnimateBlob(BlobImage1, Canvas.TopProperty, -BlobSize + Margin, windowHeight - Margin, 10);

            AnimateBlob(BlobImage2, Canvas.LeftProperty, -BlobSize + Margin, windowWidth - Margin, 10);
            AnimateBlob(BlobImage2, Canvas.BottomProperty, -BlobSize + Margin, windowHeight - Margin, 10);

            AnimateBlob(BlobImage3, Canvas.RightProperty, -BlobSize + Margin, windowWidth - Margin, 10);
            AnimateBlob(BlobImage3, Canvas.TopProperty, -BlobSize + Margin, windowHeight - Margin, 10);

            AnimateBlob(BlobImage4, Canvas.RightProperty, -BlobSize + Margin, windowWidth - Margin, 10);
            AnimateBlob(BlobImage4, Canvas.BottomProperty, -BlobSize + Margin, windowHeight - Margin, 10);
        }


        private void AnimateBlob(UIElement element, DependencyProperty property, double from, double to, double durationInSeconds)
        {
            var animation = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = new Duration(TimeSpan.FromSeconds(durationInSeconds)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            element.BeginAnimation(property, animation);
        }
    }
}