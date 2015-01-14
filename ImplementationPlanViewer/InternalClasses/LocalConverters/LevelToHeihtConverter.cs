using System;
using System.Globalization;
using DefaultControlsPack.Converters;

namespace ImplementationPlanViewer.InternalClasses.LocalConverters
{
    public class LevelToHeihtConverter : BaseConverter<LevelToHeihtConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((ulong) value)*30 + 4*30 + 40*((ulong) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
