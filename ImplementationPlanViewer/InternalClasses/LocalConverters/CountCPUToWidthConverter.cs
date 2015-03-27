﻿using System;
using System.Globalization;
using DefaultControlsPack.Converters;

namespace ImplementationPlanViewer.InternalClasses.LocalConverters
{

    public class CountCPUToWidthConverter : BaseConverter<CountCPUToWidthConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return 2*30 + (2*30*((ulong) value) + 20);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}