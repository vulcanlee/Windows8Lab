﻿using System;
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

namespace PageFlip
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void btn由上往下翻頁動畫_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(由上往下翻頁動畫Page), "");

        }

        private void btn由左上往右下翻頁動畫_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(由左上往右下翻頁動畫Page), "");

        }

        private void btn由左往右翻頁動畫Page_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(由左往右翻頁動畫Page), "");

        }

        private void btn遮罩翻頁動畫Page_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(遮罩動畫Page), "");

        }
    }
}
