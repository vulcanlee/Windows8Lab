using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace App1
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GestureRecognizer recognizer;
        bool 點選事件正在執行中 = false;
        bool 啟動了DoubleTapped = false;
        DispatcherTimer _長按手勢操作狀態Timer = new DispatcherTimer();
        長按手勢操作狀態 長按手勢操作狀態 = new 長按手勢操作狀態();
        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += MainPage_Loaded;
        }

        void MainPage_Loaded(object senderX, RoutedEventArgs e)
        {
            recognizer = new GestureRecognizer();
            recognizer.GestureSettings =
                GestureSettings.HoldWithMouse | GestureSettings.Hold;


            // Forward pointer input to the gesture recognizer.
            Rect1.PointerCanceled += OnPointerCanceled;
            Rect1.PointerPressed += OnPointerPressed;
            Rect1.PointerReleased += OnPointerReleased;
            Rect1.PointerMoved += OnPointerMoved;

            // Attach handlers for recognized gestures.
            // In each of these event handlers, args.PointerDeviceType
            // indicates the input device and 'sender' is GestureRecognizer.

            // args - TappedEventArgs
            recognizer.Tapped += (sender, args) => { tbxMessage.Text += string.Format("{0}\r\n", "Tapped raised"); };

            // args - RightTappedEventArgs
            recognizer.RightTapped += (sender, args) => { tbxMessage.Text += string.Format("{0}\r\n", "RightTapped raised"); };

            // The press-and-hold gesture.
            // args - HoldingEventArgs
            recognizer.Holding += (sender, args) => { tbxMessage.Text += string.Format("{0}\r\n", "Holding raised {0}", args.HoldingState); };

            // The slide or swipe gesture within a content area that supports 
            // panning along the perpendicular direction of the gesture.
            // args - CrossSlidingEventArgs
            recognizer.CrossSliding += (sender, args) => { tbxMessage.Text += string.Format("{0}\r\n", "CrossSliding raised"); };
        }

        #region Rect1

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0}\r\n", "Rectangle_Tapped");
        }

        private void Rectangle_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0}\r\n", "Rectangle_DoubleTapped");
        }

        private void Rect1_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0}\r\n", "Rect1_RightTapped");
        }

        private void Rect1_Holding(object sender, HoldingRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0} {1}\r\n", "Rect1_RightTapped", e.HoldingState);
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            tbxMessage.Text = "";
        }
        #endregion

        #region Rect2

        void OnPointerPressed(object sender, PointerRoutedEventArgs args)
        {
            // Route the events to the gesture recognizer.
            recognizer.ProcessDownEvent(args.GetCurrentPoint(Rect1));

            // Capture the pointer associated to this event. Rect1 is the element being manipulated. 
            // Once the pointer is captured, only the element that has the pointer raises 
            // the pointer-related events. 
            Rect1.CapturePointer(args.Pointer);

            // Mark the event handled to prevent execution of default handlers.
            args.Handled = true;
        }

        void OnPointerMoved(object sender, PointerRoutedEventArgs args)
        {
            // Pass all intermediate points to the gesture recognizer.
            recognizer.ProcessMoveEvents(args.GetIntermediatePoints(Rect1));
        }

        void OnPointerReleased(object sender, PointerRoutedEventArgs args)
        {
            recognizer.ProcessUpEvent(args.GetCurrentPoint(Rect1));
            Rect1.ReleasePointerCapture(args.Pointer);
            args.Handled = true;
        }

        void OnPointerCanceled(object sender, PointerRoutedEventArgs args)
        {
            recognizer.CompleteGesture();
            Rect1.ReleasePointerCapture(args.Pointer);
            args.Handled = true;
        }

        private void Rect2_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            啟動了DoubleTapped = true;
            tbxMessage.Text += string.Format("{0}\r\n", "Rect2_DoubleTapped");
        }

        private void Rect2_Holding(object sender, HoldingRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0} {1}\r\n", "Rect2_Holding", e.HoldingState);
        }

        private void Rect2_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0}\r\n", "Rect2_RightTapped");
        }

        private async void Rect2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (點選事件正在執行中 == false)
            {
                點選事件正在執行中 = true;

                啟動了DoubleTapped = false;
                await Task.Delay(350);
                if (啟動了DoubleTapped == false)
                {
                    tbxMessage.Text += string.Format("{0}\r\n", "Rect2_Tapped");
                }

                點選事件正在執行中 = false;
            }
        }
        #endregion

        #region Rect3
        private async void Rect3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (長按手勢操作狀態.長按操作需要停止Tap事件觸發)
            {
                長按手勢操作狀態.長按操作需要停止Tap事件觸發 = false;
                return;
            }

            if (點選事件正在執行中 == false)
            {
                點選事件正在執行中 = true;

                啟動了DoubleTapped = false;
                await Task.Delay(350);
                if (啟動了DoubleTapped == false)
                {
                    tbxMessage.Text += string.Format("{0}\r\n", "Rect3_Tapped");
                }
                點選事件正在執行中 = false;
            }
        }

        private void Rect3_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            啟動了DoubleTapped = true;
            tbxMessage.Text += string.Format("{0}\r\n", "Rect3_DoubleTapped");
        }

        private void Rect3_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0}\r\n", "Rect3_RightTapped");
        }

        private void Rect3_Holding(object sender, HoldingRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0} {1}\r\n", "Rect3_Holding", e.HoldingState);
        }

        private void Rect3_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (長按手勢操作狀態.長按操作正在進行中 == false)
            {
                sldr.Value = 0;
                sldr.Maximum = 長按手勢操作狀態.長按Timer最大觸發次數;
                長按手勢操作狀態.長按操作正在進行中 = true;
                長按手勢操作狀態.長按操作完成 = false;
                長按手勢操作狀態.長按操作需要停止Tap事件觸發 = false;
                長按手勢操作狀態.長按Timer觸發次數 = 0;

                長按手勢操作狀態.長按座標 = e.GetCurrentPoint(this.Rect3).Position;

                _長按手勢操作狀態Timer.Stop();
                //_長按手勢操作狀態Timer.Interval = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(長按手勢操作狀態.長按Timer觸發時間));
                _長按手勢操作狀態Timer.Interval = TimeSpan.FromMilliseconds(80);
                _長按手勢操作狀態Timer.Tick -= _長按手勢操作狀態Timer_Tick;
                _長按手勢操作狀態Timer.Tick -= _長按手勢操作狀態Timer_Tick;
                _長按手勢操作狀態Timer.Tick -= _長按手勢操作狀態Timer_Tick;
                _長按手勢操作狀態Timer.Tick += _長按手勢操作狀態Timer_Tick;
                _長按手勢操作狀態Timer.Start();
            }
        }

        void _長按手勢操作狀態Timer_Tick(object sender, object e)
        {
            if (長按手勢操作狀態.長按操作正在進行中)
            {
                #region 長按操作正在進行中
                長按手勢操作狀態.長按Timer觸發次數 += 1.0;
                sldr.Value = Convert.ToInt32(長按手勢操作狀態.長按Timer觸發次數);
                if (長按手勢操作狀態.長按Timer觸發次數 > 長按手勢操作狀態.長按Timer最大觸發次數)
                {
                    長按手勢操作狀態.長按操作需要停止Tap事件觸發 = true;
                    長按手勢操作狀態.長按操作正在進行中 = false;
                    長按手勢操作狀態.長按操作完成 = true;
                    // 
                    tbxMessage.Text += string.Format("{0}\r\n", "觸發長按事件");
                }
                #endregion
            }
            else
            {
                #region 長按操作沒有在進行中
                _長按手勢操作狀態Timer.Stop();
                _長按手勢操作狀態Timer.Tick -= _長按手勢操作狀態Timer_Tick;
                #endregion
            }
        }

        private void Rect3_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            長按手勢操作狀態.長按操作正在進行中 = false;
        }

        private void Rect3_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Point foopnt = e.GetCurrentPoint(this.Rect3).Position;
            if (
                長按手勢操作狀態.長按操作完成 == false &&
                ((Math.Abs(foopnt.X - 長按手勢操作狀態.長按座標.X) > 長按手勢操作狀態.長按靈敏度) ||
                (Math.Abs(foopnt.Y - 長按手勢操作狀態.長按座標.Y) > 長按手勢操作狀態.長按靈敏度))
                )
            {
                長按手勢操作狀態.長按操作需要停止Tap事件觸發 = true;
                長按手勢操作狀態.長按操作正在進行中 = false;
            }
        }

        private void Rect3_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {

        }

        private void Rect3_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {

        }

        public void 長按事件回報()
        {
            tbxMessage.Text += string.Format("{0} {1}\r\n", "Rect3_長按事件回報", "");
        }
         
        private void Rect3_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            //tbxMessage.Text += string.Format("{0}\r\n", "Rect3_ManipulationDelta");
            if (e.Cumulative.Translation.X >= 30)
            {
                tbxMessage.Text += string.Format("{0}\r\n", "向右滑動");
                e.Complete();
            }
            else if (e.Cumulative.Translation.X <= -30)
            {
                tbxMessage.Text += string.Format("{0}\r\n", "向左滑動");
                e.Complete();
            }
        }

        private void Rect3_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0}\r\n", "Rect3_ManipulationStarted");
        }

        private void Rect3_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            tbxMessage.Text += string.Format("{0}\r\n", "Rect3_ManipulationStarting");
        }

      #endregion

    }

    public class 長按手勢操作狀態
    {
        public 長按手勢操作狀態()
        {
            this.長按操作正在進行中 = false;
            this.長按操作完成 = false;
            this.長按操作需要停止Tap事件觸發 = false;
            this.長按靈敏度 = 5.0;
            this.長按觸發時間門檻 = 1.0;
            this.長按Timer觸發時間 = 100;
            this.長按Timer觸發次數 = 0;
            this.長按Timer最大觸發次數 = this.長按觸發時間門檻 * 1000.0 / this.長按Timer觸發時間;
        }
        public bool 長按操作正在進行中 { get; set; }
        public bool 長按操作需要停止Tap事件觸發 { get; set; }
        public bool 長按操作完成 { get; set; }
        public double 長按靈敏度 { get; set; }
        public double 長按觸發時間門檻 { get; set; }
        public double 長按Timer觸發次數 { get; set; }
        public double 長按Timer最大觸發次數 { get; set; }
        public double 長按Timer觸發時間 { get; set; }
        public Point 長按座標 { get; set; }
    }
}
