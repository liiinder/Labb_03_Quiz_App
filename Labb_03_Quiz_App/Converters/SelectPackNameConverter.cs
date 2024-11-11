using System.Globalization;
using System.Windows.Data;

namespace Labb_03_Quiz_App.Converters
{
    internal class SelectPackNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{values[0]} - {values[1]} Questions ({values[2]})";
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
