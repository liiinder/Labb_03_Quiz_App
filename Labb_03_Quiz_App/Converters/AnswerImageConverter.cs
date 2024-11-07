using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace Labb_03_Quiz_App.Converters
{
    internal class AnswerImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currDir = Directory.GetCurrentDirectory();
            var file = Path.Combine(currDir, "assets");
            if (value is string color)
            {
                if (color == "Red") file = Path.Combine(file, "incorrect.png");
                else if (color == "Green") file = Path.Combine(file, "correct.png");
                else file = null;
            }

            return file;

            //TODO: add some gradient to the border colors.
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
