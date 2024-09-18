//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: FilePathToImageSourceConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the FilePathToImageSourceConverter class.
    /// </summary>
    public class FilePathToImageSourceConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method converts a file path to an image source.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string filePath && File.Exists(filePath))
            {
                return new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));
            }
            return null;
        }

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method converts an image source to a file path.
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
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//