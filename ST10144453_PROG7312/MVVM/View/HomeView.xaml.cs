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
    //============== Class: AnimatedBackgroundView ==============//
    public partial class HomeView : UserControl
    {
        private double _canvasWidth;
        private double _canvasHeight;

        public HomeView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateCanvasDimensions();
            UpdateMenuBackgroundSizeAndPosition();
            UpdateWelcomeMessagePosition();
            UpdateManageMessagePosition();
            StartAnimation();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCanvasDimensions();
            UpdateMenuBackgroundSizeAndPosition();
            UpdateWelcomeMessagePosition();
            UpdateManageMessagePosition();
            UpdateButtonSizes();
        }

        private void StartAnimation()
        {
            UpdateCanvasDimensions();

            var animation1 = new DoubleAnimation
            {
                From = -BlobImage1.ActualWidth,
                To = _canvasWidth,
                Duration = new Duration(TimeSpan.FromSeconds(10)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            BlobImage1.BeginAnimation(Canvas.LeftProperty, animation1);

            var animation2 = new DoubleAnimation
            {
                From = -BlobImage2.ActualHeight,
                To = _canvasHeight,
                Duration = new Duration(TimeSpan.FromSeconds(12)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            BlobImage2.BeginAnimation(Canvas.TopProperty, animation2);

            var animation3X = new DoubleAnimation
            {
                From = -BlobImage3.ActualWidth,
                To = _canvasWidth,
                Duration = new Duration(TimeSpan.FromSeconds(15)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            var animation3Y = new DoubleAnimation
            {
                From = -BlobImage3.ActualHeight,
                To = _canvasHeight,
                Duration = new Duration(TimeSpan.FromSeconds(15)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            BlobImage3.BeginAnimation(Canvas.LeftProperty, animation3X);
            BlobImage3.BeginAnimation(Canvas.TopProperty, animation3Y);

            var animation4X = new DoubleAnimation
            {
                From = -BlobImage4.ActualWidth,
                To = _canvasWidth,
                Duration = new Duration(TimeSpan.FromSeconds(20)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            BlobImage4.BeginAnimation(Canvas.LeftProperty, animation4X);
        }

        private void UpdateCanvasDimensions()
        {
            _canvasWidth = ActualWidth;
            _canvasHeight = ActualHeight;
        }

        private void UpdateMenuBackgroundSizeAndPosition()
        {
            double originalWidth = 766;
            double originalHeight = 792;

            double widthRatio = _canvasWidth / 1910;
            double heightRatio = _canvasHeight / 1080;
            double scale = Math.Min(widthRatio, heightRatio);

            double newWidth = originalWidth * scale;
            double newHeight = originalHeight * scale;

            MenuBackground.Width = newWidth;
            MenuBackground.Height = newHeight;
            Canvas.SetLeft(MenuBackground, (_canvasWidth - newWidth) / 2);
            Canvas.SetTop(MenuBackground, (_canvasHeight - newHeight) / 2);
        }

        private void UpdateWelcomeMessagePosition()
        {
            double originalLeftOffset = 127;
            double originalTopOffset = 70;
            double originalFontSize = 48;

            double widthRatio = _canvasWidth / 1910;
            double heightRatio = _canvasHeight / 1080;
            double scale = Math.Min(widthRatio, heightRatio);

            double newLeftOffset = originalLeftOffset * scale;
            double newTopOffset = originalTopOffset * scale;
            double newFontSize = originalFontSize * scale;

            double menuBackgroundLeft = Canvas.GetLeft(MenuBackground);
            double menuBackgroundTop = Canvas.GetTop(MenuBackground);

            Canvas.SetLeft(welcomeMsg, menuBackgroundLeft + newLeftOffset);
            Canvas.SetTop(welcomeMsg, menuBackgroundTop + newTopOffset);
            welcomeMsg.FontSize = newFontSize;

            // Debugging information
            Console.WriteLine($"Menu Background Left: {menuBackgroundLeft}, Top: {menuBackgroundTop}");
            Console.WriteLine($"New Left Offset: {newLeftOffset}, New Top Offset: {newTopOffset}, New Font Size: {newFontSize}");
        }

        private void UpdateManageMessagePosition()
        {
            double originalLeftOffset = 127;
            double originalTopOffset = 110;
            double originalFontSize = 64;

            double widthRatio = _canvasWidth / 1910;
            double heightRatio = _canvasHeight / 1080;
            double scale = Math.Min(widthRatio, heightRatio);

            double newLeftOffset = originalLeftOffset * scale;
            double newTopOffset = originalTopOffset * scale;
            double newFontSize = originalFontSize * scale;

            double menuBackgroundLeft = Canvas.GetLeft(MenuBackground);
            double menuBackgroundTop = Canvas.GetTop(MenuBackground);

            Canvas.SetLeft(manageMsg, menuBackgroundLeft + newLeftOffset);
            Canvas.SetTop(manageMsg, menuBackgroundTop + newTopOffset);
            manageMsg.FontSize = newFontSize;
        }

        private void UpdateButtonSizes()
        {
            double originalWidth = 607;
            double originalHeight = 100;

            double widthRatio = _canvasWidth / 1910;
            double heightRatio = _canvasHeight / 1080;
            double scale = Math.Min(widthRatio, heightRatio);

            double newWidth = originalWidth * scale;
            double newHeight = originalHeight * scale;

            StackPanel stackPanel = LogicalTreeHelper.FindLogicalNode(this, "ButtonStackPanel") as StackPanel;
            if (stackPanel != null)
            {
                foreach (var child in stackPanel.Children.OfType<Button>())
                {
                    child.Width = newWidth;
                    child.Height = newHeight;
                }
            }
        }

    }

}
