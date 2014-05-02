//#define SHARPDXTEST

using SharpDX.IO;
using SharpDX.MediaFoundation;
using SharpDX.Multimedia;
using SharpDX.XAudio2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.PlayerFramework;
using Windows.Storage;


// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace 音樂同步測試
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Field
        double totalSec1 = 0.0;
        double totalSec2 = 0.0;
        double totalSec3 = 0.0;
        double totalSec4 = 0.0;
        double totalSec5 = 0.0;
        double SettingPlaybackRate = 1.0;

        bool ShowMessage = true;
        DispatcherTimer _timer = new DispatcherTimer();

        Stopwatch sw = new Stopwatch();

        #region SharpDX
        private XAudio2 xaudio2;
        private MasteringVoice masteringVoice;
        private Stream fileStream;
        private AudioPlayer audioPlayer;
        private DispatcherTimer timer;
        private object lockAudio = new object();
        private ToolTip volumeTooltip;
        #endregion
        #endregion

        public MainPage()
        {
            this.InitializeComponent();

            me1.BufferingProgressChanged += me1_BufferingProgressChanged;
            me2.BufferingProgressChanged += me1_BufferingProgressChanged;
            me3.BufferingProgressChanged += me1_BufferingProgressChanged;
            me4.BufferingProgressChanged += me1_BufferingProgressChanged;
        }

        void me1_BufferingProgressChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format("{0} Buffer Change", ((MediaElement)sender).Name, ((MediaElement)sender).BufferingProgress));
        }

        /// <summary>
        /// 在此頁面即將顯示在框架中時叫用。
        /// </summary>
        /// <param name="e">描述如何到達此頁面的事件資料。Parameter
        /// 屬性通常用來設定頁面。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            me1.Stop();
            me2.Stop();
            me3.Stop();
            me4.Stop();
        }

        private void SetupTimer()
        {
            _timer.Tick -= _timer_Tick;
            _timer.Tick -= _timer_Tick;
            _timer.Tick -= _timer_Tick;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            StartTimer();
        }

        private void StartTimer()
        {
            _timer.Tick += _timer_Tick;
            _timer.Start();
        }

        private void _timer_Tick(object sender, object e)
        {
            totalSec1 = this.me1.Position.TotalMilliseconds / 1000.0;
            totalSec2 = this.me2.Position.TotalMilliseconds / 1000.0;
            totalSec3 = this.me3.Position.TotalMilliseconds / 1000.0;
            totalSec4 = this.me4.Position.TotalMilliseconds / 1000.0;

            //if (totalSec1 > 272)
            //{
            //    TimeSpan ts = new TimeSpan(0, 0, 4, 30, 1);
            //    me1.Position = ts;
            //    if ((cbOnlyOneMP3.SelectedItem as string) == "No")
            //    {
            //        me2.Position = ts;
            //        me3.Position = ts;
            //        me4.Position = ts;
            //    }

            //}

            if (ShowMessage == true)
            {
#if (SHARPDXTEST)
                tbMessage1.Text = string.Format("{0:0.0000} {1:0.0000}  {2:0.0000} SD:{3:0.0000}", totalSec1, totalSec2, totalSec3, audioPlayer.Position.TotalSeconds);
#else
                //tbMessage1.Text = string.Format("T:{0:0.0000}  T:{1:0.0000}  T:{2:0.0000}  T:{3:0.0000}", totalSec1, totalSec2, totalSec3, totalSec4);                
                tbMessage1.Text = string.Format("T:{0:0.0000}  T:{1:0.0000}  T:{2:0.0000}  T:{3:0.0000}", totalSec1, totalSec2, totalSec3, totalSec4);
#endif
            }
        }

        //public async void SetPosition(int m, int s, double PlaybackRate)    
        public void SetPosition(int m, int s, double PlaybackRate)
        {
            sw.Reset();
#if (SHARPDXTEST)
            InitializeXAudio2();
            fileStream = new NativeFileStream(@"Assets\Clk_1Sec1.mp3", NativeFileMode.Open, NativeFileAccess.Read);
            audioPlayer = new AudioPlayer(xaudio2, fileStream);
#else
#endif



            TimeSpan ts = new TimeSpan(0, 0, m, s, 1);
            //await me1.SeekAsync(ts);
            me1.Position = ts;
            if ((cbOnlyOneMP3.SelectedItem as string) == "No")
            {
                //await me2.SeekAsync(ts);
                //await me3.SeekAsync(ts);
                //await me4.SeekAsync(ts);
                me2.Position = ts;
                me3.Position = ts;
                me4.Position = ts;
            }

            Debug.WriteLine(string.Format("T:{0:0.0000}  T:{1:0.0000}  T:{2:0.0000}  T:{3:0.0000}", me1.Position.TotalSeconds, me2.Position.TotalSeconds, me3.Position.TotalSeconds, me4.Position.TotalSeconds));











            //TimeSpan ts = new TimeSpan(0, 0, m, s,1);
            //me1.Position = ts;
            //if ((cbOnlyOneMP3.SelectedItem as string) == "No")
            //{
            //    me2.Position = ts;
            //    me3.Position = ts;
            //    me4.Position = ts;
            //}












#if (SHARPDXTEST)
            audioPlayer.Position = ts;
#else
#endif

            Debug.WriteLine(string.Format("-ME1 {0}", me1.CurrentState));
            Debug.WriteLine(string.Format("-ME2 {0}", me2.CurrentState));
            Debug.WriteLine(string.Format("-ME3 {0}", me3.CurrentState));
            Debug.WriteLine(string.Format("-ME4 {0}", me4.CurrentState));
            //await Task.Delay(1000);

            me1.DefaultPlaybackRate = PlaybackRate;
            me1.Play();
            //me1.Volume = 0;
            if ((cbOnlyOneMP3.SelectedItem as string) == "No")
            {
                me2.DefaultPlaybackRate = PlaybackRate;
                me3.DefaultPlaybackRate = PlaybackRate;
                me4.DefaultPlaybackRate = PlaybackRate;

                me2.Play();
                me3.Play();
                me4.Play();
                //me撥放器2.PlaybackRate = PlaybackRate;
                //me撥放器3.PlaybackRate = PlaybackRate;
                //me撥放器4.PlaybackRate = PlaybackRate;
            }
            sw.Start();
#if (SHARPDXTEST)
            audioPlayer.Play();
#else
#endif
        }

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {
            SetPosition(0, 0, SettingPlaybackRate);
            SetupTimer();
        }

        private void btnSec150_Click(object sender, RoutedEventArgs e)
        {
            SetPosition(2, 30, SettingPlaybackRate);
            SetupTimer();
        }

        private void btnSec270_Click(object sender, RoutedEventArgs e)
        {
            SetPosition(4, 30, SettingPlaybackRate);
            SetupTimer();
        }

        private void btnSec70_Click(object sender, RoutedEventArgs e)
        {
            SetPosition(1, 10, SettingPlaybackRate);
            SetupTimer();
        }

        private void btnSec210_Click(object sender, RoutedEventArgs e)
        {
            SetPosition(3, 30, SettingPlaybackRate);
            SetupTimer();
        }

        private void me1_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME1 {0}", me1.CurrentState));
        }

        private void me2_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME2 {0}", me2.CurrentState));
        }

        private void me3_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME3 {0}", me3.CurrentState));
        }

        private void me4_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME4 {0}", me4.CurrentState));
        }

        private void btnMediaElementStatus_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format("ME1 Status {0}", me1.CurrentState));
            Debug.WriteLine(string.Format("ME2 Status {0}", me2.CurrentState));
            Debug.WriteLine(string.Format("ME3 Status {0}", me3.CurrentState));
            Debug.WriteLine(string.Format("ME4 Status4 {0}", me4.CurrentState));
            tbMessage2.Text = string.Format("{0}  {1}  {2}  {3}",
                string.Format("ME1 Status {0}", me1.CurrentState),
                string.Format("ME2 Status {0}", me2.CurrentState),
                string.Format("ME3 Status {0}", me3.CurrentState),
                string.Format("ME4 Status {0}", me4.CurrentState));
        }

        private void btnShowPosition_Click(object sender, RoutedEventArgs e)
        {
            ShowMessage = !ShowMessage;
        }

        private void PlayingSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SettingPlaybackRate = Convert.ToDouble((PlayingSpeed.SelectedItem as string));
            }
            catch { }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            me1.Stop();
            me2.Stop();
            me3.Stop();
            me4.Stop();
        }

        private void btnXAudio2_Click(object sender, RoutedEventArgs e)
        {

            XAudio2 xaudio;
            MasteringVoice masteringVoice;

            xaudio = new XAudio2();
            masteringVoice = new MasteringVoice(xaudio);

            var nativefilestream = new NativeFileStream(
                @"Assets\Clk_1Sec1.wav",
                NativeFileMode.Open,
                NativeFileAccess.Read,
                NativeFileShare.Read);

            var soundstream = new SoundStream(nativefilestream);


            var waveFormat = soundstream.Format;
            var buffer = new AudioBuffer
            {
                Stream = soundstream.ToDataStream(),
                AudioBytes = (int)soundstream.Length,
                Flags = BufferFlags.EndOfStream,
            };

            var sourceVoice = new SourceVoice(xaudio, waveFormat, true);

            // There is also support for shifting the frequency.
            sourceVoice.SetFrequencyRatio(1.0f);

            sourceVoice.SubmitSourceBuffer(buffer, soundstream.DecodedPacketsInfo);

            sourceVoice.Start();
        }

        private void btnSharpDX_Click(object sender, RoutedEventArgs e)
        {
            InitializeXAudio2();
            fileStream = new NativeFileStream(@"Assets\Clk_1Sec1.mp3", NativeFileMode.Open, NativeFileAccess.Read);
            audioPlayer = new AudioPlayer(xaudio2, fileStream);
            audioPlayer.Play();
        }

        private void InitializeXAudio2()
        {
            // This is mandatory when using any of SharpDX.MediaFoundation classes
            MediaManager.Startup();

            // Starts The XAudio2 engine
            xaudio2 = new XAudio2();
            xaudio2.StartEngine();
            masteringVoice = new MasteringVoice(xaudio2);
        }

        private async void btnInitialization_Click(object sender, RoutedEventArgs e)
        {
            //var uri = new Uri("ms-appx:///Assets/Clk_1Sec1_可來提供的每秒鐘響一次音樂檔.mp3", UriKind.RelativeOrAbsolute);
            //var dst_file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("output.wav", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            //var dts_stream = dst_file.OpenStreamForWriteAsync();
            // Module01

        }

        private async void btnSharpDXMP3toWav_Click(object sender, RoutedEventArgs e)
        {
            var LocalFolder = await ApplicationData.Current.LocalFolder
                                             .CreateFolderAsync(@"Assets", CreationCollisionOption.OpenIfExists);
            var folder = await StorageFolder.GetFolderFromPathAsync(Windows.ApplicationModel.Package.Current.InstalledLocation.Path + @"\Assets");
            var fileMP3 = await folder.GetFileAsync(string.Format("{0}", "Clk_1Sec1_10s_70s_150s_210s_270s.mp3"));
            var fileWav = await LocalFolder.CreateFileAsync(string.Format("{0}", "SharpDx.wav"), CreationCollisionOption.ReplaceExisting);


            using (Stream streamMP3 = await fileMP3.OpenStreamForReadAsync())
            {
                using (Stream streamWav = await fileWav.OpenStreamForWriteAsync())
                {
                    AudioDecoder audioDecoder = new AudioDecoder(streamMP3);
                    WavWriter wavWriter = new WavWriter(streamWav);
                    

                    wavWriter.Begin(audioDecoder.WaveFormat);

                    // Decode the samples from the input file and output PCM raw data to the WAV stream.
                    wavWriter.AppendData(audioDecoder.GetSamples());

                    // Close the wav writer.
                    wavWriter.End();

                    audioDecoder.Dispose();
                }
            }

            this.me1.Source = new Uri(string.Format("ms-appdata:///local/Assets/{0}", "SharpDx.wav"));

            //Stream streamMP3 = await fileMP3.OpenStreamForReadAsync();
            //Stream streamWav = await fileWav.OpenStreamForWriteAsync();
            //AudioDecoder audioDecoder = new AudioDecoder(streamMP3);
            //WavWriter wavWriter = new WavWriter(streamWav);

            //wavWriter.Begin(audioDecoder.WaveFormat);

            //// Decode the samples from the input file and output PCM raw data to the WAV stream.
            //wavWriter.AppendData(audioDecoder.GetSamples());

            //// Close the wav writer.
            //wavWriter.End();

            //audioDecoder.Dispose();
            //await streamWav.FlushAsync();
        }

    }
}
