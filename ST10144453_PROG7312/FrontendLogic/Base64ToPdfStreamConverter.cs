using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class Base64ToPdfStreamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64String)
            {
                byte[] pdfBytes = System.Convert.FromBase64String(base64String);
                MemoryStream pdfStream = new MemoryStream(pdfBytes);
                return pdfStream;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
