using Newtonsoft.Json.Linq;
using ST10144453_PROG7312.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ST10144453_PROG7312.FrontendLogic
{
    public class TagSelectionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var selectedTags = values[0] as IList<string>;
            var currentTag = values[1] as string;

            if (selectedTags != null && currentTag != null)
            {
                return selectedTags.Contains(currentTag);
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            var isChecked = (bool)value;
            var selectedTags = targetTypes[0] as IList<string>;
            var currentTag = parameter as string;

            if (selectedTags != null && currentTag != null)
            {
                if (isChecked && !selectedTags.Contains(currentTag))
                {
                    selectedTags.Add(currentTag);
                }
                else if (!isChecked && selectedTags.Contains(currentTag))
                {
                    selectedTags.Remove(currentTag);
                }
            }

            return new object[] { selectedTags, currentTag };
        }
    }
}
