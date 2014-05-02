using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace UniqueDeviceId
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

        /// <summary>
        /// 在此頁面即將顯示在框架中時叫用。
        /// </summary>
        /// <param name="e">描述如何到達此頁面的事件資料。Parameter
        /// 屬性通常用來設定頁面。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.show.Text = GetHardwareId();
        }

        public static string GetHardwareId()
        {
            var _Token = Windows.System.Profile.HardwareIdentification.GetPackageSpecificToken(null);
            var _Id = _Token.Id;
            var _Reader = Windows.Storage.Streams.DataReader.FromBuffer(_Id);
            var _Bytes = new byte[_Id.Length];
            _Reader.ReadBytes(_Bytes);

            string ss = BitConverter.ToString(_Bytes).Replace("-", "");
            return ss;
        }
    }
}
