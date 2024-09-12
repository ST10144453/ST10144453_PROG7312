using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class FileToContentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64String)
            {
                // Determine if the base64 string represents an image or text
                if (IsBase64Image(base64String))
                {
                    return ConvertBase64ToImage(base64String);
                }
                else if (IsBase64Text(base64String))
                {
                    return ConvertBase64ToText(base64String);
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private bool IsBase64Image(string base64String)
        {
            // Simple check to determine if the base64 string is an image
            // You might need a more robust method depending on your use case
            return base64String.StartsWith("iVBORw0KGgo"); // Example header for PNG
        }

        private bool IsBase64Text(string base64String)
        {
            // Simple check to determine if the base64 string is text
            // This is a placeholder and may need adjustment
            return !IsBase64Image(base64String);
        }

        private ImageSource ConvertBase64ToImage(string base64String)
        {
            byte[] imageBytes = System.Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return bitmap;
            }
        }

        private string ConvertBase64ToText(string base64String)
        {
            byte[] textBytes = System.Convert.FromBase64String(base64String);
            return System.Text.Encoding.UTF8.GetString(textBytes);
        }
    }
}

