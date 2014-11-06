using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernControls.InternalClasses.Converters
{
    class BoolToStarConverter : BaseConverter<BoolToStarConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEditable && (value as IEditable).IsChange)
            {
                return '*';
                
            }
            return ' ';
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}