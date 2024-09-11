using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class ScaleConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
