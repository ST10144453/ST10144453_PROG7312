//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: BoolToVisibilityConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the BoolToVisibilityConverter class.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method converts a boolean value to a visibility value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method converts a visibility value to a boolean value.
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