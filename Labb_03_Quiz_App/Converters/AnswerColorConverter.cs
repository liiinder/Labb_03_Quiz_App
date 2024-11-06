using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Labb_03_Quiz_App.Converters
{
    internal class AnswerColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(0, 0, 0, 0);
            if (value is string color)
            {
                if (color == "Red") brush.Color = Colors.Red;
                else if (color == "Green") brush.Color = Colors.LimeGreen;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
