using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace BackgroundDownloaderTest
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            IReadOnlyList<DownloadOperation> foodownloader = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (var item in foodownloader)
            {
                if (item.Progress.Status == BackgroundTransferStatus.PausedByApplication)
                {
                    item.Resume();
                }
                //item.AttachAsync();
                item.AttachAsync().AsTask().ContinueWith(OnUploadCompleted, item);
            }
        }
        private async void OnUploadCompleted(Task<DownloadOperation> task, object arg)
        {
            var fook = await task;
            Debug.WriteLine(fook.Progress.Status);
            // Upload is complete at this point.
            // Check task.Status to see if the upload was successful or not.
        }

    }
}
