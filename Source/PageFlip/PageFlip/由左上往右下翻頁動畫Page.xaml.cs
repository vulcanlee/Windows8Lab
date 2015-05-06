using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class 由左上往右下翻頁動畫Page : Page
    {
        int PageIndex = 0;
        string PageImageUrl = "ms-appx:///Assets/page_{0}.png";
        string PageLeftUrl = "";
        string PageRightUrl = "";
        public 由左上往右下翻頁動畫Page()
        {
            this.InitializeComponent();
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (PageIndex >= 10)
            {
                PageIndex = -2;
            }
            img左邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex + 2), UriKind.RelativeOrAbsolute));
            img右邊手寫畫板翻頁動畫.Source = new BitmapImage(new Uri(string.Format(PageImageUrl, PageIndex + 2+1), UriKind.RelativeOrAbsolute));
            Reset頁面動畫();

            await Storyboard由上往下.BeginAsync();

            Reset頁面動畫();

            img左邊手寫畫板原始圖片.Source = img左邊手寫畫板翻頁動畫.Source;
            img右邊手寫畫板原始圖片.Source = img右邊手寫畫板翻頁動畫.Source;
            PageIndex += 2;
            //img左邊手寫畫板原始圖片
        }

        public void Reset頁面動畫()
        {
            img左邊手寫畫板翻頁動畫.Height = 0;
            img左邊手寫畫板翻頁動畫.Width = 0;
            img左邊手寫畫板翻頁動畫背景.Height = 0;
            img左邊手寫畫板翻頁動畫背景.Width = 0;
            img右邊手寫畫板翻頁動畫.Height = 0;
            img右邊手寫畫板翻頁動畫.Width = 0;
            img右邊手寫畫板翻頁動畫背景.Height = 0;
            img右邊手寫畫板翻頁動畫背景.Width = 0;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
