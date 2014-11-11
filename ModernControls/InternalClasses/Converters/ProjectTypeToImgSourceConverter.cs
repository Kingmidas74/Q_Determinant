using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media.Imaging;
using Core;

namespace ModernControls.InternalClasses.Converters
{
    public class ProjectTypeToImgSourceConverter : BaseConverter<ProjectTypeToImgSourceConverter>
    {
        private readonly Dictionary<ProjectTypes, string> _imagePathDictionary = new Dictionary<ProjectTypes, string>
        {
            {ProjectTypes.Execute, "/ModernControls;component/Assets/te1.png"},
            {ProjectTypes.Function, "Assets/te2.png"},
        };
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return _imagePathDictionary[(BlockTypes) value];
            return new BitmapImage(new Uri(_imagePathDictionary[(ProjectTypes)value], UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
