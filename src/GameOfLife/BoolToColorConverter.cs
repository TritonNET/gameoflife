using System.Globalization;

namespace GameOfLife
{
    public sealed class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is bool b && b) ? Colors.Black : Colors.White;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Color color && color == Colors.Black;
    }
}
