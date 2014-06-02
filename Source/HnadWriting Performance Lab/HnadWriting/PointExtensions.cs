using System;
using System.Windows;
using Windows.Foundation;

namespace SharpGeometry{
    public static class PointExtensions{
        public static float DistanceToPoint(this Point point, Point point2){
            return
                (float)
                Math.Sqrt((point2.X - point.X)*(point2.X - point.X) + (point2.Y - point.Y)*(point2.Y - point.Y));
        }

        public static bool IsBetweenTwoPoints(this Point targetPoint, Point point1, Point point2){
            double minX = Math.Min(point1.X, point2.X);
            double minY = Math.Min(point1.Y, point2.Y);
            double maxX = Math.Max(point1.X, point2.X);
            double maxY = Math.Max(point1.Y, point2.Y);

            double targetX = targetPoint.X;
            double targetY = targetPoint.Y;

            return LessOrEqual(minX, targetX)
                   && LessOrEqual(targetX, maxX)
                   && LessOrEqual(minY, targetY)
                   && LessOrEqual(targetY, maxY);
        }

        private static bool LessOrEqual(double left, double right){
            return left <= right || (1 <= left / right && left / right < 1.00000000001);
        }
    }
}