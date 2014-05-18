using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.AwaitableUI;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaElementPlayRate
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool playing = false;
        public MainPage()
        {
            this.InitializeComponent();
            //me1.Source = new Uri(string.Format("ms-appx:///Assets/Music0.wav"), UriKind.RelativeOrAbsolute);

        }

        private void btnSetPlaybackRate_Click(object sender, RoutedEventArgs e)
        {
            double dd = Convert.ToDouble(tbPlayrate.Text);
            me1.DefaultPlaybackRate = dd;
            me1.PlaybackRate = dd;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            me1.Play();
            playing = true;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            me1.Stop();
            playing = false;
        }

        async private void btnPlaySlowDown_Click(object sender, RoutedEventArgs e)
        {
            mainLayout.Background = new SolidColorBrush(Colors.Red);
            if (playing == true)
            {
                me1.Stop();
            await me1.WaitToCompleteAsync();
            }
            me1.DefaultPlaybackRate = 0.9;
            me1.PlaybackRate = 0.9;
            tbPlayrate.Text = "0.9";
            me1.Play();
            playing = true; 
            mainLayout.Background = new SolidColorBrush(Colors.Black);
        }

        async private void btnPlaySpeedUp_Click(object sender, RoutedEventArgs e)
        {
            mainLayout.Background = new SolidColorBrush(Colors.Red);
            if (playing == true)
            {
                me1.Stop();
                await me1.WaitToCompleteAsync();
            }
            me1.DefaultPlaybackRate = 1.05;
            me1.PlaybackRate = 1.05;
            tbPlayrate.Text = "1.05";
            me1.Play();
            playing = true;
            mainLayout.Background = new SolidColorBrush(Colors.Black);
        }

        async private void btnPlayNormal_Click(object sender, RoutedEventArgs e)
        {
            mainLayout.Background = new SolidColorBrush(Colors.Red);
            if (playing == true)
            {
                me1.Stop();
                await me1.WaitToCompleteAsync();
            }
            me1.DefaultPlaybackRate = 1.00;
            me1.PlaybackRate = 1.0;
            tbPlayrate.Text = "1.0";
            me1.Play();
            playing = true;
            mainLayout.Background = new SolidColorBrush(Colors.Black);
        }

        async private void btnChangeToMp3_Click(object sender, RoutedEventArgs e)
        {
            mainLayout.Background = new SolidColorBrush(Colors.Red);
            if (playing == true)
            {
                me1.Stop();
                await me1.WaitToCompleteAsync();
                playing = false; 
            }
            me1.Source = new Uri(string.Format("ms-appx:///Assets/Music0.mp3"), UriKind.RelativeOrAbsolute);
            mainLayout.Background = new SolidColorBrush(Colors.Black);
        }

        async private void btnChangeToWAV_Click(object sender, RoutedEventArgs e)
        {
            mainLayout.Background = new SolidColorBrush(Colors.Red);
            if (playing == true)
            {
                me1.Stop();
                await me1.WaitToCompleteAsync();
                playing = false;
            }
            me1.Source = new Uri(string.Format("ms-appx:///Assets/Music0.wav"), UriKind.RelativeOrAbsolute);
            mainLayout.Background = new SolidColorBrush(Colors.Black);
        }
    }
}
