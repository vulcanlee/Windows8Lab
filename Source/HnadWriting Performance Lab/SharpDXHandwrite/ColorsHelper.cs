using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace SharpDXHandwrite
{
    public class ColorsHelper
    {
        public static Color Parse(string color)
        {
            var offset = color.StartsWith("#") ? 1 : 0;


            var a = Byte.Parse(color.Substring(0 + offset, 2), NumberStyles.HexNumber);
            var r = Byte.Parse(color.Substring(2 + offset, 2), NumberStyles.HexNumber);
            var g = Byte.Parse(color.Substring(4 + offset, 2), NumberStyles.HexNumber);
            var b = Byte.Parse(color.Substring(6 + offset, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
