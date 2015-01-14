using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
