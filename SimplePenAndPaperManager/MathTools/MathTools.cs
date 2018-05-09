using SimplePenAndPaperManager.MapEditor.Entities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SimplePenAndPaperManager.MathTools
{
    public static class MathTools
    {
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
    }
}
