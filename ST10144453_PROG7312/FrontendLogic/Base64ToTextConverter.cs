using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class Base64ToTextConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
