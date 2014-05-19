using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// C:\Users\%UserProfile%\AppData\Local\Packages\df298231-a68d-495c-8a28-0d4b5909ccf4_bwhtsc3gp9n2m\LocalState

namespace MediaCaptureTest
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaCapture m_mediaCaptureMgr = null;
        private IRandomAccessStream AudioStream;
        private VideoEncodingQuality SelectedQuality;
        MediaEncodingProfile encodingProfile = null;
        string 錄影與照片檔案名稱 = "";
        bool 發生異常需要停止Log = false;

        public MainPage()
        {
            this.InitializeComponent();
            選擇視訊編碼格式.SelectedIndex = 0;
            選擇錄影錄音測試模式.SelectedIndex = 0;
            選擇PhotoCaptureSource.SelectedIndex = 1;
            錄音錄影啟動暫停時間秒數.Text = "1.5";
        }

        #region 錄影之相關方法
        private async Task 攝影機初始化()
        {
            try
            {
                previewElement1.FlowDirection = Windows.UI.Xaml.FlowDirection.LeftToRight;

                if (m_mediaCaptureMgr != null)
                {
                    await 停止進行錄影預覽();
                    await 停止進行錄影攝影();
                }
                發生異常需要停止Log = false;
                m_mediaCaptureMgr = new Windows.Media.Capture.MediaCapture();

                var captureInitSettings = new MediaCaptureInitializationSettings();
                captureInitSettings.StreamingCaptureMode = StreamingCaptureMode.AudioAndVideo;
                string xu = 選擇PhotoCaptureSource.SelectedItem as string;
                switch (xu)
                {
                    case "Auto":
                        captureInitSettings.PhotoCaptureSource = PhotoCaptureSource.Auto;
                        break;
                    case "VideoPreview":
                        captureInitSettings.PhotoCaptureSource = PhotoCaptureSource.VideoPreview;
                        break;
                    case "Photo":
                        captureInitSettings.PhotoCaptureSource = PhotoCaptureSource.Photo;
                        break;
                }

                await m_mediaCaptureMgr.InitializeAsync(captureInitSettings);

                m_mediaCaptureMgr.Failed += new Windows.Media.Capture.MediaCaptureFailedEventHandler(Failed); ;

                previewElement1.Source = m_mediaCaptureMgr;
                await 開始進行錄影預覽();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                MainHelper.ShowToast("產生錯誤", "無法啟動錄音或錄影裝置");
                發生異常需要停止Log = true;
            }
        }

        void 音訊或視訊檔案的編碼設定()
        {
            encodingProfile = null;

            string xs = 選擇視訊編碼格式.SelectedItem as string;
            switch (xs)
            {
                case "Auto":
                    SelectedQuality = VideoEncodingQuality.Auto;
                    break;
                case "HD1080p":
                    SelectedQuality = VideoEncodingQuality.HD1080p;
                    break;
                case "HD720p":
                    //SelectedQuality = VideoEncodingQuality.HD720p;
                    break;
                case "Wvga":
                    SelectedQuality = VideoEncodingQuality.Wvga;
                    break;
                case "Ntsc":
                    SelectedQuality = VideoEncodingQuality.Ntsc;
                    break;
                case "Pal":
                    SelectedQuality = VideoEncodingQuality.Pal;
                    break;
                case "Vga":
                    SelectedQuality = VideoEncodingQuality.Vga;
                    break;
                case "Qvga":
                    SelectedQuality = VideoEncodingQuality.Qvga;
                    break;
            }
            encodingProfile = MediaEncodingProfile.CreateMp4(SelectedQuality);

            AudioStream = new InMemoryRandomAccessStream();
        }

        #region 預覽相關

        async Task 開始進行錄影預覽()
        {
            try
            {
                await m_mediaCaptureMgr.StartPreviewAsync();
            }
            catch (Exception ex)
            {
                發生異常需要停止Log = true;
                MainHelper.ShowToast("開始預覽發生異常", ex.Message);
            }
        }

        async Task 停止進行錄影預覽()
        {
            try
            {
                await m_mediaCaptureMgr.StopPreviewAsync();
            }
            catch (Exception ex)
            {
                發生異常需要停止Log = true;
                MainHelper.ShowToast("停止錄影發生異常", ex.Message);
            }
        }

        async Task 照相()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var mediaFile = await folder.CreateFileAsync(錄影與照片檔案名稱 + ".jpg", CreationCollisionOption.ReplaceExisting);
            var photoProperties = Windows.Media.MediaProperties.ImageEncodingProperties.CreateJpeg();
            ImageEncodingProperties imageProperties = Windows.Media.MediaProperties.ImageEncodingProperties.CreateJpeg();
            if (照相照片寬度.Text.Trim() == "" || 照相照片寬度.Text.Trim() == "0" ||
                照相照片高度.Text.Trim() == "" || 照相照片高度.Text.Trim() == "0")
            {

            }
            else
            {
                imageProperties.Width = Convert.ToUInt32(照相照片寬度.Text);
                imageProperties.Height = Convert.ToUInt32(照相照片高度.Text); ;
            }
            try
            {
                await m_mediaCaptureMgr.CapturePhotoToStorageFileAsync(imageProperties, mediaFile);
            }
            catch (Exception ex)
            {
                發生異常需要停止Log = true;
                MainHelper.ShowToast("開始照相發生異常", ex.Message);
            }
        }
        #endregion

        #region 錄影相關

        async Task 開始進行錄影攝影()
        {
            try
            {
                音訊或視訊檔案的編碼設定();
                var folder = ApplicationData.Current.LocalFolder;
                var mediaFile = await folder.CreateFileAsync(錄影與照片檔案名稱 + ".mp4", CreationCollisionOption.ReplaceExisting);
                await m_mediaCaptureMgr.StartRecordToStorageFileAsync(encodingProfile, mediaFile);
                //await m_mediaCaptureMgr.StartRecordToStreamAsync(encodingProfile, AudioStream);

                檔案路徑.Text = folder.Path;
            }
            catch (Exception ex)
            {
                發生異常需要停止Log = true;
                MainHelper.ShowToast("開始錄影攝影發生異常", ex.Message);
            }
        }

        async Task 停止進行錄影攝影()
        {
            try
            {
                await m_mediaCaptureMgr.StopRecordAsync();
            }
            catch (Exception ex)
            {
                發生異常需要停止Log = true;
                MainHelper.ShowToast("停止錄影攝影發生異常", ex.Message);
            }
        }

        #region 不同錄影測試模式
        async Task 模式1()
        {
            double sleep = Convert.ToDouble(錄音錄影啟動暫停時間秒數.Text);

            紀錄事件("");
            發生異常需要停止Log = false;
            紀錄事件("攝影機初始化");
            await 攝影機初始化();
            產生錄影檔案名稱();

            await Task.Delay(TimeSpan.FromSeconds(sleep));
            紀錄事件("開始照相");
            await 照相();

            await Task.Delay(TimeSpan.FromSeconds(sleep));
            紀錄事件("開始攝影");
            await 開始進行錄影攝影();
        }

        async Task 模式2()
        {
            double sleep = Convert.ToDouble(錄音錄影啟動暫停時間秒數.Text);

            紀錄事件("");
            發生異常需要停止Log = false;
            紀錄事件("攝影機初始化");
            await 攝影機初始化();
            產生錄影檔案名稱();

            await Task.Delay(TimeSpan.FromSeconds(sleep));
            紀錄事件("開始攝影");
            await 開始進行錄影攝影();

            紀錄事件("開始照相");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 照相();

        }

        async Task 模式3()
        {
            double sleep = Convert.ToDouble(錄音錄影啟動暫停時間秒數.Text);

            紀錄事件("");
            發生異常需要停止Log = false;
            紀錄事件("攝影機初始化");
            await 攝影機初始化();
            產生錄影檔案名稱();

            紀錄事件("暫停預覽");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 停止進行錄影預覽();

            紀錄事件("開始照相");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 照相();

            await Task.Delay(TimeSpan.FromSeconds(sleep));
            紀錄事件("開始預覽");
            await 開始進行錄影預覽();

            紀錄事件("開始攝影");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 開始進行錄影攝影();
        }

        async Task 模式4()
        {
            double sleep = Convert.ToDouble(錄音錄影啟動暫停時間秒數.Text);

            紀錄事件("");
            發生異常需要停止Log = false;
            紀錄事件("攝影機初始化");
            await 攝影機初始化();
            產生錄影檔案名稱();

            紀錄事件("開始攝影");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 開始進行錄影攝影();

            紀錄事件("暫停預覽");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 停止進行錄影預覽();

            紀錄事件("開始照相");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 照相();

            await Task.Delay(TimeSpan.FromSeconds(sleep));
            紀錄事件("開始預覽");
            await 開始進行錄影預覽();
        }

        async Task 模式5()
        {
            double sleep = Convert.ToDouble(錄音錄影啟動暫停時間秒數.Text);

            紀錄事件("");
            發生異常需要停止Log = false;
            紀錄事件("攝影機初始化");
            await 攝影機初始化();
            產生錄影檔案名稱();

            紀錄事件("暫停預覽");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 停止進行錄影預覽();

            紀錄事件("開始照相");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 照相();

            紀錄事件("攝影機初始化");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 攝影機初始化();

            紀錄事件("開始攝影");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 開始進行錄影攝影();

        }

        async Task 模式6()
        {
            double sleep = Convert.ToDouble(錄音錄影啟動暫停時間秒數.Text);

            紀錄事件("");
            發生異常需要停止Log = false;
            紀錄事件("攝影機初始化");
            await 攝影機初始化();
            產生錄影檔案名稱();

            紀錄事件("暫停預覽");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 停止進行錄影預覽();

            紀錄事件("開始照相");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 照相();

            紀錄事件("攝影機初始化");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 攝影機初始化();

            紀錄事件("開始攝影");
            await Task.Delay(TimeSpan.FromSeconds(sleep));
            await 開始進行錄影攝影();

        }
        #endregion
        #endregion

        #endregion

        #region 錄影案件事件
        async private void 啟動錄影錄音裝置_Click(object sender, RoutedEventArgs e)
        {
            await 攝影機初始化();
        }

        async private void 停止錄影錄音裝置_Click(object sender, RoutedEventArgs e)
        {

        }

        async private void 進行錄影錄音測試_Click(object sender, RoutedEventArgs e)
        {
            me撥放器背景音樂.Play();
            string xs = 選擇錄影錄音測試模式.SelectedItem as string;
            switch (xs)
            {
                case "模式1":
                    await 模式1();
                    break;
                case "模式2":
                    await 模式2();
                    break;
                case "模式3":
                    await 模式3();
                    break;
                case "模式4":
                    await 模式4();
                    break;
                case "模式5":
                    await 模式5();
                    break;
                case "模式6":
                    await 模式6();
                    break;
            }
        }

        async private void 停止錄影錄音_Click(object sender, RoutedEventArgs e)
        {
            紀錄事件("停止攝影");
            await 停止進行錄影攝影();
            me撥放器背景音樂.Stop();
            me撥放器.Source = new Uri(string.Format("ms-appdata:///local/{0}.mp4", 錄影與照片檔案名稱));
        }
        #endregion

        #region MediaCapture異常事件
        private void Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            MainHelper.ShowToast(errorEventArgs.Code.ToString(), errorEventArgs.Message);
        }


        #endregion

        #region 其他方法
        public string RandomString(int size = 20)
        {
            string rndStr = "abcdefghijklmnopqrstuvwxyz1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int rndStrLen = rndStr.Length;
            StringBuilder builder = new StringBuilder();
            Random random = new Random((int)(DateTime.Now.Ticks % 999999));
            string ch;
            for (int i = 0; i < size; i++)
            {
                ch = rndStr.Substring(random.Next(rndStrLen), 1);
                builder.Append(ch);
            }

            builder.Clear();
            builder.Append(DateTime.Now.ToString("yyyyMMdd_hhmmss"));
            return builder.ToString();
        }

        void 產生錄影檔案名稱()
        {
            錄影與照片檔案名稱 = RandomString(20);
        }

        void 紀錄事件(string msg)
        {
            if (msg == "")
            {
                處理紀錄.Text = "";
            }
            if (發生異常需要停止Log == false)
            {
                處理紀錄.Text += "\r\n" + string.Format("{0} {1}", DateTime.Now, msg);
            }
        }

        #endregion

        #region 音樂撥放速度測試

        private void 撥放音樂_Click(object sender, RoutedEventArgs e)
        {
            me撥放器.Source = new Uri(string.Format("ms-appx:///Assets/music{0}.mp3", MusicID.Text));
            me撥放器.Play();
        }

        private void 撥放音樂1_Click(object sender, RoutedEventArgs e)
        {
            //me撥放器.Source = new Uri(string.Format("ms-appx:///Assets/Music0.wav"));
            me撥放器.Source = new Uri(string.Format("ms-appx:///Assets/Music{0}.wav", MusicID.Text));
            me撥放器.Play();
        }

        private void 停止音樂_Click(object sender, RoutedEventArgs e)
        {
            me撥放器.Stop();
        }

        private void 音樂10_Click(object sender, RoutedEventArgs e)
        {
            me撥放器.PlaybackRate = 1.0;
            me撥放器.DefaultPlaybackRate = 1.0;
        }

        private void 音樂09_Click(object sender, RoutedEventArgs e)
        {
            me撥放器.PlaybackRate = 0.90;
            me撥放器.DefaultPlaybackRate = 0.90;
        }

        private void 音樂105_Click(object sender, RoutedEventArgs e)
        {
            me撥放器.PlaybackRate = 1.05;
            me撥放器.DefaultPlaybackRate = 1.05;
        }
          #endregion
  }
}
