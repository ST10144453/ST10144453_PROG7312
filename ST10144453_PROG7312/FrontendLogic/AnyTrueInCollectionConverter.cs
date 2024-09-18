//0000000000oooooooooo..........Start Of File..........ooooooooooo00000000000//
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    //============== Class: AnyTrueInCollectionConverter ==============//
    /// <summary>
    /// This class holds the base implementation for the AnyTrueInCollectionConverter class.
    /// </summary>
    public class AnyTrueInCollectionConverter : IValueConverter
    {
        //++++++++++++++ Methods: Convert ++++++++++++++//
        /// <summary>
        /// This method converts a collection of media items to a boolean value indicating if any media type is present.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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

        //++++++++++++++ Methods: ConvertBack ++++++++++++++//
        /// <summary>
        /// This method converts a boolean value to a collection of media items.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
//0000000000oooooooooo...........End Of File...........ooooooooooo00000000000//
