using System;
using System.Globalization;
using System.Windows;

namespace DefaultControlsPack.Converters
{
    public class IconPathToVisibilityConverter : BaseConverter<IconPathToVisibilityConverter>
    {
       
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return String.IsNullOrEmpty(value.ToString()) ? Visibility.Collapsed : Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
