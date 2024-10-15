//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: Base64ToImageSourceConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the Base64ToImageSourceConverter class.
    /// </summary>
    public class Base64ToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string base64String && !string.IsNullOrEmpty(base64String))
            {
                try
                {
                    byte[] imageBytes = System.Convert.FromBase64String(base64String);
                    using (var ms = new MemoryStream(imageBytes))
                    {
                        var image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = ms;
                        image.CacheOption = BitmapCacheOption.OnLoad;
                        image.EndInit();
                        return image;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting base64 string to image: {ex.Message}");
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
            Console.WriteLine("ConvertBack method not implemented.");
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//