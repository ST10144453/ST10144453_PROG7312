//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: Base64ToPdfStreamConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the Base64ToPdfStreamConverter class.
    /// </summary>
    public class Base64ToPdfStreamConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method converts a base64 string to a PDF stream.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method converts a PDF stream to a base64 string.
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