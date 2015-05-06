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

namespace ScrollViewer同步
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        bool fooIsSync = false;
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            //if (fooIsSync == false)
            //{
            //    fooIsSync = true;
            //    //scrollViewerRight.ScrollToVerticalOffset((sender as ScrollViewer).VerticalOffset);
            //    //sv2.ScrollToHorizontalOffset(sv1.HorizontalOffset);
            //    sv2.ChangeView(sv1.HorizontalOffset, null, null);
            //}
        }

        private void sv2_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            if (fooIsSync == false)
            {
                fooIsSync = true;
                sv1.ChangeView(sv2.HorizontalOffset, null, null);
            }
        }

        private void sv2_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            fooIsSync = false;
        }

        private void sv1_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            fooIsSync = false;
        }
    }
}
