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

using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.Media.Capture;
using System.Threading.Tasks;

namespace MediaRecorder
{
   
    public sealed partial class MainPage : Page
    {
        public enum RecordingMode
        {
            Initializing,
            Recording,
            Stopped,
        };

        private MediaCapture CaptureMedia;
        private IRandomAccessStream AudioStream;
        private FileSavePicker FileSave;
        private DispatcherTimer DishTimer;
        private TimeSpan SpanTime;
        private AudioEncodingFormat SelectedFormat;
        private AudioEncodingQuality SelectedQuality;

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitMediaCapture();
            LoadAudioEncodings();
            LoadAudioQualities();
            UpdateRecordingControls(RecordingMode.Initializing);
            InitTimer();
        }

        private async Task InitMediaCapture()
        {
            CaptureMedia = new MediaCapture();
            var captureInitSettings = new MediaCaptureInitializationSettings();
            captureInitSettings.StreamingCaptureMode = StreamingCaptureMode.Audio;
            await CaptureMedia.InitializeAsync(captureInitSettings);
            CaptureMedia.Failed += MediaCaptureOnFailed;
            CaptureMedia.RecordLimitationExceeded += MediaCaptureOnRecordLimitationExceeded;
        }

        private void LoadAudioEncodings()
        {
            var audioEncodingFormats = Enum.GetValues(typeof(AudioEncodingFormat)).Cast<AudioEncodingFormat>();
            AudioFormat.ItemsSource = audioEncodingFormats;
            AudioFormat.SelectedItem = AudioEncodingFormat.Mp3;
        }

        private void LoadAudioQualities()
        {
            var audioQualities = Enum.GetValues(typeof(AudioEncodingQuality)).Cast<AudioEncodingQuality>();
            AudioQuality.ItemsSource = audioQualities;
            AudioQuality.SelectedItem = AudioEncodingQuality.Auto;
        }

        private void UpdateRecordingControls(RecordingMode recordingMode)
        {
            switch (recordingMode)
            {
                case RecordingMode.Initializing:
                    RecordBtn.IsEnabled = true;
                    StopBtn.IsEnabled = false;
                    SaveBtn.IsEnabled = false;
                    break;
                case RecordingMode.Recording:
                    RecordBtn.IsEnabled = false;
                    StopBtn.IsEnabled = true;
                    SaveBtn.IsEnabled = false;
                    break;
                case RecordingMode.Stopped:
                    RecordBtn.IsEnabled = true;
                    StopBtn.IsEnabled = false;
                    SaveBtn.IsEnabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("recordingMode");
            }
        }

        private void InitTimer()
        {
            DishTimer = new DispatcherTimer();
            DishTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            DishTimer.Tick += TimerOnTick;
        }

        private void TimerOnTick(object sender, object o)
        {
            SpanTime = SpanTime.Add(DishTimer.Interval);
            Duration.DataContext = SpanTime;
        }

        private async void MediaCaptureOnRecordLimitationExceeded(MediaCapture sender)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                await sender.StopRecordAsync();
                var warningMessage = new MessageDialog("The media recording has been stopped because you exceeded the maximum recording length.", "Recording Stoppped");
                await warningMessage.ShowAsync();
            });
        }

        private async void MediaCaptureOnFailed(MediaCapture sender, MediaCaptureFailedEventArgs errorEventArgs)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                var warningMessage = new MessageDialog(String.Format("The media capture failed: {0}", errorEventArgs.Message), "Capture Failed");
                await warningMessage.ShowAsync();
            });
        }

        private async void RecordBtn_Click(object sender, RoutedEventArgs e)
        {
            MediaEncodingProfile encodingProfile = null;

            switch (SelectedFormat)
            {
                case AudioEncodingFormat.Mp3:
                    encodingProfile = MediaEncodingProfile.CreateMp3(SelectedQuality);
                    break;
                case AudioEncodingFormat.Mp4:
                    encodingProfile = MediaEncodingProfile.CreateM4a(SelectedQuality);
                    break;
                case AudioEncodingFormat.Wma:
                    encodingProfile = MediaEncodingProfile.CreateWma(SelectedQuality);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            AudioStream = new InMemoryRandomAccessStream();
            await CaptureMedia.StartRecordToStreamAsync(encodingProfile, AudioStream);
            UpdateRecordingControls(RecordingMode.Recording);
            DishTimer.Start();
        }

        private async void StopBtn_Click(object sender, RoutedEventArgs e)
        {
            await CaptureMedia.StopRecordAsync();
            UpdateRecordingControls(RecordingMode.Stopped);
            DishTimer.Stop();
        }

        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var mediaFile = await FileSave.PickSaveFileAsync();

            if (mediaFile != null)
            {
                using (var dataReader = new DataReader(AudioStream.GetInputStreamAt(0)))
                {
                    await dataReader.LoadAsync((uint)AudioStream.Size);
                    byte[] buffer = new byte[(int)AudioStream.Size];
                    dataReader.ReadBytes(buffer);
                    await FileIO.WriteBytesAsync(mediaFile, buffer);
                    UpdateRecordingControls(RecordingMode.Initializing);
                }
            }
        }

        private void AudioFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFormat = (AudioEncodingFormat)AudioFormat.SelectedItem;
            InitFileSavePicker();
        }

        private void InitFileSavePicker()
        {
            FileSave = new FileSavePicker();
            FileSave.FileTypeChoices.Add("Encoding", new List<string>() { SelectedFormat.ToFileExtension() });
            FileSave.SuggestedStartLocation = PickerLocationId.MusicLibrary;
        }

        private void AudioQuality_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedQuality = (AudioEncodingQuality)AudioQuality.SelectedItem;
        }
    }
}
