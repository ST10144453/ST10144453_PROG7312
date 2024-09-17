using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class AnyTrueInCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var mediaItems = value as IEnumerable<MediaItem>;
            if (mediaItems != null)
            {
                var isPdfCheck = mediaItems.Any(item => item.IsPdf);
                var isWordCheck = mediaItems.Any(item => item.IsWord);
                return isPdfCheck || isWordCheck;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

