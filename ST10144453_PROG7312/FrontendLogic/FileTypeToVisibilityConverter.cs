//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: FileTypeToVisibilityConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the FileTypeToVisibilityConverter class.
    /// </summary>
    public class FileTypeToVisibilityConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method converts a file path to a visibility value based on the file type.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return Visibility.Collapsed;
            }

            string filePath = value.ToString();
            string param = parameter.ToString();

            string extension = Path.GetExtension(filePath).ToLower();

            if (param == "Image" && (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".bmp" || extension == ".gif"))
            {
                return Visibility.Visible;
            }

            if (param == "Text")
            {
                return Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method converts a visibility value to a file path based on the file type.
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