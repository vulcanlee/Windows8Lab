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
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.AwaitableUI;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace 動畫效果模擬
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        int cc = 0;
        VisualStateManager VisualStateManager = new VisualStateManager();
        public MainPage()
        {
            this.InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
        }

        private void btn發動動畫_Click(object sender, RoutedEventArgs e)
        {
            Button fooBtn = sender as Button;
            string fooAnimationName = fooBtn.Content as string;
            //VisualStateManager.GoToState(this, "各種不同狀態下的動畫模擬_預設", true);


            cc = 0;
            timer.Tick -= timer_Ticker;
            timer.Tick += timer_Ticker;
            timer.Start();

            VisualStateManager.GoToState(this, fooAnimationName, true);
        }

        private async void btn發動動畫Test_Click(object sender, RoutedEventArgs e)
        {
            //sb由左往右翻頁_右支點.Begin();
            //Storyboard1.Begin();
            planeProjection.CenterOfRotationX = 1;
            planeProjection.CenterOfRotationY = 0.5;
            planeProjection.CenterOfRotationY = 0;
            planeProjection.RotationX = 0;

            cc = 0;
            timer.Tick -= timer_Ticker;
            timer.Tick += timer_Ticker;
            timer.Start();

            await sb由左往右翻頁_右支點.BeginAsync();
        }

        private void btn發動動畫P1_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BasicPage1), "");
        }

        private void timer_Ticker(object sender, object e)
        {
            if (cc > 2)
            {
                timer.Stop();
                timer.Tick -= timer_Ticker;
            }
            //tbkplaneProjection.Text = planeProjection.RotationY.ToString();
            tbk這是動畫測試對象.Text = "這是動畫測試對象";
            cc++;
        }
    }
}
