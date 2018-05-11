using SimplePenAndPaperManager.MathTools;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace SimplePenAndPaperManager.UserInterface.View.Converter
{
    class MeterToPixelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is PointCollection)
            {
                PointCollection meterPoints = new PointCollection();
                foreach (Point point in (PointCollection)value) meterPoints.Add(point.MeterToPx());
                return meterPoints;
            }
            if (value is Point) return ((Point)value).MeterToPx();
            return Utils.MeterToPx((double)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is PointCollection)
            {
                PointCollection meterPoints = new PointCollection();
                foreach (Point point in (PointCollection)value) meterPoints.Add(point.PxToMeter());
                return meterPoints;
            }
            if (value is Point) return ((Point)value).PxToMeter();
            return Utils.PxToMeter((double)value);
        }
    }
}
