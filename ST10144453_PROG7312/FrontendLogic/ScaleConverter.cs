//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: ScaleConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the ScaleConverter class.
    /// </summary>
    public class ScaleConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method scales a value by a given factor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a valid number
            if (value == null || parameter == null)
            {
                return DependencyProperty.UnsetValue;
            }

            try
            {
                // Convert the value and the parameter to double
                double inputValue = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
                double scaleFactor = System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);

                // Return the scaled value
                return inputValue * scaleFactor;
            }
            catch (FormatException)
            {
                // Handle incorrect format
                return DependencyProperty.UnsetValue;
            }
            catch (InvalidCastException)
            {
                // Handle invalid casting
                return DependencyProperty.UnsetValue;
            }
        }

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method scales a value back by a given factor.
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