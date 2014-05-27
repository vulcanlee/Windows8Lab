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

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace MediaCaptureTest
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class DeviceInformation : Page
    {
        MediaCapture m_mediaCaptureMgr = null;
        private IRandomAccessStream AudioStream;
        private VideoEncodingQuality SelectedQuality;
        MediaEncodingProfile encodingProfile = null;
        string 錄影與照片檔案名稱 = "";
        bool 發生異常需要停止Log = false;

        public DeviceInformation()
        {
            this.InitializeComponent();
        }

        #region 網路攝影機動作
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
                //m_mediaCaptureMgr.VideoDeviceController.Zoom.
                var captureInitSettings = new MediaCaptureInitializationSettings();
                captureInitSettings.StreamingCaptureMode = StreamingCaptureMode.AudioAndVideo;
                string xu = "VideoPreview";
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
                產生錄影檔案名稱();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                MainHelper.ShowToast("產生錯誤", "無法啟動錄音或錄影裝置");
                發生異常需要停止Log = true;
            }
        }

        private void Failed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            MainHelper.ShowToast("** " + errorEventArgs.Code.ToString(), errorEventArgs.Message);
        }

        #region 預覽 & 照相相關

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

        void 音訊或視訊檔案的編碼設定()
        {
            encodingProfile = null;

            string xs = "Auto";
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

        #endregion


        #endregion

        #region 取得裝置資訊
        VideoEncodingProperties MediaEncodingProperties_VideoPreview = null;
        VideoEncodingProperties MediaEncodingProperties_VideoRecord = null;
        VideoEncodingProperties MediaEncodingProperties_Photo = null;
        void 取得裝置資訊()
        {
            string line = "";
            裝置資訊清單.Text = "";
            // Find the highest resolution available
            VideoEncodingProperties resolutionMax = null;
            int max = 0;
            var xa = m_mediaCaptureMgr.VideoDeviceController;
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + "正在使用的裝置資訊";
            IMediaEncodingProperties xb = xa.GetMediaStreamProperties(MediaStreamType.VideoPreview);
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + xb.Type + " " + "VideoPreview";
            VideoEncodingProperties xc = xb as VideoEncodingProperties;
            MediaEncodingProperties_VideoPreview = xc;
            line = string.Format("W{0} H{1} ID{2} ", xc.Width, xc.Height, xc.ProfileId);
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + line;

            xb = xa.GetMediaStreamProperties(MediaStreamType.VideoRecord);
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + xb.Type + " " + "VideoRecord"; ;
            xc = xb as VideoEncodingProperties;
            MediaEncodingProperties_VideoRecord = xc;
            line = string.Format("W{0} H{1} ID{2} ", xc.Width, xc.Height, xc.ProfileId);
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + line;

            xb = xa.GetMediaStreamProperties(MediaStreamType.Photo);
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + xb.Type + " " + "Photo"; ;
            xc = xb as VideoEncodingProperties;
            MediaEncodingProperties_Photo = xc;
            line = string.Format("W{0} H{1} ID{2} ", xc.Width, xc.Height, xc.ProfileId);
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + line;

            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + "\r\n";

            現有裝置的可用支援(MediaStreamType.VideoPreview);
            現有裝置的可用支援(MediaStreamType.VideoRecord);
            現有裝置的可用支援(MediaStreamType.Photo);

            //await m_mediaCaptureMgr.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.VideoPreview, resolutionMax);

        }

        void 現有裝置的可用支援(MediaStreamType MediaStreamType)
        {
            裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n-----------------------------------------\r\n" + MediaStreamType.ToString() + "\r\n-----------------------------------------";
            string line = "";
            var xa = m_mediaCaptureMgr.VideoDeviceController;
            var resolutions = xa.GetAvailableMediaStreamProperties(MediaStreamType);
            for (var i = 0; i < resolutions.Count; i++)
            {
                VideoEncodingProperties res = (VideoEncodingProperties)resolutions[i];

                line = string.Format("W{0} H{1} ID{2} ", res.Width, res.Height, res.ProfileId);

                裝置資訊清單.Text = 裝置資訊清單.Text + "\r\n" + line;
                //Debug.WriteLine("resolution : " + res.Width + "x" + res.Height);
                //if (res.Width * res.Height > max)
                //{
                //    max = (int)(res.Width * res.Height);
                //    resolutionMax = res;
                //}
            }


        }

        private void 取得裝置資訊_Click(object sender, RoutedEventArgs e)
        {
            取得裝置資訊();
        }


        #endregion

        #region 控制網路攝影機事件

        async private void 攝影機初始化_Click(object sender, RoutedEventArgs e)
        {
            await 攝影機初始化();
        }

        async private void 開始預覽_Click(object sender, RoutedEventArgs e)
        {
            await 開始進行錄影預覽();
        }

        async private void 停止預覽_Click(object sender, RoutedEventArgs e)
        {
            await 停止進行錄影預覽();
        }

        async private void 照相_Click(object sender, RoutedEventArgs e)
        {
            await 照相();
        }

        async private void 開始攝影_Click(object sender, RoutedEventArgs e)
        {
            await 開始進行錄影攝影();
        }

        async private void 停止攝影_Click(object sender, RoutedEventArgs e)
        {
            await 停止進行錄影攝影();
        }
        #endregion

        #region 其他方法
        void 產生錄影檔案名稱()
        {
            錄影與照片檔案名稱 = RandomString(20);
        }

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

        #endregion

        async private void 強制設定攝影機_Click(object sender, RoutedEventArgs e)
        {
            VideoEncodingProperties resNew = null;
            var xa = m_mediaCaptureMgr.VideoDeviceController;
            var resolutions = xa.GetAvailableMediaStreamProperties(MediaStreamType.VideoRecord);
            for (var i = 0; i < resolutions.Count; i++)
            {
                VideoEncodingProperties res = (VideoEncodingProperties)resolutions[i];

                if (MediaEncodingProperties_VideoPreview.Width == res.Width && MediaEncodingProperties_VideoPreview.Height == res.Height)
                //if (320 == res.Width && 240 == res.Height)
                {
                    resNew = res;
                    break;
                }
            }

            if (resNew != null)
            {
                await xa.SetMediaStreamPropertiesAsync(MediaStreamType.VideoRecord, resNew);
            }
        }
    }
}
