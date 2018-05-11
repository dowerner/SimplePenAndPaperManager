using SimplePenAndPaperManager.MapEditor.Entities;
using SimplePenAndPaperManager.UserInterface.Model;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SimplePenAndPaperManager.MathTools
{
    public static class Utils
    {
        public static double PxToMeter(double px)
        {
            return px / Constants.PxPerMeter;
        }

        public static double MeterToPx(double meter)
        {
            return meter * Constants.PxPerMeter;
        }

        public static Point PxToMeter(this Point pxPoint)
        {
            return pxPoint.Mult(1 / Constants.PxPerMeter);
        }

        public static Point MeterToPx(this Point meterPoint)
        {
            return meterPoint.Mult(Constants.PxPerMeter);
        }

        public static bool AddIfNoPointPresent(this List<Point2D> points, Point2D point, double searchRadius = 3)
        {
            foreach(Point2D existingPoint in points)
            {
                if (point.Distance(existingPoint) < searchRadius) return false;   // return and do not add if point is close to existing point
            }
            points.Add(point);
            return true;
        }

        public static double Distance(this Point2D point1, Point2D point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        public static double Distance(this Point2D point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        public static double Distance(this Point point1, Point2D point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        public static double Distance(this Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }

        public static Point Sub(this Point point1, Point point2)
        {
            return new Point(point1.X - point2.X, point1.Y - point2.Y);
        }

        public static Point Add(this Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static Point Mult(this Point point, double factor)
        {
            return new Point(factor * point.X, factor * point.Y);
        }

        public static double Dot(this Point point1, Point point2)
        {
            return point1.X * point2.X + point1.Y * point2.Y;
        }
    }
}
