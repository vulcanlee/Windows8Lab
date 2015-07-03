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

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace PlaneProjectionProblem
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        int cc = 0;
        public MainPage()
        {
            this.InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
        }

        private void btnTurnRight_Click(object sender, RoutedEventArgs e)
        {
            cc = 0;
            timer.Tick -= timer_Ticker;
            timer.Tick += timer_Ticker;
            timer.Start();
            StoryboardTurnRight.Begin();
        }

        private void btnTurnLeft_Click(object sender, RoutedEventArgs e)
        {
            cc = 0;
            timer.Tick -= timer_Ticker;
            timer.Tick += timer_Ticker;
            timer.Start();
            StoryboardTurnLeft.Begin();
        }

        private void timer_Ticker(object sender, object e)
        {
            if (cc > 2)
            {
                timer.Stop();
                timer.Tick -= timer_Ticker;
            }
            //tbkplaneProjection.Text = planeProjection.RotationY.ToString();
            tbkplaneProjection.Text = "";
            cc++;
        }
    }
}
