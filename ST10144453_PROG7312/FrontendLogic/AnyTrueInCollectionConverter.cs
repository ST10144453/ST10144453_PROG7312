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
                // Check if there are any items of each type
                var hasPdf = mediaItems.Any(item => item.IsPdf);
                var hasWord = mediaItems.Any(item => item.IsWord);
                var hasImage = mediaItems.Any(item => item.IsImage);
                var hasText = mediaItems.Any(item => item.IsText);

                // Return true if any type of media is present
                return hasPdf || hasWord || hasImage || hasText;
            }
            return false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

