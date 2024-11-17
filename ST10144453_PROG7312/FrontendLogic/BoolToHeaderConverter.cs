using System;
using System.Globalization;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class BoolToHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isStaff && parameter is string headerOptions)
            {
                // Split the parameter into staff|user headers
                var headers = headerOptions.Split('|');
                if (headers.Length == 2)
                {
                    return isStaff ? headers[0] : headers[1];
                }
            }
            return "Service Requests"; // Default header
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
