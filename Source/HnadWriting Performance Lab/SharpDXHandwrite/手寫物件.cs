using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace SharpDXHandwrite
{
    public class 單曲手寫內容
    {
        public 頁面手寫物件軌跡 Items { get; set; }
    }

    /// <summary>
    /// 特定頁面的手寫物件定義
    /// </summary>
    public class 頁面手寫物件軌跡
    {
        public List<手寫物件軌跡> 手寫物件軌跡s { get; set; }
        /// <summary>
        /// Store產品所有相關檔案GetProductFiles內所標示的樂器ID
        /// </summary>
        public string InsID { get; set; }
        /// <summary>
        /// Store產品所有相關檔案GetProductFiles內的ScoreMapping欄位上所表示的索引值(樂器和分譜圖)
        /// </summary>
        public int ScoreMappingIndex { get; set; }
        /// <summary>
        /// 手寫內容的樂譜頁面
        /// </summary>
        public int PageNo { get; set; }

        public 頁面手寫物件軌跡()
        {
            手寫物件軌跡s = new List<手寫物件軌跡>();
            InsID = "";
            ScoreMappingIndex = -1;
        }
    }

    /// <summary>
    /// 一次手寫的所有物件定義
    /// </summary>
    public class 手寫物件軌跡
    {
        public List<手寫物件> 手寫物件s { get; set; }
        public double StrokeThickness { get; set; }
        public 手寫物件調色盤 手寫物件調色盤 { get; set; }
        public int 寫入順序編號 { get; set; }
        public 手或筆與橡皮擦 手或筆與橡皮擦 { get; set; }

        public 手寫物件軌跡()
        {
            手寫物件s = new List<手寫物件>();
            手或筆與橡皮擦 = 手或筆與橡皮擦.筆;
        }

        public SolidColorBrush 橡皮擦的SolidColorBrush()
        {
            SolidColorBrush solidColorBrush = null;
            solidColorBrush = new SolidColorBrush(ColorsHelper.Parse("#88FFFFFF"));
            return solidColorBrush;
        }

        public Color 取得手寫物件調色盤的實際Color()
        {
            Color solidColorBrush = Colors.Black;

            switch (this.手寫物件調色盤)
            {
                case 手寫物件調色盤.img_color_black:
                    solidColorBrush = ColorsHelper.Parse("ff1a1a1a");
                    break;
                case 手寫物件調色盤.img_color_gray:
                    solidColorBrush = ColorsHelper.Parse("ff999999");
                    break;
                case 手寫物件調色盤.img_color_r:
                    solidColorBrush = ColorsHelper.Parse("ffff0000");
                    break;
                case 手寫物件調色盤.img_color_b:
                    solidColorBrush = ColorsHelper.Parse("ff006cff");
                    break;
                case 手寫物件調色盤.img_color_g:
                    solidColorBrush = ColorsHelper.Parse("ff0da522");
                    break;
                case 手寫物件調色盤.img_opacity_b:
                    solidColorBrush = ColorsHelper.Parse("6627e8ff");
                    break;
                case 手寫物件調色盤.img_opacity_g:
                    solidColorBrush = ColorsHelper.Parse("66999999");
                    break;
                case 手寫物件調色盤.img_opacity_r:
                    solidColorBrush = ColorsHelper.Parse("66ff0000");
                    break;
                case 手寫物件調色盤.img_opacity_o:
                    solidColorBrush = ColorsHelper.Parse("66ffa200");
                    break;
                case 手寫物件調色盤.img_opacity_y:
                    solidColorBrush = ColorsHelper.Parse("66ffea00");
                    break;
                default:
                    break;
            }
            return solidColorBrush;
        }
    }

    /// <summary>
    /// 手寫物件的每個小段落的座標描述
    /// </summary>
    public class 手寫物件
    {
        //X1 = 0,
        //Y1 = 0,
        //X2 = 100,
        //Y2 = 100,
        //StrokeThickness = 5,
        //Stroke = new SolidColorBrush(Colors.Red)
        public double X1 { get; set; }
        public double Y1 { get; set; }
        public double X2 { get; set; }
        public double Y2 { get; set; }
    }

    public enum 手寫物件調色盤
    {
        img_color_black,
        img_color_gray,
        img_color_r,
        img_color_b,
        img_color_g,
        img_opacity_b,
        img_opacity_g,
        img_opacity_r,
        img_opacity_o,
        img_opacity_y
    }

    public enum 手或筆與橡皮擦
    {
        筆,
        橡皮擦,
        手
    }

    public class 使用中的繪圖工具
    {
        public 手或筆與橡皮擦 手或筆與橡皮擦 { get; set; }
        public int 線條粗細 { get; set; }
        public 手寫物件調色盤 手寫物件調色盤 { get; set; }

        public 使用中的繪圖工具()
        {
            this.手或筆與橡皮擦 = 手或筆與橡皮擦.手;
            this.線條粗細 = 1;
            this.手寫物件調色盤 = 手寫物件調色盤.img_color_r;
        }
    }
}
