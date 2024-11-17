using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                switch (status.ToLower())
                {
                    case "pending":
                        return new SolidColorBrush(Color.FromRgb(255, 165, 0));    // Orange
                    case "in progress":
                        return new SolidColorBrush(Color.FromRgb(0, 119, 204));    // Blue
                    case "completed":
                        return new SolidColorBrush(Color.FromRgb(46, 139, 87));    // Green
                    case "rejected":
                        return new SolidColorBrush(Color.FromRgb(220, 20, 60));    // Red
                    default:
                        return new SolidColorBrush(Colors.Gray);                   // Default gray
                }
            }

            return new SolidColorBrush(Colors.Gray); // Default return value
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
