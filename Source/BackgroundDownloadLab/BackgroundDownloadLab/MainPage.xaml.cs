using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace BackgroundDownloadLab
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<DownloadOperation> _activeDownload = new List<DownloadOperation>();
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

        private async void DownloadClick(object sender, RoutedEventArgs e)
        {
            const string fileLocation
             = "http://www.vulcanlab.net/download/Release.rar";
            var uri = new Uri(fileLocation);
            var downloader = new BackgroundDownloader();
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("100MB.zip",
                CreationCollisionOption.ReplaceExisting);
            DownloadOperation download = downloader.CreateDownload(uri, file);
            await StartDownloadAsync(download);
        }

        private void ProgressCallback(DownloadOperation obj)
        {
            double progress
                = ((double)obj.Progress.BytesReceived / obj.Progress.TotalBytesToReceive);
            DownloadProgress.Value = progress * 100;
            if (progress >= 1.0)
            {
                _activeDownload = null;
                DownloadButton.IsEnabled = true;
            }
        }

        private async Task StartDownloadAsync(DownloadOperation downloadOperation)
        {
            try
            {
                DownloadButton.IsEnabled = false;
                _activeDownload.Add(downloadOperation);
                var progress = new Progress<DownloadOperation>(ProgressCallback);
                await downloadOperation.StartAsync().AsTask(progress);
                //await downloadOperation.StartAsync();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
