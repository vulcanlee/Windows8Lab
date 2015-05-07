using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.AwaitableUI;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace PageFlip
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class 遮罩動畫Page : Page
    {
        int PageIndex = 0;
        string PageImageUrl = "ms-appx:///Assets/page_{0}.png";
        string PageLeftUrl = "";
        string PageRightUrl = "";
        public 遮罩動畫Page()
        {
            this.InitializeComponent();
            img左邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, 0), UriKind.RelativeOrAbsolute));
            img右邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, 0 + 1), UriKind.RelativeOrAbsolute));
            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
        }

        public void Reset頁面動畫()
        {
            //LineSegment LineSegment = new LineSegment();
            //LineSegment.Point = new Point();
            //LineSegment.Point.
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void btnStart由左向右翻頁_Click(object sender, RoutedEventArgs e)
        {
            PageIndex += 2;
            if (PageIndex >= 11)
            {
                PageIndex = 0;
            }

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;





            ibsh左邊手寫畫板翻頁動畫.ImageSource = img左邊手寫畫板原始圖片.Source;
            ibsh右邊手寫畫板翻頁動畫.ImageSource = img右邊手寫畫板原始圖片.Source;
            img左邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute));
            img右邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex + 1), UriKind.RelativeOrAbsolute));
            await Task.Delay(30);


            ls左邊Point1.Point = new Point(0, 0);
            ls左邊Point2.Point = new Point(0, 0);
            ls左邊Point3.Point = new Point(0, 1024);
            ls左邊Point10.Point = new Point(0, 0);
            ls左邊Point20.Point = new Point(0, 0);
            ls左邊Point30.Point = new Point(0, 1024);
            gd左邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ls右邊Point1.Point = new Point(0, 0);
            ls右邊Point2.Point = new Point(0, 0);
            ls右邊Point3.Point = new Point(0, 1024);
            ls右邊Point10.Point = new Point(0, 0);
            ls右邊Point20.Point = new Point(0, 0);
            ls右邊Point30.Point = new Point(0, 1024);
            gd右邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;

            var foosbd1 = Storyboard左頁斜角翻頁.BeginAsync();
            var foosbd2 = Storyboard左頁背景斜角翻頁.BeginAsync();
            var foosbd3 = Storyboard右頁斜角翻頁.BeginAsync();
            var foosbd4 = Storyboard右頁背景斜角翻頁.BeginAsync();
            await Task.WhenAll(foosbd1, foosbd2, foosbd3, foosbd4);

            gd左邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ls左邊Point1.Point = new Point(0, 0);
            ls左邊Point2.Point = new Point(0, 0);
            ls左邊Point3.Point = new Point(0, 1024);
            ls左邊Point10.Point = new Point(0, 0);
            ls左邊Point20.Point = new Point(0, 0);
            ls左邊Point30.Point = new Point(0, 1024);
            ls右邊Point1.Point = new Point(0, 0);
            ls右邊Point2.Point = new Point(0, 0);
            ls右邊Point3.Point = new Point(0, 1024);
            ls右邊Point10.Point = new Point(0, 0);
            ls右邊Point20.Point = new Point(0, 0);
            ls右邊Point30.Point = new Point(0, 1024);
        }

        private async void btnStart由右向左翻頁_Click(object sender, RoutedEventArgs e)
        {
            PageIndex -= 2;
            if (PageIndex <= -1)
            {
                PageIndex = 10;
            }

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;




            //imgTemp右.Source = null;
            //imgTemp右.Source = img左邊手寫畫板原始圖片.Source;
            //imgTemp右.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ls左邊Point1.Point = new Point(768, 1024);
            ls左邊Point2.Point = new Point(768, 1024);
            ls左邊Point3.Point = new Point(768, 1024);
            ls左邊Point10.Point = new Point(768, 1024);
            ls左邊Point20.Point = new Point(768, 1024);
            ls左邊Point30.Point = new Point(768, 1024);
            gd左邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ls右邊Point1.Point = new Point(768, 1024);
            ls右邊Point2.Point = new Point(768, 1024);
            ls右邊Point3.Point = new Point(768, 1024);
            ls右邊Point10.Point = new Point(768, 1024);
            ls右邊Point20.Point = new Point(768, 1024);
            ls右邊Point30.Point = new Point(768, 1024);
            gd右邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            //mypath右邊背景.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //mypath右邊.Visibility = Windows.UI.Xaml.Visibility.Collapsed;





            ibsh左邊手寫畫板翻頁動畫.ImageSource = img左邊手寫畫板原始圖片.Source;
            ibsh右邊手寫畫板翻頁動畫.ImageSource = img右邊手寫畫板原始圖片.Source;
            await Task.Delay(50);
            //img左邊手寫畫板原始圖片.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //img右邊手寫畫板原始圖片.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            img左邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute));
            img右邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex + 1), UriKind.RelativeOrAbsolute));


            ls左邊Point1.Point = new Point(0, 0);
            ls左邊Point2.Point = new Point(0, 0);
            ls左邊Point3.Point = new Point(0, 1024);
            ls左邊Point10.Point = new Point(0, 0);
            ls左邊Point20.Point = new Point(0, 0);
            ls左邊Point30.Point = new Point(0, 1024);
            gd左邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            ls右邊Point1.Point = new Point(0, 0);
            ls右邊Point2.Point = new Point(0, 0);
            ls右邊Point3.Point = new Point(0, 1024);
            ls右邊Point10.Point = new Point(0, 0);
            ls右邊Point20.Point = new Point(0, 0);
            ls右邊Point30.Point = new Point(0, 1024);
            gd右邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;

            //imgTemp右.Source = null;
            //imgTemp右.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //await Task.Delay(500);
            gd右邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            img左邊手寫畫板原始圖片.Visibility = Windows.UI.Xaml.Visibility.Visible;
            img右邊手寫畫板原始圖片.Visibility = Windows.UI.Xaml.Visibility.Visible;
            var foosbd1 = Storyboard左頁斜角翻頁.BeginAsync();
            var foosbd2 = Storyboard左頁背景斜角翻頁.BeginAsync();
            var foosbd3 = Storyboard右頁斜角翻頁.BeginAsync();
            var foosbd4 = Storyboard右頁背景斜角翻頁.BeginAsync();
            await Task.WhenAll(foosbd1, foosbd2, foosbd3, foosbd4);

            gd左邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊斜角動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ls左邊Point1.Point = new Point(768, 1024);
            ls左邊Point2.Point = new Point(768, 1024);
            ls左邊Point3.Point = new Point(768, 1024);
            ls左邊Point10.Point = new Point(768, 1024);
            ls左邊Point20.Point = new Point(768, 1024);
            ls左邊Point30.Point = new Point(768, 1024);
            ls右邊Point1.Point = new Point(768, 1024);
            ls右邊Point2.Point = new Point(768, 1024);
            ls右邊Point3.Point = new Point(768, 1024);
            ls右邊Point10.Point = new Point(768, 1024);
            ls右邊Point20.Point = new Point(768, 1024);
            ls右邊Point30.Point = new Point(768, 1024);
        }
    }
}
