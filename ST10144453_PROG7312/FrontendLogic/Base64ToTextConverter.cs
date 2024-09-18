//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: Base64ToTextConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the Base64ToTextConverter class.
    /// </summary>
    public class Base64ToTextConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method converts a base64 string to a text string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var base64String = value as string;
            if (string.IsNullOrEmpty(base64String))
                return string.Empty;

            try
            {
                var fileBytes = System.Convert.FromBase64String(base64String);
                return Encoding.UTF8.GetString(fileBytes);
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                return string.Empty;
            }
        }

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method converts a text string to a base64 string.
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