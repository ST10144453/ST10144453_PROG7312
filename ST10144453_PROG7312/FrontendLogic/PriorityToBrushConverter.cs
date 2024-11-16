using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class PriorityToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isHighPriority)
            {
                if (isHighPriority)
                {
                    // High priority: Light red background
                    return new SolidColorBrush(Color.FromArgb(255, 255, 235, 238)); // #FFEBEE
                }
                else
                {
                    // Normal priority: Light blue background
                    return new SolidColorBrush(Color.FromArgb(255, 227, 242, 253)); // #E3F2FD
                }
            }

            // Default fallback color
            return new SolidColorBrush(Colors.White);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}