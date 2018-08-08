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

        public static bool AddIfNoPointPresent(this List<Point2D> points, Point2D point, double searchRadius = 0.1)
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

        public static bool RectangleContains(Point point, Point position, double width, double height, double orientation)
        {
            Point a = position.Add(new Point(-width / 2, height / 2).Rotate(orientation));
            Point b = position.Add(new Point(width / 2, height / 2).Rotate(orientation));
            //Point c = position.Add(new Point(width / 2, -height / 2).Rotate(orientation));
            Point d = position.Add(new Point(-width / 2, -height / 2).Rotate(orientation));

            Point ap = point.Sub(a);
            Point ab = b.Sub(a);

            double apab = ap.Dot(ab);
            double abab = ab.Dot(ab);
            if (apab < 0 || apab >= abab) return false;

            Point ad = d.Sub(a);

            double apad = ap.Dot(ad);
            double adad = ad.Dot(ad);
            if (apad < 0 || apad >= adad) return false;

            return true;
        }

        public static Point Rotate(this Point point, double rotation)
        {
            double angle = rotation * Math.PI / 180;
            double x = point.X * Math.Cos(angle) - point.Y * Math.Sin(angle);
            double y = point.X * Math.Sin(angle) + point.Y * Math.Cos(angle);
            return new Point(x, y);
        }
    }
}
