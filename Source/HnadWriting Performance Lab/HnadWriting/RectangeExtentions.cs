using System.Collections.Generic;
using System.Windows;
using Windows.Foundation;

namespace SharpGeometry{
    internal static class RectangeExtentions{
        public static IEnumerable<LineEquation> GetLinesForRectangle(this Rect rectangle){
            var lines = new List<LineEquation>{
                new LineEquation(new Point(rectangle.X, rectangle.Y),
                                 new Point(rectangle.X, rectangle.Y + rectangle.Height)),
                new LineEquation(new Point(rectangle.X, rectangle.Y + rectangle.Height),
                                 new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height)),
                new LineEquation(new Point(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height),
                                 new Point(rectangle.X + rectangle.Width, rectangle.Y)),
                new LineEquation(new Point(rectangle.X + rectangle.Width, rectangle.Y),
                                 new Point(rectangle.X, rectangle.Y)),
            };
            return lines;
        }
    }
}