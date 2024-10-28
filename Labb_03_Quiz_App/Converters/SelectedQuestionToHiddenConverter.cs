using Labb_03_Quiz_App.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Labb_03_Quiz_App.Converters
{
    internal class SelectedQuestionToHiddenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Question? Selected = null;
            if (value is Question q) Selected = q;

            if (Selected is null) return Visibility.Hidden;
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}