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
            StartAnimation();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateCanvasDimensions();
            UpdateMenuBackgroundSizeAndPosition();
        }

        private void StartAnimation()
        {
            UpdateCanvasDimensions();

            double offsetX = _canvasWidth * 0.0;
            double offsetY = _canvasHeight * 0.0;

            var animation1 = new DoubleAnimation
            {
                From = -BlobImage1.ActualWidth + offsetX,
                To = _canvasWidth - offsetX,
                Duration = new Duration(TimeSpan.FromSeconds(10)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            BlobImage1.BeginAnimation(Canvas.LeftProperty, animation1);

            var animation2 = new DoubleAnimation
            {
                From = -BlobImage2.ActualHeight + offsetY,
                To = _canvasHeight - offsetY,
                Duration = new Duration(TimeSpan.FromSeconds(12)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            BlobImage2.BeginAnimation(Canvas.TopProperty, animation2);

            var animation3X = new DoubleAnimation
            {
                From = -BlobImage3.ActualWidth + offsetX,
                To = _canvasWidth - offsetX,
                Duration = new Duration(TimeSpan.FromSeconds(15)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            var animation3Y = new DoubleAnimation
            {
                From = -BlobImage3.ActualHeight + offsetY,
                To = _canvasHeight - offsetY,
                Duration = new Duration(TimeSpan.FromSeconds(15)),
                RepeatBehavior = RepeatBehavior.Forever,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            BlobImage3.BeginAnimation(Canvas.LeftProperty, animation3X);
            BlobImage3.BeginAnimation(Canvas.TopProperty, animation3Y);

            var animation4X = new DoubleAnimation
            {
                From = -BlobImage4.ActualWidth + offsetX,
                To = _canvasWidth - offsetX,
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

            Canvas.SetLeft(MenuBackground, (_canvasWidth - newWidth) / 2);
            Canvas.SetTop(MenuBackground, (_canvasHeight - newHeight) / 2);
            MenuBackground.Width = newWidth;
            MenuBackground.Height = newHeight;
        }
    }
}
