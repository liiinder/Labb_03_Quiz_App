﻿using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Labb_03_Quiz_App.Converters
{
    internal class TrueToHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is true ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}