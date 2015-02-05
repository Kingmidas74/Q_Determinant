using System;
using System.Globalization;
using System.Windows.Media;
using DefaultControlsPack.Converters;

namespace ImplementationPlanViewer.InternalClasses.LocalConverters
{
    public class BlockSelectedToColorConverter : BaseConverter<BlockSelectedToColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((bool) value))
            {
                return new SolidColorBrush(Colors.Red);
            }
            else
            {
                return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
