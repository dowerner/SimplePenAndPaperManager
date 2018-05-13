using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.View.Converter
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = parameter != null ? (Color)parameter : Colors.Blue;

            if (color == Colors.White)
            {
                return (bool)value ? new SolidColorBrush(color) : new SolidColorBrush(Colors.Black);
            }
            else
            {
                return (bool)value ? new SolidColorBrush(color) : new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = parameter != null ? (Color)parameter : Colors.Blue;
            return ((SolidColorBrush)value).Color == color;
        }
    }
}
