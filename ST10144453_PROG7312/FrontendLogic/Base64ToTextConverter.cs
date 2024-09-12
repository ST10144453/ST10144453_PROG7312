using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class Base64ToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64String)
            {
                byte[] textBytes = System.Convert.FromBase64String(base64String);
                return System.Text.Encoding.UTF8.GetString(textBytes);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
