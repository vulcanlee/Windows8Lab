using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI.Core;
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
    public sealed partial class MainPage : Page
    {
        public BackgroundDownloader downloader = new BackgroundDownloader();
        public DownloadOperation downloadOperation;
        public CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public Queue<DownloadOperation> downloadQueue = new Queue<DownloadOperation>();
        public int maxConcurrentDownloads = 4;
        public List<string> downloadUrl = new List<string>()
        {
            "http://download.thinkbroadband.com/10MB.zip",
            "http://download.thinkbroadband.com/20MB.zip",
            "http://download.thinkbroadband.com/40MB.zip",
        };
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnDownloadX_Click(object sender, RoutedEventArgs e)
        {
            Button fooButton = sender as Button;
            string fooTag = fooButton.Tag as string;
            int fooIdx = Convert.ToInt32(fooTag);
            string path = downloadUrl[fooIdx];
            string name = path.Substring(path.LastIndexOf('/') + 1);


            // C:\Users\vulcan\AppData\Local\Packages\ae0d48ab-81cc-4c00-a9f1-33dacf1e5d76_bwhtsc3gp9n2m\LocalState
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync(name, CreationCollisionOption.ReplaceExisting);
            DownloadOperation operation = downloader.CreateDownload(new Uri(path, UriKind.Absolute), file);
            Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>();
            progressCallback.ProgressChanged += progressCallback_ProgressChanged;

            operation.StartAsync().AsTask(cancellationToken.Token, progressCallback).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    #region Cancel

                    #endregion
                }
                else if (task.IsFaulted)
                {
                    #region Faulted

                    #endregion
                }
                else if (task.IsCompleted)
                {
                    #region Complete
                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
                    {
                        var foop = await task;
                        Debug.WriteLine("Main:{0}",foop.Progress.Status);

                        if (fooIdx == 0)
                        {
                            Progressbar1.Value = 0;
                        }
                        else if (fooIdx == 1)
                        {
                            Progressbar2.Value = 0;
                        }
                        else if (fooIdx == 2)
                        {
                            Progressbar3.Value = 0;
                        }
                    }); 
                    #endregion
                }
                else
                {
                    #region not Complete

                    #endregion
                }
            });
        }

        void progressCallback_ProgressChanged(object sender, DownloadOperation e)
        {
            try
            {
                double bytesRecieved = Convert.ToDouble(e.Progress.BytesReceived);
                double totalBytesToReceive = Convert.ToDouble(e.Progress.TotalBytesToReceive);
                double DownloadProgress = (bytesRecieved / totalBytesToReceive) * 100;

                int fooIdx = downloadUrl.IndexOf(e.RequestedUri.ToString());
                if (fooIdx == 0)
                {
                    Progressbar1.Value = DownloadProgress;
                }
                else if (fooIdx == 1)
                {
                    Progressbar2.Value = DownloadProgress;
                }
                else if (fooIdx == 2)
                {
                    Progressbar3.Value = DownloadProgress;
                }
                if (DownloadProgress == 100)
                {
                    //IsDownloadInProgress = Windows.UI.Xaml.Visibility.Collapsed;
                    //Button1.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async void btnPauseX_Click(object sender, RoutedEventArgs e)
        {
            Button fooButton = sender as Button;
            string fooTag = fooButton.Tag as string;
            int fooIdx = Convert.ToInt32(fooTag);
            string path = downloadUrl[fooIdx];
            string name = path.Substring(path.LastIndexOf('/') + 1);

            IReadOnlyList<DownloadOperation> foodownloader = await BackgroundDownloader.GetCurrentDownloadsAsync();
            var fooIt = foodownloader.FirstOrDefault(x => x.RequestedUri.ToString() == path);
            if (fooIt != null)
            {
                fooIt.Pause();
            }
        }

        private async void btnResumeX_Click(object sender, RoutedEventArgs e)
        {
            Button fooButton = sender as Button;
            string fooTag = fooButton.Tag as string;
            int fooIdx = Convert.ToInt32(fooTag);
            string path = downloadUrl[fooIdx];
            string name = path.Substring(path.LastIndexOf('/') + 1);

            IReadOnlyList<DownloadOperation> foodownloader = await BackgroundDownloader.GetCurrentDownloadsAsync();
            var fooIt = foodownloader.FirstOrDefault(x => x.RequestedUri.ToString() == path);
            if (fooIt != null)
            {
                fooIt.Resume();
            }
        }

        private async void btnAllRefresh_Click(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<DownloadOperation> foodownloader = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (var item in foodownloader)
            {
                int fooIdx = downloadUrl.IndexOf(item.RequestedUri.ToString());

                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>();
                progressCallback.ProgressChanged += progressCallback_ProgressChanged;

                item.AttachAsync().AsTask(cancellationToken.Token, progressCallback).ContinueWith(task =>
                {
                    if (task.IsCanceled)
                    {
                        #region Cancel

                        #endregion
                    }
                    else if (task.IsFaulted)
                    {
                        #region Faulted

                        #endregion
                    }
                    else if (task.IsCompleted)
                    {
                        #region Complete
                        Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            if (fooIdx == 0)
                            {
                                Progressbar1.Value = 0;
                            }
                            else if (fooIdx == 1)
                            {
                                Progressbar2.Value = 0;
                            }
                            else if (fooIdx == 2)
                            {
                                Progressbar3.Value = 0;
                            }
                        });
                        #endregion
                    }
                    else
                    {
                        #region not Complete

                        #endregion
                    }
                });
            }
        }

        private async void btnAllRefreshAttach_Click(object sender, RoutedEventArgs e)
        {
            IReadOnlyList<DownloadOperation> foodownloader = await BackgroundDownloader.GetCurrentDownloadsAsync();
            foreach (var item in foodownloader)

            {
                if (item.Progress.Status == BackgroundTransferStatus.PausedByApplication)
                {
                    //item.Resume();
                }
                Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>();
                progressCallback.ProgressChanged += progressCallback_ProgressChanged;

                item.AttachAsync().AsTask(cancellationToken.Token, progressCallback).ContinueWith(OnUploadCompleted, item);
            }
        }
        private async void OnUploadCompleted(Task<DownloadOperation> task, object arg)
        {
            //var fook = await task;
            //Debug.WriteLine(fook.Progress.Status);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
