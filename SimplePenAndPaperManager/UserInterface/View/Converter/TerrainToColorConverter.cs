using SimplePenAndPaperManager.MapEditor;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.View.Converter
{
    public class TerrainToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((FloorMaterial)value)
            {
                case FloorMaterial.Asphalt:
                    return new SolidColorBrush(Colors.Gray);
                case FloorMaterial.Grass:
                    return new SolidColorBrush(Colors.DarkGreen);
                case FloorMaterial.Stone:
                    return new SolidColorBrush(Colors.LightGray);
                case FloorMaterial.Water:
                    return new SolidColorBrush(Colors.Blue);
                case FloorMaterial.Wood:
                    return new SolidColorBrush(Colors.BurlyWood);
                default:
                    return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = ((SolidColorBrush)value).Color;

            if(color == Colors.Gray) return FloorMaterial.Asphalt;
            else if (color == Colors.DarkGreen) return FloorMaterial.Grass;
            else if (color == Colors.LightGray) return FloorMaterial.Stone;
            else if (color == Colors.Blue) return FloorMaterial.Water;
            else if (color == Colors.BurlyWood) return FloorMaterial.Wood;
            else return FloorMaterial.None;
        }
    }
}
