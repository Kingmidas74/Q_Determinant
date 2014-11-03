using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Media.Imaging;
using Core;

namespace ModernControls.InternalClasses.Converters
{
    public class BlockTypeToImgSourceConverter : BaseConverter<BlockTypeToImgSourceConverter>
    {
        private readonly Dictionary<BlockTypes, string> _imagePathDictionary = new Dictionary<BlockTypes, string>
        {
            {BlockTypes.Start, "/ModernControls;component/Assets/te1.png"},
            {BlockTypes.End, "Assets/te2.png"},
            {BlockTypes.Input, @"Assets\te2.png"},
            {BlockTypes.Output, "InOut.png"},
            {BlockTypes.Process, "Process.png"},
            {BlockTypes.Condition, "Condition.png"},
        };
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return _imagePathDictionary[(BlockTypes) value];
            return new BitmapImage(new Uri(_imagePathDictionary[(BlockTypes)value], UriKind.Relative));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
