using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using ST10144453_PROG7312.MVVM.View_Model;
using ST10144453_PROG7312.MVVM.Model;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class Base64ToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mediaItem = value as MediaItem;
            if (mediaItem == null || !mediaItem.IsImage)
                return null;

            try
            {
                var base64String = mediaItem.Base64String;
                var imageData = System.Convert.FromBase64String(base64String);
                using (var stream = new MemoryStream(imageData))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    return bitmap;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
