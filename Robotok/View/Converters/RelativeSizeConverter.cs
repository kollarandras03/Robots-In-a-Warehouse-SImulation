﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Robotok.View.Converters
{
    public class RelativeSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null || value == DependencyProperty.UnsetValue ||
                parameter == null || parameter == DependencyProperty.UnsetValue)
                return 0;
            return (double)value * System.Convert.ToDouble(((String)parameter).Replace('.', ','));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
