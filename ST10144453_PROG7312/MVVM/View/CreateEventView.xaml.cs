using ST10144453_PROG7312.MVVM.Model;
using ST10144453_PROG7312.MVVM.View_Model;
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
    /// Interaction logic for CreateEventView.xaml
    /// </summary>
    public partial class CreateEventView : Window
    {

        //++++++++++++++ Constants ++++++++++++++//
        /// <summary>
        /// This constant holds the size of the blob.
        /// </summary>
        private const double BlobSize = 1000;

        /// <summary>
        /// This constant holds the duration of the animation.
        /// </summary>
        private const double AnimationDuration = 30;

        public List<TagsModel> Tags { get; set; }

        public CreateEventView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            DataContext = new CreateEventViewModel();
            Tags = TagsModel.Tags; // Assuming Tags is a property in your ViewModel
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

        private void CreateEvent_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
