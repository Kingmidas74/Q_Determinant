using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace ModernControls.InternalClasses.Converters
{
    public class LogsTypeToColorConverter: BaseConverter<LogsTypeToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            LogType type = (LogType)value;
            switch (type)
            {
                case LogType.Error:
                    return (SolidColorBrush) Application.Current.Resources["ErrorFontColorBrush"];
                case LogType.Success: return (SolidColorBrush)Application.Current.Resources["SuccessFontColorBrush"];

            }
            return (SolidColorBrush)Application.Current.Resources["BasicFontColorBrush"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}