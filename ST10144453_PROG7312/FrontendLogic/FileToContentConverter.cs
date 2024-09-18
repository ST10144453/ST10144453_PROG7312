//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: FileToContentConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the FileToContentConverter class.
    /// </summary>
    public class FileToContentConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method converts a base64 string to an image or text content.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method converts an image or text content to a base64 string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //++++++++++++++ Methods: IsBase64Image ++++++++++++++//
        /// <summary>
        /// This method determines if the base64 string is an image.
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        private bool IsBase64Image(string base64String)
        {
            // Simple check to determine if the base64 string is an image
            // You might need a more robust method depending on your use case
            return base64String.StartsWith("iVBORw0KGgo"); // Example header for PNG
        }

        //++++++++++++++ Methods: IsBase64Text ++++++++++++++//
        /// <summary>
        /// This method determines if the base64 string is text.
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        private bool IsBase64Text(string base64String)
        {
            // Simple check to determine if the base64 string is text
            // This is a placeholder and may need adjustment
            return !IsBase64Image(base64String);
        }

        //++++++++++++++ Methods: ConvertBase64ToImage ++++++++++++++//
        /// <summary>
        /// This method converts a base64 string to an image.
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
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

        //++++++++++++++ Methods: ConvertBase64ToText ++++++++++++++//
        /// <summary>
        /// This method converts a base64 string to text.
        /// </summary>
        /// <param name="base64String"></param>
        /// <returns></returns>
        private string ConvertBase64ToText(string base64String)
        {
            byte[] textBytes = System.Convert.FromBase64String(base64String);
            return System.Text.Encoding.UTF8.GetString(textBytes);
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//