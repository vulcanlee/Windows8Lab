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
    public sealed partial class 由左往右翻頁動畫Page : Page
    {
        int PageIndex = 0;
        string PageImageUrl = "ms-appx:///Assets/page_{0}.png";
        string PageLeftUrl = "";
        string PageRightUrl = "";
        public 由左往右翻頁動畫Page()
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
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void btnStart由左向右_Click(object sender, RoutedEventArgs e)
        {
            PageIndex -= 2;
            if (PageIndex <= -1)
            {
                PageIndex = 10;
            }


            img左邊手寫畫板翻頁動畫.Source = img左邊手寫畫板原始圖片.Source;

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd左邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp左邊手寫畫板翻頁動畫.CenterOfRotationX = 1.0;

            img左邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute));

            img右邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex  + 1), UriKind.RelativeOrAbsolute));

            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd右邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp右邊手寫畫板翻頁動畫.CenterOfRotationX = 0;

            Reset頁面動畫();

            gd左邊手寫畫板翻頁動畫PPRY_From.Value = 0;
            gd左邊手寫畫板翻頁動畫PPRY_To.Value = -69;
            gd右邊手寫畫板翻頁動畫PPRY_From.Value = 69;
            gd右邊手寫畫板翻頁動畫PPRY_To.Value = 0;

            TurnPage由左往右.Begin();
            await TurnPage由左往右1.BeginAsync();

            Reset頁面動畫();

            img右邊手寫畫板原始圖片.Source = img右邊手寫畫板翻頁動畫.Source;

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            //img左邊手寫畫板原始圖片
        }

        private async void btnStart由左向右1_Click(object sender, RoutedEventArgs e)
        {
            PageIndex -= 2;
            if (PageIndex <= -1)
            {
                PageIndex = 10;
            }

            //PageIndex += 2;
            //if (PageIndex >= 11)
            //{
            //    PageIndex = 0;
            //}


            img左邊手寫畫板翻頁動畫.Source = img左邊手寫畫板原始圖片.Source;
            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd左邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp左邊手寫畫板翻頁動畫.CenterOfRotationX = 1.0;
            img左邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex ), UriKind.RelativeOrAbsolute));

            Reset頁面動畫();

            gd左邊手寫畫板翻頁動畫PPRY_From.Value = 0;
            gd左邊手寫畫板翻頁動畫PPRY_To.Value = -69;

            await TurnPage由左往右.BeginAsync();
            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            img右邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex  + 1), UriKind.RelativeOrAbsolute));
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd右邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp右邊手寫畫板翻頁動畫.CenterOfRotationX = 0;

            gd右邊手寫畫板翻頁動畫PPRY_From.Value = 69;
            gd右邊手寫畫板翻頁動畫PPRY_To.Value = 0;

            await TurnPage由左往右1.BeginAsync();

            Reset頁面動畫();

            img右邊手寫畫板原始圖片.Source = img右邊手寫畫板翻頁動畫.Source;

            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }

        private async void btnStart由右向左_Click(object sender, RoutedEventArgs e)
        {
            PageIndex += 2;

            if (PageIndex >= 11)
            {
                PageIndex = 0;
            }
            //PageIndex -= 2;
            //if (PageIndex <= -1)
            //{
            //    PageIndex = 10;
            //}







            img右邊手寫畫板翻頁動畫.Source = img右邊手寫畫板原始圖片.Source;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd右邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            img右邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex + 1), UriKind.RelativeOrAbsolute));
            pp右邊手寫畫板翻頁動畫.CenterOfRotationX = 0;
            pp右邊手寫畫板翻頁動畫.RotationY = 0;

            gd右邊手寫畫板翻頁動畫PPRY_From.Value = 0;
            gd右邊手寫畫板翻頁動畫PPRY_To.Value = 69;


            Reset頁面動畫();

            //img右邊手寫畫板原始圖片.Source = img右邊手寫畫板翻頁動畫.Source;





            pp左邊手寫畫板翻頁動畫.CenterOfRotationX = 1.0;
            pp左邊手寫畫板翻頁動畫.RotationY = -69;
            img左邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute));
            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd左邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            //

            Reset頁面動畫();

            gd左邊手寫畫板翻頁動畫PPRY_From.Value = -69;
            gd左邊手寫畫板翻頁動畫PPRY_To.Value = 0;

             TurnPage由左往右1.Begin();
            await TurnPage由左往右.BeginAsync();

            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            img左邊手寫畫板原始圖片.Source = img左邊手寫畫板翻頁動畫.Source;
            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void btnStart由右向左1_Click(object sender, RoutedEventArgs e)
        {
            PageIndex += 2;

            if (PageIndex >= 11)
            {
                PageIndex = 0;
            }
            //PageIndex -= 2;
            //if (PageIndex <= -1)
            //{
            //    PageIndex = 10;
            //}





            img右邊手寫畫板翻頁動畫.Source = img右邊手寫畫板原始圖片.Source;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd右邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            img右邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex  + 1), UriKind.RelativeOrAbsolute));
            pp右邊手寫畫板翻頁動畫.CenterOfRotationX = 0;
            pp右邊手寫畫板翻頁動畫.RotationY =0;

            gd右邊手寫畫板翻頁動畫PPRY_From.Value = 0;
            gd右邊手寫畫板翻頁動畫PPRY_To.Value = 69;

            await TurnPage由左往右1.BeginAsync();

            Reset頁面動畫();

            //img右邊手寫畫板原始圖片.Source = img右邊手寫畫板翻頁動畫.Source;

            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;




            pp左邊手寫畫板翻頁動畫.CenterOfRotationX = 1.0;
            pp左邊手寫畫板翻頁動畫.RotationY = -69;
            img左邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute)); 
            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd左邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            //

            Reset頁面動畫();

            gd左邊手寫畫板翻頁動畫PPRY_From.Value = -69;
            gd左邊手寫畫板翻頁動畫PPRY_To.Value = 0;

            await TurnPage由左往右.BeginAsync();
            img左邊手寫畫板原始圖片.Source = img左邊手寫畫板翻頁動畫.Source;
            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

        }

        private async void btnStart由左向右單頁_Click(object sender, RoutedEventArgs e)
        {
            PageIndex -= 2;
            if (PageIndex <= -1)
            {
                PageIndex = 10;
            }


            img左邊手寫畫板翻頁動畫.Source = img左邊手寫畫板原始圖片.Source;

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd左邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp左邊手寫畫板翻頁動畫.CenterOfRotationX = 0;

            img左邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute));

            img右邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex + 1), UriKind.RelativeOrAbsolute));

            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp右邊手寫畫板翻頁動畫.CenterOfRotationX = 0;

            Reset頁面動畫();

            gd左邊手寫畫板翻頁動畫PPRY_From.Value = 0;
            gd左邊手寫畫板翻頁動畫PPRY_To.Value = 69;
            gd右邊手寫畫板翻頁動畫PPRY_From.Value = 69;
            gd右邊手寫畫板翻頁動畫PPRY_To.Value = 0;

            await TurnPage由左往右.BeginAsync();
            //await TurnPage由左往右1.BeginAsync();

            Reset頁面動畫();

            img右邊手寫畫板原始圖片.Source = img右邊手寫畫板翻頁動畫.Source;

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void btnStart由右向左單頁_Click(object sender, RoutedEventArgs e)
        {
            PageIndex += 2;

            if (PageIndex >= 11)
            {
                PageIndex = 0;
            }
            //PageIndex -= 2;
            //if (PageIndex <= -1)
            //{
            //    PageIndex = 10;
            //}







            img左邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute));

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Visible;
            gd左邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp左邊手寫畫板翻頁動畫.CenterOfRotationX = 0;

            //img左邊手寫畫板原始圖片.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex), UriKind.RelativeOrAbsolute));

            img右邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex + 1), UriKind.RelativeOrAbsolute));

            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板.Visibility = Windows.UI.Xaml.Visibility.Visible;
            pp右邊手寫畫板翻頁動畫.CenterOfRotationX = 0;

            Reset頁面動畫();

            gd左邊手寫畫板翻頁動畫PPRY_From.Value = 69;
            gd左邊手寫畫板翻頁動畫PPRY_To.Value = 0;
            gd右邊手寫畫板翻頁動畫PPRY_From.Value = 69;
            gd右邊手寫畫板翻頁動畫PPRY_To.Value = 0;

            await TurnPage由左往右.BeginAsync();
            //await TurnPage由左往右1.BeginAsync();

            Reset頁面動畫();

            img左邊手寫畫板原始圖片.Source = img左邊手寫畫板翻頁動畫.Source;
            img右邊手寫畫板原始圖片.Source = img右邊手寫畫板翻頁動畫.Source;

            gd左邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            gd右邊手寫畫板翻頁動畫.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void btn動畫時間_Click(object sender, RoutedEventArgs e)
        {
            //TimeSpan foots = TimeSpan.FromMilliseconds(Convert.ToDouble(tbkTime.Text) -
            //    gd左邊手寫畫板翻頁動畫PPRY_To.KeyTime.TimeSpan.Milliseconds);
            //gd左邊手寫畫板翻頁動畫PPRY_To.KeyTime = TimeSpan.FromMilliseconds(Convert.ToDouble(tbkTime.Text));
            gd左邊手寫畫板翻頁動畫PPRY_To.KeyTime = TimeSpan.FromMilliseconds(Convert.ToDouble(tbkTime.Text));
            gd右邊手寫畫板翻頁動畫PPRY_To.KeyTime = TimeSpan.FromMilliseconds(Convert.ToDouble(tbkTime.Text));
            //gd右邊手寫畫板翻頁動畫PPRY_To.KeyTime.TimeSpan.Add(
            //    TimeSpan.FromSeconds(Convert.ToDouble(tbkTime.Text) - 
            //    gd右邊手寫畫板翻頁動畫PPRY_To.KeyTime.TimeSpan.Milliseconds));
        }
    }
}
