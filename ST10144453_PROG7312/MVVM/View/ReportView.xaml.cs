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
using System.Windows.Shapes;

namespace ST10144453_PROG7312.MVVM.View
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Window
    {
        private const double BlobSize = 2000; // Adjust size as needed
        private const double Margin = 300; // Increase margin to reduce overlap
        private const double OriginalWidth = 1910;
        private const double OriginalHeight = 1080;
        private const double ContentWidth = 766;
        private const double ContentHeight = 792;
        private const double BackgroundWidth = 866;
        private const double BackgroundHeight = 892;
        private const double ButtonWidth = 280;
        private const double ButtonFontSize = 30;
        private ReportUserControl _reportIssueView;
        private ForumUserControl _pastReportsView;

        public ReportView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;

            _reportIssueView = new ReportUserControl();
            _pastReportsView = new ForumUserControl();

            MainContentControl.Content = _reportIssueView;
        }

        private void NavigateToReportIssue_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = _reportIssueView;
        }

        // Event handler for Past Reports button
        private void NavigateToPastReports_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = _pastReportsView;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            StartAnimation();
            UpdateLayout();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            StartAnimation();
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            double scaleX = ActualWidth / OriginalWidth;
            double scaleY = ActualHeight / OriginalHeight;
            double scale = Math.Min(scaleX, scaleY);

            const double MinScale = 0.5;
            scale = Math.Max(scale, MinScale);

            ContentPanel.Width = ContentWidth * scale;
            ContentPanel.Height = ContentHeight * scale;

            foreach (var button in ButtonStack.Children)
            {
                if (button is Button btn)
                {
                    btn.Width = ButtonWidth * scale;
                    btn.FontSize = ButtonFontSize * scale;
                    btn.Margin = new Thickness(0, 5 * scale, 0, 5 * scale);
                }
            }
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
