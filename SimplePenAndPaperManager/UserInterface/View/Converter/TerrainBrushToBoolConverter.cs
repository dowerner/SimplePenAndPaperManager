using SimplePenAndPaperManager.UserInterface.View.States;
using SimplePenAndPaperManager.UserInterface.ViewModel.DataModels;
using System;
using System.Globalization;
using System.Windows.Data;

namespace SimplePenAndPaperManager.UserInterface.View.Converter
{
    public class TerrainBrushToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TerrainBrush brush = (TerrainBrush)value;

            switch (int.Parse((string)parameter))
            {
                case 0:
                    return brush == TerrainBrush.Rectangle;
                case 1:
                    return brush == TerrainBrush.Circle;
                default:
                    return brush == TerrainBrush.None;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isEnabled = (bool)value;

            switch (int.Parse((string)parameter))
            {
                case 0:
                    return isEnabled ? TerrainBrush.Rectangle : DataModel.Instance.TerrainBrush;
                case 1:
                    return isEnabled ? TerrainBrush.Circle : DataModel.Instance.TerrainBrush;
                default:
                    return isEnabled ? TerrainBrush.None : DataModel.Instance.TerrainBrush;
            }
        }
    }
}
