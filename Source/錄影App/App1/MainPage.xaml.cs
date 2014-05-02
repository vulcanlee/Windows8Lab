using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Windows.Media.Capture.MediaCapture m_mediaCaptureMgr;
        private Windows.Storage.StorageFile m_photoStorageFile;
        private Windows.Storage.StorageFile m_recordStorageFile;
        private readonly String PHOTO_FILE_NAME = "photo.jpg";
        private readonly String VIDEO_FILE_NAME = "video.mp4";


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

        private async void EnumerateWebcamsAsync()
        {
            try
            {
                ShowStatusMessage("Enumerating Webcams...");
                m_devInfoCollection = null;

                m_devInfoCollection = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                if (m_devInfoCollection.Count == 0)
                {
                    ShowStatusMessage("No WebCams found.");
                }
                else
                {
                    for (int i = 0; i < m_devInfoCollection.Count; i++)
                    {
                        var devInfo = m_devInfoCollection[i];
                    }
                    ShowStatusMessage("Enumerating Webcams completed successfully.");
                }
            }
            catch (Exception e)
            {
                ShowExceptionMessage(e);
            }
        }

        internal async void btnStartDevice_Click(Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                EnableButton(false, "StartDevice");
                ShowStatusMessage("Starting device");
                m_mediaCaptureMgr = new Windows.Media.Capture.MediaCapture();

                await m_mediaCaptureMgr.InitializeAsync();

                EnableButton(true, "StartPreview");
                EnableButton(true, "StartStopRecord");
                EnableButton(true, "TakePhoto");
                ShowStatusMessage("Device initialized successful");

                m_mediaCaptureMgr.RecordLimitationExceeded += new Windows.Media.Capture.RecordLimitationExceededEventHandler(RecordLimitationExceeded); ;
                m_mediaCaptureMgr.Failed += new Windows.Media.Capture.MediaCaptureFailedEventHandler(Failed); ;

            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
            }
        }

        public async void Failed(Windows.Media.Capture.MediaCapture currentCaptureObject, MediaCaptureFailedEventArgs currentFailure)
        {
            try
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    ShowStatusMessage("Fatal error" + currentFailure.Message);
                });
            }
            catch (Exception e)
            {
                ShowExceptionMessage(e);
            }
        }

        private void ScenarioReset()
        {
            previewCanvas1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            ScenarioInit();
        }

        private void ScenarioInit()
        {
            btnStartDevice1.IsEnabled = true;
            btnStartPreview1.IsEnabled = false;
            btnStartStopRecord1.IsEnabled = false;
            m_bRecording = false;
            m_bPreviewing = false;
            btnStartStopRecord1.Content = "StartRecord";
            btnTakePhoto1.IsEnabled = false;
            previewElement1.Source = null;
            playbackElement1.Source = null;
            imageElement1.Source = null;
            sldBrightness.IsEnabled = false;
            sldContrast.IsEnabled = false;
            m_bSuspended = false;
            previewCanvas1.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

            ShowStatusMessage("");

        }

        internal void sldContrast_ValueChanged(Object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            try
            {
                bool succeeded = m_mediaCaptureMgr.VideoDeviceController.Contrast.TrySetValue(sldContrast.Value);
                if (!succeeded)
                {
                    ShowStatusMessage("Set Contrast failed");
                }
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
            }
        }

        // VideoDeviceControllers
        internal void sldBrightness_ValueChanged(Object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            try
            {
                bool succeeded = m_mediaCaptureMgr.VideoDeviceController.Brightness.TrySetValue(sldBrightness.Value);
                if (!succeeded)
                {
                    ShowStatusMessage("Set Brightness failed");
                }
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
            }
        }

        public async void RecordLimitationExceeded(Windows.Media.Capture.MediaCapture currentCaptureObject)
        {
            try
            {
                if (m_bRecording)
                {
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                    {
                        try
                        {
                            ShowStatusMessage("Stopping Record on exceeding max record duration");
                            await m_mediaCaptureMgr.StopRecordAsync();
                            m_bRecording = false;
                            SwitchRecordButtonContent();
                            EnableButton(true, "StartStopRecord");
                            ShowStatusMessage("Stopped record on exceeding max record duration:" + m_recordStorageFile.Path);
                        }
                        catch (Exception e)
                        {
                            ShowExceptionMessage(e);
                        }
                    });
                }
            }
            catch (Exception e)
            {
                ShowExceptionMessage(e);
                m_bRecording = false;
                SwitchRecordButtonContent();
                EnableButton(true, "StartStopRecord");
            }
        }

        private void SwitchRecordButtonContent()
        {
            {
                if (m_bRecording)
                {
                    btnStartStopRecord1.Content = "StopRecord";
                }
                else
                {
                    btnStartStopRecord1.Content = "StartRecord";
                }
            }
        }

        internal async void btnStartPreview_Click(Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            m_bPreviewing = false;
            try
            {
                ShowStatusMessage("Starting preview");
                EnableButton(false, "StartPreview");

                previewCanvas1.Visibility = Windows.UI.Xaml.Visibility.Visible;
                previewElement1.Source = m_mediaCaptureMgr;

                await m_mediaCaptureMgr.StartPreviewAsync();

                if ((m_mediaCaptureMgr.VideoDeviceController.Brightness != null) && m_mediaCaptureMgr.VideoDeviceController.Brightness.Capabilities.Supported)
                {
                    SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Brightness, sldBrightness);
                }
                if ((m_mediaCaptureMgr.VideoDeviceController.Contrast != null) && m_mediaCaptureMgr.VideoDeviceController.Contrast.Capabilities.Supported)
                {
                    SetupVideoDeviceControl(m_mediaCaptureMgr.VideoDeviceController.Contrast, sldContrast);
                }
                m_bPreviewing = true;
                ShowStatusMessage("Start preview successful");

            }
            catch (Exception exception)
            {
                m_bPreviewing = false;
                previewElement1.Source = null;
                EnableButton(true, "StartPreview");
                ShowExceptionMessage(exception);
            }
        }

        private void SetupVideoDeviceControl(Windows.Media.Devices.MediaDeviceControl videoDeviceControl, Slider slider)
        {
            try
            {
                if ((videoDeviceControl.Capabilities).Supported)
                {
                    slider.IsEnabled = true;
                    slider.Maximum = videoDeviceControl.Capabilities.Max;
                    slider.Minimum = videoDeviceControl.Capabilities.Min;
                    slider.StepFrequency = videoDeviceControl.Capabilities.Step;
                    double controlValue = 0;
                    if (videoDeviceControl.TryGetValue(out controlValue))
                    {
                        slider.Value = controlValue;
                    }
                }
                else
                {
                    slider.IsEnabled = false;
                }
            }
            catch (Exception e)
            {
                ShowExceptionMessage(e);
            }
        }

        internal async void btnStartStopRecord_Click(Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                if (!m_bRecording)
                {
                    var m_recordStorageFile = await SaveFile(".mp4");
                    this.m_recordStorageFile = m_recordStorageFile;

                    if (m_recordStorageFile != null)
                    {
                        ShowStatusMessage("Starting Record");

                        EnableButton(false, "StartStopRecord");

                        //m_recordStorageFile = await Windows.Storage.KnownFolders.VideosLibrary.CreateFileAsync(fileName, Windows.Storage.CreationCollisionOption.GenerateUniqueName);

                        ShowStatusMessage("Create record file successful");

                        MediaEncodingProfile recordProfile = null;
                        recordProfile = MediaEncodingProfile.CreateMp4(Windows.Media.MediaProperties.VideoEncodingQuality.Auto);

                        await m_mediaCaptureMgr.StartRecordToStorageFileAsync(recordProfile, m_recordStorageFile);
                        m_bRecording = true;
                        SwitchRecordButtonContent();
                        EnableButton(true, "StartStopRecord");

                        ShowStatusMessage("Start Record successful");
                    }
                    else
                    {

                    }
                }
                else
                {
                    ShowStatusMessage("Stopping Record");

                    await m_mediaCaptureMgr.StopRecordAsync();

                    m_bRecording = false;
                    EnableButton(true, "StartStopRecord");
                    SwitchRecordButtonContent();

                    ShowStatusMessage("Stop record successful");
                    if (!m_bSuspended)
                    {
                        var stream = await m_recordStorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);

                        ShowStatusMessage("Record file opened");
                        ShowStatusMessage(this.m_recordStorageFile.Path);
                        playbackElement1.AutoPlay = true;
                        playbackElement1.SetSource(stream, this.m_recordStorageFile.FileType);
                        playbackElement1.Play();


                    }

                }
            }
            catch (Exception ex)
            {
                ShowExceptionMessage(ex);
                m_bRecording = false;
                SwitchRecordButtonContent();
                EnableButton(true, "StartStopRecord");
            }

        }

        /// <summary>
        /// 存檔使用FileSavePicker
        /// </summary>
        /// <returns></returns>G
        public static async Task<StorageFile> SaveFile(params string[] fileTypeArray)
        {
            FileSavePicker savePicker = new FileSavePicker();

            StorageFile storageFile = null;

            if (fileTypeArray.GetLength(0) > 0)
            {
                savePicker.DefaultFileExtension = fileTypeArray[0];
                savePicker.SuggestedStartLocation = PickerLocationId.Desktop;

                List<string> fileTypeList = fileTypeArray.ToList();
                foreach (var fileType in fileTypeList)
                {
                    savePicker.FileTypeChoices.Add(fileType, new List<string>() { fileType });
                }



                storageFile = await savePicker.PickSaveFileAsync();
            }

            return storageFile;
        }

        internal async void btnTakePhoto_Click(Object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            try
            {
                m_photoStorageFile = await SaveFile(".png", ".jpg");

                if (m_photoStorageFile != null)
                {
                    ShowStatusMessage("Taking photo");
                    EnableButton(false, "TakePhoto");

                    ShowStatusMessage("Create photo file successful");

                    ImageEncodingProperties imageProperties = null;

                    switch (m_photoStorageFile.FileType)
                    {
                        case ".png":
                            imageProperties = ImageEncodingProperties.CreatePng();
                            break;
                        case ".jpg":
                            imageProperties = ImageEncodingProperties.CreateJpeg();
                            break;
                        default:
                            EnableButton(true, "TakePhoto");
                            throw new NotImplementedException();
                            break;
                    }

                    await m_mediaCaptureMgr.CapturePhotoToStorageFileAsync(imageProperties, m_photoStorageFile);

                    EnableButton(true, "TakePhoto");
                    ShowStatusMessage("Photo taken");

                    var photoStream = await m_photoStorageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    ShowStatusMessage("File open successful");
                    var bmpimg = new BitmapImage();

                    bmpimg.SetSource(photoStream);
                    imageElement1.Source = bmpimg;
                    ShowStatusMessage(this.m_photoStorageFile.Path);
                }
            }
            catch (Exception exception)
            {
                ShowExceptionMessage(exception);
                EnableButton(true, "TakePhoto");
            }
        }

        private void ShowExceptionMessage(Exception ex)
        {
            NotifyUser(ex.Message, NotifyType.ErrorMessage);
        }

        private void ShowStatusMessage(string text)
        {
            NotifyUser(text, NotifyType.StatusMessage);
        }

        public void NotifyUser(string strMessage, NotifyType type)
        {
            //switch (type)
            //{
            //    // Use the status message style.
            //    case NotifyType.StatusMessage:
            //        StatusBlock.Style = Resources["StatusStyle"] as Style;
            //        break;
            //    // Use the error message style.
            //    case NotifyType.ErrorMessage:
            //        StatusBlock.Style = Resources["ErrorStyle"] as Style;
            //        break;
            //}
            StatusBlock.Text = strMessage;

            // Collapse the StatusBlock if it has no text to conserve real estate.
            if (StatusBlock.Text != String.Empty)
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                StatusBlock.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
        }

        private void EnableButton(bool enabled, String name)
        {
            if (name.Equals("StartDevice"))
            {
                btnStartDevice1.IsEnabled = enabled;
            }
            else if (name.Equals("StartPreview"))
            {
                btnStartPreview1.IsEnabled = enabled;
            }
            else if (name.Equals("StartStopRecord"))
            {
                btnStartStopRecord1.IsEnabled = enabled;
            }
            else if (name.Equals("TakePhoto"))
            {
                btnTakePhoto1.IsEnabled = enabled;
            }
        }

        public bool m_bPreviewing { get; set; }

        public bool m_bRecording { get; set; }

        public bool m_bSuspended { get; set; }

        public MediaCaptureInitializationSettings settings { get; set; }

        public DeviceInformationCollection m_devInfoCollection { get; set; }

        private void btnRotationDevice_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
