using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class EventPhotoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var photos = value as List<string>;
            int index = 0;
            if (parameter != null)
            {
                int.TryParse(parameter.ToString(), out index);
            }

            if (photos != null && photos.Count > index)
            {
                // Assuming the photo is a base64 string, convert it to a BitmapImage
                var photoData = photos[index];
                byte[] binaryData = System.Convert.FromBase64String(photoData); // Fully qualify the Convert method
                var bitmap = new BitmapImage();
                using (var stream = new System.IO.MemoryStream(binaryData))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }
                return bitmap;
            }
            // Return a default image or null
            return new BitmapImage(new Uri("pack://application:,,,/Resources/Hardcoded/ProfilePhoto/koos_pfp.jpeg"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
