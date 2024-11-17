//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.View_Model;
using ST10144453_PROG7312.MVVM.ViewMode;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using ST10144453_PROG7312.Core;

namespace ST10144453_PROG7312
{
    //============== Class: MainWindow ==============//
    /// <summary>
    /// This class holds the base implementation for the MainWindow class.
    /// </summary>
    public partial class MainWindow : Window
    {
        //++++++++++++++ Constants ++++++++++++++//
        /// <summary>
        /// This constant holds the size of the blob.
        /// </summary>
        private const double BlobSize = 2000;

        /// <summary>
        /// This constant holds the duration of the animation.
        /// </summary>
        private const double AnimationDuration = 30;

        //++++++++++++++ Methods: Default Constructor ++++++++++++++//
        /// <summary>
        /// This method initializes the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
            DataContext = new MainWindowViewModel();
            MainContentControl.Content = new MVVM.View.LoginRegisterMenu();
            var loginRegisterViewModel = new LoginRegisterViewModel();

        }

        //++++++++++++++ Methods: OnLoaded ++++++++++++++//
        /// <summary>
        /// This method is called when the MainWindow is loaded.
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
        /// This method is called when the size of the MainWindow changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetInitialBlobPositions();
            StartAnimation();
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

        //++++++++++++++ Methods: MinimizeButton_Click ++++++++++++++//
        /// <summary>
        /// This method minimizes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this) != null)
            {
                Window.GetWindow(this).WindowState = WindowState.Minimized;
            }
        }

        //++++++++++++++ Methods: MaximizeButton_Click ++++++++++++++//
        /// <summary>
        /// This method maximizes the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this)?.WindowState == WindowState.Maximized)
            {
                Window.GetWindow(this)?.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
                MaximizeButton.Content = "🗖";
            }
            else
            {
                Window.GetWindow(this)?.SetCurrentValue(Window.WindowStateProperty, WindowState.Maximized);
                MaximizeButton.Content = "🗗";
            }
        }

        //++++++++++++++ Methods: OnMouseLeftButtonDown ++++++++++++++//
        /// <summary>
        /// This method allows the window to be dragged.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                Window.GetWindow(this)?.DragMove();
            }
        }


        public void Navigate(UserControl nextPage)
        {
            MainContentControl.Content = nextPage;
        }
    }
}
//0000000000oooooooooo..........End Of File..........ooooooooooo00000000000//