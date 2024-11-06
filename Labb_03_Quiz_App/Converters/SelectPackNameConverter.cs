using Labb_03_Quiz_App.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Labb_03_Quiz_App.Converters
{
    internal class SelectPackNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string count = string.Empty;
            if (values[2] is Collection<Question> e) count = $"{e.Count}";
            return $"{values[0]} - {count} Questions ({values[1]})";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
