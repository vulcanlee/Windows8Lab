using Microsoft.Xna.Framework.Media;
using SharpDX.IO;
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

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace 音樂同步測試
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        double totalSec1 = 0.0;
        double totalSec2 = 0.0;
        double totalSec3 = 0.0;
        double totalSec4 = 0.0;
        double totalSec5 = 0.0;
        double 設定的撥放速度 = 1.0;

        bool 顯示監控訊息 = true;
        DispatcherTimer _timer = new DispatcherTimer();
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
            me撥放器1.Stop();
            me撥放器2.Stop();
            me撥放器3.Stop();
            me撥放器4.Stop();
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
            totalSec1 = this.me撥放器1.Position.TotalMilliseconds / 1000.0;
            totalSec2 = this.me撥放器2.Position.TotalMilliseconds / 1000.0;
            totalSec3 = this.me撥放器3.Position.TotalMilliseconds / 1000.0;
            totalSec4 = this.me撥放器4.Position.TotalMilliseconds / 1000.0;

            if (顯示監控訊息 == true)
            {
                tbMessage1.Text = string.Format("T:{0:0.0000}  T:{0:0.0000}  T:{0:0.0000}  T:{0:0.0000}", totalSec1, totalSec2, totalSec3, totalSec4);
            }
        }

        public void 設定播放位置(int m, int s, double PlaybackRate)
        {
            //Debug.WriteLine(string.Format("PrevME1 {0}", me撥放器1.CurrentState));
            //Debug.WriteLine(string.Format("PrevME2 {0}", me撥放器2.CurrentState));
            //Debug.WriteLine(string.Format("PrevME3 {0}", me撥放器3.CurrentState));
            //Debug.WriteLine(string.Format("PrevME4 {0}", me撥放器4.CurrentState));
            //me撥放器1.Stop();
            //me撥放器2.Stop();
            //me撥放器3.Stop();
            //me撥放器4.Stop();
            //Debug.WriteLine(string.Format("SPME1 {0}", me撥放器1.CurrentState));
            //Debug.WriteLine(string.Format("SPME2 {0}", me撥放器2.CurrentState));
            //Debug.WriteLine(string.Format("SPME3 {0}", me撥放器3.CurrentState));
            //Debug.WriteLine(string.Format("SPME4 {0}", me撥放器4.CurrentState));

            TimeSpan ts = new TimeSpan(0, m, s);
            me撥放器1.Position = ts;
            me撥放器2.Position = ts;
            me撥放器3.Position = ts;
            me撥放器4.Position = ts;

            Debug.WriteLine(string.Format("ME1 {0}", me撥放器1.CurrentState));
            Debug.WriteLine(string.Format("ME2 {0}", me撥放器2.CurrentState));
            Debug.WriteLine(string.Format("ME3 {0}", me撥放器3.CurrentState));
            Debug.WriteLine(string.Format("ME4 {0}", me撥放器4.CurrentState));
            //await Task.Delay(1000);

            me撥放器1.Play();
            me撥放器1.PlaybackRate = PlaybackRate;
            if ((cb只播放一個音樂檔.SelectedItem as string) == "No")
            {
                me撥放器2.Play();
                me撥放器3.Play();
                me撥放器4.Play();

                me撥放器2.PlaybackRate = PlaybackRate;
                me撥放器3.PlaybackRate = PlaybackRate;
                me撥放器4.PlaybackRate = PlaybackRate;
            }
        }

        private void btnBegin_Click(object sender, RoutedEventArgs e)
        {
            設定播放位置(0, 0, 設定的撥放速度);
            SetupTimer();
        }

        private void btnSec150_Click(object sender, RoutedEventArgs e)
        {
            設定播放位置(2, 30, 設定的撥放速度);
            SetupTimer();
        }

        private void btnSec270_Click(object sender, RoutedEventArgs e)
        {
            設定播放位置(4, 30, 設定的撥放速度);
            SetupTimer();
        }

        private void btnSec70_Click(object sender, RoutedEventArgs e)
        {
            設定播放位置(1, 10, 設定的撥放速度);
            SetupTimer();
        }

        private void btnSec210_Click(object sender, RoutedEventArgs e)
        {
            設定播放位置(3, 30, 設定的撥放速度);
            SetupTimer();
        }

        private void me撥放器1_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME1 {0}", me撥放器1.CurrentState));
        }

        private void me撥放器2_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME2 {0}", me撥放器2.CurrentState));
        }

        private void me撥放器3_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME3 {0}", me撥放器3.CurrentState));
        }

        private void me撥放器4_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format(">>ME4 {0}", me撥放器4.CurrentState));
        }

        private void btn媒體撥放器狀態_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Format("媒體撥放器狀態1 {0}", me撥放器1.CurrentState));
            Debug.WriteLine(string.Format("媒體撥放器狀態2 {0}", me撥放器2.CurrentState));
            Debug.WriteLine(string.Format("媒體撥放器狀態3 {0}", me撥放器3.CurrentState));
            Debug.WriteLine(string.Format("媒體撥放器狀態4 {0}", me撥放器4.CurrentState));
            tbMessage2.Text = string.Format("{0}  {1}  {2}  {3}",
                string.Format("媒體撥放器狀態1 {0}", me撥放器1.CurrentState),
                string.Format("媒體撥放器狀態2 {0}", me撥放器1.CurrentState),
                string.Format("媒體撥放器狀態3 {0}", me撥放器1.CurrentState),
                string.Format("媒體撥放器狀態4 {0}", me撥放器1.CurrentState));
        }

        private void btn監控訊息是否顯示_Click(object sender, RoutedEventArgs e)
        {
            顯示監控訊息 = !顯示監控訊息;
        }

        private void 撥放速度_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                設定的撥放速度 = Convert.ToDouble((撥放速度.SelectedItem as string));
            }
            catch { }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            me撥放器1.Stop();
            me撥放器2.Stop();
            me撥放器3.Stop();
            me撥放器4.Stop();
        }

        private void btnXAudio2_Click(object sender, RoutedEventArgs e)
        {
            //XAudio2 xaudio;
            //MasteringVoice masteringVoice;

            //xaudio = new XAudio2();
            //masteringVoice = new MasteringVoice(xaudio);

            //var nativefilestream = new NativeFileStream(
            //    @"Assets\海綿寶寶.wav",
            //    NativeFileMode.Open,
            //    NativeFileAccess.Read,
            //    NativeFileShare.Read);

            //var soundstream = new SoundStream(nativefilestream);


            //var waveFormat = soundstream.Format;
            //var buffer = new AudioBuffer
            //{
            //    Stream = soundstream.ToDataStream(),
            //    AudioBytes = (int)soundstream.Length,
            //    Flags = BufferFlags.EndOfStream,
            //};

            //var sourceVoice = new SourceVoice(xaudio, waveFormat, true);

            //// There is also support for shifting the frequency.
            //sourceVoice.SetFrequencyRatio(1.0f);

            //sourceVoice.SubmitSourceBuffer(buffer, soundstream.DecodedPacketsInfo);

            //sourceVoice.Start();
        }

        private void btn1Begin_Click(object sender, RoutedEventArgs e)
        {
            var song = commonContent.Load(songFile);
            MediaPlayer.
        }

        private void btn1Sec270_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
