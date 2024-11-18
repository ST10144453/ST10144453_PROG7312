//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ST10144453_PROG7312.Core
{
    /// <summary>
    /// Converts a <see cref="TimeSpan"/> to a nullable <see cref="TimeSpan"/> and vice versa.
    /// </summary>
    public class TimeSpanToNullableConverter : IValueConverter
    {
        /// <summary>
        /// Converts a <see cref="TimeSpan"/> to a nullable <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">The <see cref="TimeSpan"/> value to convert.</param>
        /// <param name="targetType">The target type of the conversion.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>A nullable <see cref="TimeSpan"/> value.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TimeSpan timeSpan ? (TimeSpan?)timeSpan : null;
        }

        /// <summary>
        /// Converts a nullable <see cref="TimeSpan"/> to a <see cref="TimeSpan"/>.
        /// </summary>
        /// <param name="value">The nullable <see cref="TimeSpan"/> value to convert.</param>
        /// <param name="targetType">The target type of the conversion.</param>
        /// <param name="parameter">An optional parameter for the conversion.</param>
        /// <param name="culture">The culture to use for the conversion.</param>
        /// <returns>A <see cref="TimeSpan"/> value.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is TimeSpan nullableTimeSpan ? (TimeSpan?)nullableTimeSpan : null;
        }
    }

}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
