//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ST10144453_PROG7312.MVVM.View
{
    //============== Class: ReportView ==============//
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : Window
    {
        //++++++++++++++ Constants ++++++++++++++//
        /// <summary>
        /// This constant holds the size of the blob.
        /// </summary>
        private const double BlobSize = 400; // Reduced blob size for better visibility

        /// <summary>
        /// This constant holds the margin of the blob.
        /// </summary>
        private const double Margin = 50;

        /// <summary>
        /// This constant holds the duration of the animation.
        /// </summary>
        private const double AnimationDuration = 15;

        /// <summary>
        /// This constant holds the duration of the animation.
        /// </summary>
        private ReportUserControl _reportIssueView;

        /// <summary>
        /// This constant holds the duration of the animation.
        /// </summary>
        private ForumUserControl _pastReportsView;

        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        /// <summary>
        /// This method initializes the ReportView class.
        /// </summary>
        public ReportView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;

            _reportIssueView = new ReportUserControl();
            _pastReportsView = new ForumUserControl();

            MainContentControl.Content = _reportIssueView;
        }

        //++++++++++++++ Methods: NavigateToReportIssue_Click ++++++++++++++//
        /// <summary>
        /// This method navigates to the ReportIssue view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToReportIssue_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = _reportIssueView;
        }

        //++++++++++++++ Methods: NavigateToPastReports_Click ++++++++++++++//
        /// <summary>
        /// This method navigates to the PastReports view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NavigateToPastReports_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.Content = _pastReportsView;
        }

        //++++++++++++++ Methods: OnLoaded ++++++++++++++//
        /// <summary>
        /// This method is called when the ReportView is loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SetInitialBlobPositions();
            StartAnimation();
        }

        //++++++++++++++ Methods: OnSizeChanged ++++++++++++++//
        /// <summary>
        /// This method is called when the size of the ReportView changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetInitialBlobPositions();
            StartAnimation();
        }

        //++++++++++++++ Methods: MinimizeButton_Click ++++++++++++++//
        /// <summary>
        /// This method minimizes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //++++++++++++++ Methods: MaximizeButton_Click ++++++++++++++//
        /// <summary>
        /// This method maximizes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaximizeButton.Content = "🗖";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaximizeButton.Content = "🗗";
            }
        }

        //++++++++++++++ Methods: CloseButton_Click ++++++++++++++//
        /// <summary>
        /// This method closes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //++++++++++++++ Methods: OnMouseLeftButtonDown ++++++++++++++//
        /// <summary>
        /// This method allows the window to be dragged.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        //++++++++++++++ Methods: SetInitialBlobPositions ++++++++++++++//
        /// <summary>
        /// This method sets the initial positions of the blobs.
        /// </summary>
        private void SetInitialBlobPositions()
        {
            double windowWidth = ActualWidth;
            double windowHeight = ActualHeight;

            // Position blobs in each quadrant
            SetBlobPosition(BlobImage1, -BlobSize / 2, -BlobSize / 2);
            SetBlobPosition(BlobImage2, windowWidth - BlobSize / 2, -BlobSize / 2);
            SetBlobPosition(BlobImage3, -BlobSize / 2, windowHeight - BlobSize / 2);
            SetBlobPosition(BlobImage4, windowWidth - BlobSize / 2, windowHeight - BlobSize / 2);
        }

        //++++++++++++++ Methods: SetBlobPosition ++++++++++++++//
        /// <summary>
        /// This method sets the position of the blob.
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        private void SetBlobPosition(UIElement blob, double left, double top)
        {
            Canvas.SetLeft(blob, left);
            Canvas.SetTop(blob, top);
        }

        //++++++++++++++ Methods: StartAnimation ++++++++++++++//
        /// <summary>
        /// This method starts the animation of the blobs.
        /// </summary>
        private void StartAnimation()
        {
            double windowWidth = ActualWidth;
            double windowHeight = ActualHeight;
            double quadrantWidth = windowWidth / 2;
            double quadrantHeight = windowHeight / 2;

            // Animate blobs within their respective quadrants
            AnimateBlob(BlobImage1, -BlobSize / 2, quadrantWidth - BlobSize / 2, -BlobSize / 2, quadrantHeight - BlobSize / 2);
            AnimateBlob(BlobImage2, quadrantWidth - BlobSize / 2, windowWidth - BlobSize / 2, -BlobSize / 2, quadrantHeight - BlobSize / 2);
            AnimateBlob(BlobImage3, -BlobSize / 2, quadrantWidth - BlobSize / 2, quadrantHeight - BlobSize / 2, windowHeight - BlobSize / 2);
            AnimateBlob(BlobImage4, quadrantWidth - BlobSize / 2, windowWidth - BlobSize / 2, quadrantHeight - BlobSize / 2, windowHeight - BlobSize / 2);
        }

        //++++++++++++++ Methods: AnimateBlob ++++++++++++++//
        /// <summary>
        /// This method animates the blob.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="leftMin"></param>
        /// <param name="leftMax"></param>
        /// <param name="topMin"></param>
        /// <param name="topMax"></param>
        private void AnimateBlob(UIElement element, double leftMin, double leftMax, double topMin, double topMax)
        {
            AnimateProperty(element, Canvas.LeftProperty, leftMin, leftMax);
            AnimateProperty(element, Canvas.TopProperty, topMin, topMax);
        }

        //++++++++++++++ Methods: AnimateProperty ++++++++++++++//
        /// <summary>
        /// This method animates the property.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="property"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        private void AnimateProperty(UIElement element, DependencyProperty property, double min, double max)
        {
            var animation = new DoubleAnimation
            {
                From = min,
                To = max,
                Duration = TimeSpan.FromSeconds(AnimationDuration),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            element.BeginAnimation(property, animation);
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//