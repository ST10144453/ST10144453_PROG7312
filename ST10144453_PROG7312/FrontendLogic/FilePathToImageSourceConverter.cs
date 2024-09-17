using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class FilePathToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filePath && File.Exists(filePath))
            {
                return new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
