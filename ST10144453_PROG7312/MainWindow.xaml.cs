﻿using System;
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

namespace ST10144453_PROG7312
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const double BlobSize = 2000; // Adjust size as needed
        private const double Margin = 300; // Increase margin to reduce overlap

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            StartAnimation();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            StartAnimation();
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
