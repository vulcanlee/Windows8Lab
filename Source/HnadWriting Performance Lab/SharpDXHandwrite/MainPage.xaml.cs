using SharpDXHandwrite.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VulcanSharpDXClassLibrary;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace SharpDXHandwrite
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Field
        #region SharpDX Direct2D繪圖會用到的物件        
        private VulcanImageSource Scenario1Drawing;
        private ImageBrush d2dBrush;
        SharpDX.Direct2D1.PathGeometry1 pathGeometry1;

        private VulcanImageSource Scenario1Drawing_左邊;
        private ImageBrush d2dBrush_左邊;
        private VulcanImageSource Scenario1Drawing_右邊;
        private ImageBrush d2dBrush_右邊;
        #endregion

        #region 手寫筆記會用到的變數

        public bool 正在手寫中 = false;

        頁面手寫物件軌跡 頁面手寫物件軌跡_左 = new 頁面手寫物件軌跡();
        頁面手寫物件軌跡 頁面手寫物件軌跡_右 = new 頁面手寫物件軌跡();
        手寫物件軌跡 手寫物件軌跡_左 = new 手寫物件軌跡();
        手寫物件軌跡 手寫物件軌跡_右 = new 手寫物件軌跡();
        使用中的繪圖工具 使用中的繪圖工具 = new 使用中的繪圖工具();
        uint PenID, TouchID;
        double X1, X2, Y1, Y2, LastX, LastY, StrokeThickness = 1;
        Point StartPoint, PreviousContactPoint, CurrentContactPoint;
        double IntX = 0;
        double IntY = 0;
        int lastx = 0;
        int lasty = 0;
        bool isStartDrawing = false;
        bool isFirstPoint = false;


        DateTime 最後繪圖時間 = DateTime.Now;
        DispatcherTimer timer;
        int 動畫的開始繪製座標 = 0;
        #endregion

        #endregion

        #region 屬性
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// 此項可能變更為強類型檢視模型。
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper 是用在每個頁面上協助巡覽及
        /// 處理程序生命週期管理
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }
        #endregion

        #region 頁面事件

        public MainPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);

            this.Loaded += MainPage_Loaded;
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;

            this.正在手寫中 = false;
            使用中的繪圖工具.線條粗細 = 3;
            使用中的繪圖工具.手或筆與橡皮擦 = 手或筆與橡皮擦.筆;
            使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_color_b;
            更新最新選擇方法();

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 30);
            timer.Tick += timer_Tick;
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            #region 測試區會用到的 SharpDX物件初始化
            Scenario1Drawing = new VulcanImageSource((int)canvasAutoTest.Width, (int)canvasAutoTest.Height, false);
            d2dBrush = new ImageBrush();
            canvasAutoTest.Background = d2dBrush;
            d2dBrush.ImageSource = Scenario1Drawing;
            #endregion

            #region 左邊手寫面板會用到的SharpDX物件初始化
            Scenario1Drawing_左邊 = new VulcanImageSource((int)canvas左邊手寫畫板.Width, (int)canvas左邊手寫畫板.Height, false);
            d2dBrush_左邊 = new ImageBrush();
            canvas左邊手寫畫板.Background = d2dBrush_左邊;
            d2dBrush_左邊.ImageSource = Scenario1Drawing_左邊;
            #endregion

            #region 右邊手寫面板會用到的SharpDX物件初始化
            Scenario1Drawing_右邊 = new VulcanImageSource((int)canvas右邊手寫畫板.Width, (int)canvas右邊手寫畫板.Height, false);
            d2dBrush_右邊 = new ImageBrush();
            canvas右邊手寫畫板.Background = d2dBrush_右邊;
            d2dBrush_右邊.ImageSource = Scenario1Drawing_右邊;
            #endregion

            btnPrimitiveBlend.Content = Scenario1Drawing_左邊.PrimitiveBlend.ToString();

        }
        #endregion

        #region NavigationHelper 註冊

        /// 本區段中提供的方法只用來允許
        /// NavigationHelper 可回應頁面的巡覽方法。
        /// 
        /// 頁面專屬邏輯應該放在事件處理常式中
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// 和 <see cref="GridCS.Common.NavigationHelper.SaveState"/>。
        /// 巡覽參數可用於 LoadState 方法
        /// 除了先前的工作階段期間保留的頁面狀態。

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            this.canvas左邊手寫畫板.PointerMoved += canvas左邊手寫畫板_PointerMoved;
            this.canvas左邊手寫畫板.PointerReleased += canvas左邊手寫畫板_PointerReleased;
            this.canvas左邊手寫畫板.PointerPressed += canvas左邊手寫畫板_PointerPressed;
            this.canvas左邊手寫畫板.PointerExited += canvas左邊手寫畫板_PointerExited;
            this.canvas左邊手寫畫板.PointerCaptureLost += canvas左邊手寫畫板_PointerCaptureLost;

            this.canvas右邊手寫畫板.PointerMoved += canvas右邊手寫畫板_PointerMoved;
            this.canvas右邊手寫畫板.PointerReleased += canvas右邊手寫畫板_PointerReleased;
            this.canvas右邊手寫畫板.PointerPressed += canvas右邊手寫畫板_PointerPressed;
            this.canvas右邊手寫畫板.PointerExited += canvas右邊手寫畫板_PointerExited;
            this.canvas右邊手寫畫板.PointerCaptureLost += canvas右邊手寫畫板_PointerCaptureLost;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);

            this.canvas左邊手寫畫板.PointerMoved -= canvas左邊手寫畫板_PointerMoved;
            this.canvas左邊手寫畫板.PointerReleased -= canvas左邊手寫畫板_PointerReleased;
            this.canvas左邊手寫畫板.PointerPressed -= canvas左邊手寫畫板_PointerPressed;
            this.canvas左邊手寫畫板.PointerExited -= canvas左邊手寫畫板_PointerExited;
            this.canvas左邊手寫畫板.PointerCaptureLost -= canvas左邊手寫畫板_PointerCaptureLost;

            this.canvas右邊手寫畫板.PointerMoved -= canvas右邊手寫畫板_PointerMoved;
            this.canvas右邊手寫畫板.PointerReleased -= canvas右邊手寫畫板_PointerReleased;
            this.canvas右邊手寫畫板.PointerPressed -= canvas右邊手寫畫板_PointerPressed;
            this.canvas右邊手寫畫板.PointerExited -= canvas右邊手寫畫板_PointerExited;
            this.canvas右邊手寫畫板.PointerCaptureLost -= canvas右邊手寫畫板_PointerCaptureLost;
        }

        #endregion

        #region 測試用的事件
        private void 自動隨機繪製測試_Click(object sender, RoutedEventArgs e)
        {
            // Create a new pseudo-random number generator
            Random randomGenerator = new Random((int)DateTime.Now.Ticks);

            int mm = randomGenerator.Next(100, 200);


            // Begin updating the SurfaceImageSource
            Scenario1Drawing.BeginDraw();

            // Clear background
            Scenario1Drawing.Clear(Colors.Transparent);

            //Random randomGenerator = new Random();
            byte[] pixelValues = new byte[3]; // Represents the red, green, and blue channels of a color

            // Draw 50 random retangles
            Color colorGreen = Colors.Green;
            Color colorRed = Colors.Red;

            Color colorTransparent = Colors.Transparent;
            colorTransparent.A = 0;

            for (int i = 0; i < 2000; i++)
            {
                // Generate a new random color
                //randomGenerator.NextBytes(pixelValues);
                //Color color = new Color() { R = pixelValues[0], G = pixelValues[1], B = pixelValues[2], A = 255 };


                int p1x = randomGenerator.Next(10, 760);
                int p1y = randomGenerator.Next(10, 1000);
                int p2x = randomGenerator.Next(10, 760);
                int p2y = randomGenerator.Next(10, 1000);

                Scenario1Drawing.DrawLine(p1x, p1y, p2x, p2y, Scenario1Drawing.ConvertToSolidColorBrush(colorGreen), 1);

                // Add a new randomly colored 50x50 rectangle that will fit somewhere within the bounds of the Image1 control
                //Scenario1Drawing.FillSolidRect(
                //    color,
                //    new Rect(randomGenerator.Next((int)Image1.Width - 50), randomGenerator.Next((int)Image1.Height - 50), 50, 50)
                //    );
            }

            for (int i = 0; i < 100; i++)
            {
                int p1x = randomGenerator.Next(10, 760);
                int p1y = randomGenerator.Next(10, 1000);
                int p2x = randomGenerator.Next(10, 760);
                int p2y = randomGenerator.Next(10, 1000);

                //Scenario1Drawing.DrawLine(p1x, p1y, p2x, p2y, new SharpDX.Direct2D1.SolidColorBrush(Scenario1Drawing.d2dContext, SharpDX.Color.Transparent), 10);
                Scenario1Drawing.DrawLine(p1x, p1y, p2x, p2y, Scenario1Drawing.ConvertToSolidColorBrush(colorTransparent), 10);
            }

            for (int i = 0; i < 30; i++)
            {
                // Generate a new random color
                randomGenerator.NextBytes(pixelValues);
                Color colorRandom = new Color() { R = pixelValues[0], G = pixelValues[1], B = pixelValues[2], A = 255 };

                int p1x = randomGenerator.Next(10, 760);
                int p1y = randomGenerator.Next(10, 1000);
                int p2x = randomGenerator.Next(10, 760);
                int p2y = randomGenerator.Next(10, 1000);

                Scenario1Drawing.DrawLine(p1x, p1y, p2x, p2y, Scenario1Drawing.ConvertToSolidColorBrush(colorRandom), 3);
            }

            // Stop updating the SurfaceImageSource and draw its contents
            Scenario1Drawing.EndDraw();
        }

        private void 自動隨機動畫測試_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled == true)
            {
                timer.Stop();
            }
            else
            {
                Scenario1Drawing.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Copy;
                Scenario1Drawing.setPrimitiveBlend();
                動畫的開始繪製座標 = 0;
                使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_opacity_b;
                timer.Start();
            }
        }

        private void timer_Tick(object sender, object e)
        {
            // Begin updating the SurfaceImageSource
            Scenario1Drawing.BeginDraw();

            // Clear background
            Scenario1Drawing.Clear(Colors.Transparent);

            Color colorGreen = Colors.Green;
            Color colorRed = Colors.Red;
            Color colorTransparent = Colors.Transparent;
            colorTransparent.A = 0;

            int 線條粗細 = 1;

            int p1x = 0;
            int p1y = 0;
            int p2x = 0;
            int p2y = 0;
            float 線條粗細f = 0.0f;

            colorGreen = ColorsHelper.Parse("66ff0000");
            colorGreen = 取得手寫物件調色盤的實際Color(使用中的繪圖工具.手寫物件調色盤);
            Scenario1Drawing.FillSolidRect(colorGreen, new Rect(0, 0, 768, 動畫的開始繪製座標));

            // Stop updating the SurfaceImageSource and draw its contents
            Scenario1Drawing.EndDraw();

            動畫的開始繪製座標 += 10;

            if ((動畫的開始繪製座標 % 100) == 0)
            {
                while (true)
                {
                    更新到最新的手寫物件();
                    colorGreen = 取得手寫物件調色盤的實際Color(使用中的繪圖工具.手寫物件調色盤);
                    if (colorGreen.A == 255)
                    {

                    }
                    else
                    {
                        break;
                    }
                }

            }

            if (動畫的開始繪製座標 >= 1024)
            {
                動畫的開始繪製座標 = 0;
            }
        }


        private async void 儲存圖片_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(gd測試圖片);

            // 1. Get the pixels
            IBuffer pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var pixels = await renderTargetBitmap.GetPixelsAsync();

            var filePicker = new FileSavePicker();

            filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            var textFileExtension = new[] { ".jpg" };
            var dataFileExtension = new[] { ".png" };

            filePicker.FileTypeChoices.Add("JPG", textFileExtension);
            filePicker.FileTypeChoices.Add("PNG", dataFileExtension);

            IAsyncOperation<StorageFile> asyncOp = filePicker.PickSaveFileAsync();
            StorageFile file = await asyncOp;

            if (file == null)
            {
                return;
            }

            
            using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await
                    BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                byte[] bytes = pixels.ToArray();
                encoder.SetPixelData(BitmapPixelFormat.Bgra8,
                                     BitmapAlphaMode.Ignore,
                                     (uint)gd測試圖片.Width, (uint)gd測試圖片.Height,
                                     96, 96, bytes);

                await encoder.FlushAsync();
            }
            
        }
        #endregion

        #region 手寫事件
        #region 左邊手寫事件
        private void canvas左邊手寫畫板_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            最後繪圖時間 = DateTime.Now;
            手寫物件軌跡_左 = new 手寫物件軌跡()
            {
                StrokeThickness = 使用中的繪圖工具.線條粗細,
                手寫物件調色盤 = 使用中的繪圖工具.手寫物件調色盤,
                手或筆與橡皮擦 = this.使用中的繪圖工具.手或筆與橡皮擦,
                手寫物件s = new List<手寫物件>(),
            };

            if (this.使用中的繪圖工具.手或筆與橡皮擦 == 手或筆與橡皮擦.橡皮擦)
            {
                手寫物件軌跡_左.手寫物件調色盤 = 手寫物件調色盤.img_color_g;
            }

            手寫物件軌跡_左.手或筆與橡皮擦 = this.使用中的繪圖工具.手或筆與橡皮擦;

            頁面手寫物件軌跡_左.手寫物件軌跡s.Add(手寫物件軌跡_左);
            PreviousContactPoint = e.GetCurrentPoint(this.canvas左邊手寫畫板).Position;
            LastX = PreviousContactPoint.X;
            LastY = PreviousContactPoint.Y;

            if (e.GetCurrentPoint(this.canvas左邊手寫畫板).Properties.IsLeftButtonPressed)
            {
                PenID = e.GetCurrentPoint(this.canvas左邊手寫畫板).PointerId;
                e.Handled = true;
                this.正在手寫中 = true;
            }
        }

        private void canvas左邊手寫畫板_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (this.正在手寫中 == false)
            {
                return;
            }

            if (e.Pointer.PointerId == PenID || e.Pointer.PointerId == TouchID)
            {
                foreach (PointerPoint pointerPoint in e.GetIntermediatePoints(this.canvas左邊手寫畫板).Reverse())
                {
                    CurrentContactPoint = pointerPoint.Position;
                    X1 = PreviousContactPoint.X;
                    Y1 = PreviousContactPoint.Y;
                    X2 = CurrentContactPoint.X;
                    Y2 = CurrentContactPoint.Y;

                    if (this.使用中的繪圖工具.手或筆與橡皮擦 == 手或筆與橡皮擦.筆)
                    {
                        手寫物件軌跡_左.手寫物件s.Add(new 手寫物件()
                        {
                            //X1 = LastX,
                            X1 = X1,
                            X2 = X2,
                            //Y1 = LastY,
                            Y1 = Y1,
                            Y2 = Y2,
                        });
                    }
                    else
                    {
                        手寫物件軌跡_左.手寫物件s.Add(new 手寫物件()
                        {
                            X1 = X1,
                            X2 = X2,
                            Y1 = Y1,
                            Y2 = Y2,
                        });
                    }
                    //LastX = CurrentContactPoint.X;
                    //LastY = CurrentContactPoint.Y;

                    PreviousContactPoint = CurrentContactPoint;
                }
                Redraw左邊手寫板();
            }
        }

        private void canvas左邊手寫畫板_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            TouchID = 0;
            PenID = 0;
            e.Handled = true;
            this.正在手寫中 = false;
        }

        private void canvas左邊手寫畫板_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            this.正在手寫中 = false;
        }

        private void canvas左邊手寫畫板_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.正在手寫中 = false;
            Debug.WriteLine("canvas左邊手寫畫板_PointerExited");
        }

        #endregion

        #region 右邊手寫事件
        private void canvas右邊手寫畫板_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            最後繪圖時間 = DateTime.Now;
            手寫物件軌跡_右 = new 手寫物件軌跡()
            {
                StrokeThickness = 使用中的繪圖工具.線條粗細,
                手寫物件調色盤 = 使用中的繪圖工具.手寫物件調色盤,
                手或筆與橡皮擦 = this.使用中的繪圖工具.手或筆與橡皮擦,
                手寫物件s = new List<手寫物件>(),
            };

            if (this.使用中的繪圖工具.手或筆與橡皮擦 == 手或筆與橡皮擦.橡皮擦)
            {
                手寫物件軌跡_右.手寫物件調色盤 = 手寫物件調色盤.img_color_g;
            }

            手寫物件軌跡_右.手或筆與橡皮擦 = this.使用中的繪圖工具.手或筆與橡皮擦;

            頁面手寫物件軌跡_右.手寫物件軌跡s.Add(手寫物件軌跡_右);
            PreviousContactPoint = e.GetCurrentPoint(this.canvas右邊手寫畫板).Position;
            LastX = PreviousContactPoint.X;
            LastY = PreviousContactPoint.Y;

            if (e.GetCurrentPoint(this.canvas右邊手寫畫板).Properties.IsLeftButtonPressed)
            {
                PenID = e.GetCurrentPoint(this.canvas右邊手寫畫板).PointerId;
                e.Handled = true;
                this.正在手寫中 = true;
            }
        }

        private void canvas右邊手寫畫板_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (this.正在手寫中 == false)
            {
                return;
            }

            if (e.Pointer.PointerId == PenID || e.Pointer.PointerId == TouchID)
            {
                foreach (PointerPoint pointerPoint in e.GetIntermediatePoints(this.canvas右邊手寫畫板).Reverse())
                {
                    CurrentContactPoint = pointerPoint.Position;
                    X1 = PreviousContactPoint.X;
                    Y1 = PreviousContactPoint.Y;
                    X2 = CurrentContactPoint.X;
                    Y2 = CurrentContactPoint.Y;

                    if (this.使用中的繪圖工具.手或筆與橡皮擦 == 手或筆與橡皮擦.筆)
                    {
                        手寫物件軌跡_右.手寫物件s.Add(new 手寫物件()
                        {
                            //X1 = LastX,
                            X1 = X1,
                            X2 = X2,
                            //Y1 = LastY,
                            Y1 = Y1,
                            Y2 = Y2,
                        });
                    }
                    else
                    {
                        手寫物件軌跡_右.手寫物件s.Add(new 手寫物件()
                        {
                            X1 = X1,
                            X2 = X2,
                            Y1 = Y1,
                            Y2 = Y2,
                        });
                    }
                    //LastX = CurrentContactPoint.X;
                    //LastY = CurrentContactPoint.Y;

                    PreviousContactPoint = CurrentContactPoint;
                }
                Redraw右邊手寫板();
            }
        }

        private void canvas右邊手寫畫板_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            TouchID = 0;
            PenID = 0;
            e.Handled = true;
            this.正在手寫中 = false;
        }

        private void canvas右邊手寫畫板_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            this.正在手寫中 = false;
        }

        private void canvas右邊手寫畫板_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.正在手寫中 = false;
            Debug.WriteLine("canvas右邊手寫畫板_PointerExited");
        }

        #endregion

        #region 重新繪製手寫與橡皮擦軌跡
        public void Redraw左邊手寫板()
        {
            // Begin updating the SurfaceImageSource
            Scenario1Drawing_左邊.BeginDraw();

            // Clear background
            Scenario1Drawing_左邊.Clear(Colors.Transparent);

            Color colorGreen = Colors.Green;
            Color colorRed = Colors.Red;
            Color colorTransparent = Colors.Transparent;
            colorTransparent.A = 0;

            int 線條粗細 = 1;

            int p1x = 0;
            int p1y = 0;
            int p2x = 0;
            int p2y = 0;
            float 線條粗細f = 0.0f;

            foreach (var 手寫物件軌跡s in 頁面手寫物件軌跡_左.手寫物件軌跡s)
            {
                if (手寫物件軌跡s.手或筆與橡皮擦 == 手或筆與橡皮擦.筆)
                {
                    colorGreen = 手寫物件軌跡s.取得手寫物件調色盤的實際Color();
                }
                else
                {
                    colorGreen = colorTransparent;
                }
                線條粗細 = (int)手寫物件軌跡s.StrokeThickness;
                線條粗細f = (float)線條粗細;
                線條粗細f /= 2.0f;

                #region 使用線條來繪製
                isStartDrawing = false;
                foreach (var item in 手寫物件軌跡s.手寫物件s)
                {
                    p1x = (int)item.X1;
                    p1y = (int)item.Y1;
                    p2x = (int)item.X2;
                    p2y = (int)item.Y2;
                    Scenario1Drawing_左邊.DrawLine(p1x, p1y, p2x, p2y, Scenario1Drawing_左邊.ConvertToSolidColorBrush(colorGreen), 線條粗細);
                    if (線條粗細 != 1)
                    {
                        //Scenario1Drawing_左邊.DrawEllipse(p1x, p1y, 線條粗細f, 線條粗細f, Scenario1Drawing_左邊.ConvertToSolidColorBrush(colorGreen), 1);
                        Scenario1Drawing_左邊.DrawFillEllipse(p1x, p1y, 線條粗細f, 線條粗細f, Scenario1Drawing_左邊.ConvertToSolidColorBrush(colorGreen));
                    }
                }
                #endregion
            }

            // Stop updating the SurfaceImageSource and draw its contents
            Scenario1Drawing_左邊.EndDraw();
        }

        public void Redraw右邊手寫板()
        {
            // Begin updating the SurfaceImageSource
            Scenario1Drawing_右邊.BeginDraw();

            // Clear background
            Scenario1Drawing_右邊.Clear(Colors.Transparent);

            Color colorGreen = Colors.Green;
            Color colorRed = Colors.Red;
            Color colorTransparent = Colors.Transparent;
            colorTransparent.A = 0;

            int 線條粗細 = 1;

            int p1x = 0;
            int p1y = 0;
            int p2x = 0;
            int p2y = 0;
            float 線條粗細f = 0.0f;

            foreach (var 手寫物件軌跡s in 頁面手寫物件軌跡_右.手寫物件軌跡s)
            {
                if (手寫物件軌跡s.手或筆與橡皮擦 == 手或筆與橡皮擦.筆)
                {
                    colorGreen = 手寫物件軌跡s.取得手寫物件調色盤的實際Color();
                }
                else
                {
                    colorGreen = colorTransparent;
                }
                線條粗細 = (int)手寫物件軌跡s.StrokeThickness;
                線條粗細f = (float)線條粗細;
                線條粗細f /= 2.0f;

                #region 使用 PathGeometry1 來繪製

                #endregion

                #region 使用線條來繪製
                isStartDrawing = false;
                foreach (var item in 手寫物件軌跡s.手寫物件s)
                {
                    p1x = (int)item.X1;
                    p1y = (int)item.Y1;
                    p2x = (int)item.X2;
                    p2y = (int)item.Y2;
                    Scenario1Drawing_右邊.DrawLine(p1x, p1y, p2x, p2y, Scenario1Drawing_右邊.ConvertToSolidColorBrush(colorGreen), 線條粗細);
                    if (線條粗細 != 1)
                    {
                        Scenario1Drawing_右邊.DrawFillEllipse(p1x, p1y, 線條粗細f, 線條粗細f, Scenario1Drawing_右邊.ConvertToSolidColorBrush(colorGreen));
                    }

                }
                #endregion
            }

            Scenario1Drawing_右邊.EndDraw();
        }

        #endregion

        #endregion

        #region 切換功能的點選事件

        private void btn線條粗細_Click(object sender, RoutedEventArgs e)
        {
            change線條粗細();
        }

        private void btn手或筆與橡皮擦_Click(object sender, RoutedEventArgs e)
        {
            switch (使用中的繪圖工具.手或筆與橡皮擦)
            {
                case 手或筆與橡皮擦.筆:
                    使用中的繪圖工具.手或筆與橡皮擦 = 手或筆與橡皮擦.橡皮擦;
                    break;
                case 手或筆與橡皮擦.橡皮擦:
                    使用中的繪圖工具.手或筆與橡皮擦 = 手或筆與橡皮擦.筆;
                    break;
                case 手或筆與橡皮擦.手:
                    break;
                default:
                    break;
            }
            更新最新選擇方法();
        }

        private void btn調色盤_Click(object sender, RoutedEventArgs e)
        {
            更新到最新的手寫物件();
        }

        public void 更新到最新的手寫物件()
        {
            switch (使用中的繪圖工具.手寫物件調色盤)
            {
                case 手寫物件調色盤.img_color_black:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_color_gray;
                    break;
                case 手寫物件調色盤.img_color_gray:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_color_r;
                    break;
                case 手寫物件調色盤.img_color_r:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_color_b;
                    break;
                case 手寫物件調色盤.img_color_b:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_color_g;
                    break;
                case 手寫物件調色盤.img_color_g:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_opacity_b;
                    break;
                case 手寫物件調色盤.img_opacity_b:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_opacity_g;
                    break;
                case 手寫物件調色盤.img_opacity_g:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_opacity_r;
                    break;
                case 手寫物件調色盤.img_opacity_r:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_opacity_o;
                    break;
                case 手寫物件調色盤.img_opacity_o:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_opacity_y;
                    break;
                case 手寫物件調色盤.img_opacity_y:
                    使用中的繪圖工具.手寫物件調色盤 = 手寫物件調色盤.img_color_black;
                    break;
                default:
                    break;
            }
            更新最新選擇方法();
        }


        public void change線條粗細()
        {
            switch (使用中的繪圖工具.線條粗細)
            {
                case 1:
                    使用中的繪圖工具.線條粗細 = 3;
                    break;
                case 3:
                    使用中的繪圖工具.線條粗細 = 5;
                    break;
                case 5:
                    使用中的繪圖工具.線條粗細 = 7;
                    break;
                case 7:
                    使用中的繪圖工具.線條粗細 = 9;
                    break;
                case 9:
                    使用中的繪圖工具.線條粗細 = 11;
                    break;
                case 11:
                    使用中的繪圖工具.線條粗細 = 15;
                    break;
                case 15:
                    使用中的繪圖工具.線條粗細 = 20;
                    break;
                case 20:
                    使用中的繪圖工具.線條粗細 = 1;
                    break;
                default:
                    break;
            }
            更新最新選擇方法();
        }

        private void btn左邊復原_Click(object sender, RoutedEventArgs e)
        {
            var last = 頁面手寫物件軌跡_左.手寫物件軌跡s.LastOrDefault();
            if (last != null)
            {
                頁面手寫物件軌跡_左.手寫物件軌跡s.Remove(last);
                更新最新選擇方法();
                Redraw左邊手寫板();
            }
        }


        private void btn右邊復原_Click(object sender, RoutedEventArgs e)
        {
            var last = 頁面手寫物件軌跡_右.手寫物件軌跡s.LastOrDefault();
            if (last != null)
            {
                頁面手寫物件軌跡_右.手寫物件軌跡s.Remove(last);
                更新最新選擇方法();
                Redraw右邊手寫板();
            }
        }

        private void btnPrimitiveBlend_Click(object sender, RoutedEventArgs e)
        {
            string pb = btnPrimitiveBlend.Content as string;

            switch (pb)
            {
                case "Copy":
                    Scenario1Drawing_左邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Minimum;
                    Scenario1Drawing_右邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Minimum;
                    Scenario1Drawing_左邊.setPrimitiveBlend();
                    Scenario1Drawing_右邊.setPrimitiveBlend();
                    btnPrimitiveBlend.Content = Scenario1Drawing_左邊.PrimitiveBlend.ToString();
                    break;
                case "Minimum":
                    Scenario1Drawing_左邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.SourceOver;
                    Scenario1Drawing_右邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.SourceOver;
                    Scenario1Drawing_左邊.setPrimitiveBlend();
                    Scenario1Drawing_右邊.setPrimitiveBlend();
                    btnPrimitiveBlend.Content = Scenario1Drawing_左邊.PrimitiveBlend.ToString();
                    break;
                case "SourceOver":
                    Scenario1Drawing_左邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Add;
                    Scenario1Drawing_右邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Add;
                    Scenario1Drawing_左邊.setPrimitiveBlend();
                    Scenario1Drawing_右邊.setPrimitiveBlend();
                    btnPrimitiveBlend.Content = Scenario1Drawing_左邊.PrimitiveBlend.ToString();
                    break;
                case "Add":
                    Scenario1Drawing_左邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Copy;
                    Scenario1Drawing_右邊.PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Copy;
                    Scenario1Drawing_左邊.setPrimitiveBlend();
                    Scenario1Drawing_右邊.setPrimitiveBlend();
                    btnPrimitiveBlend.Content = Scenario1Drawing_左邊.PrimitiveBlend.ToString();
                    break;
                default:
                    break;
            }
            Redraw左邊手寫板();
            Redraw右邊手寫板();
        }
        #endregion

        #region 其他方法
        public void 更新最新選擇方法()
        {
            tb線條粗細.Text = 使用中的繪圖工具.線條粗細.ToString();
            tb手或筆與橡皮擦.Text = 使用中的繪圖工具.手或筆與橡皮擦.ToString();
            tb調色盤.Text = 使用中的繪圖工具.手寫物件調色盤.ToString();
        }

        public Color 取得手寫物件調色盤的實際Color(手寫物件調色盤 手寫物件調色盤)
        {
            Color solidColorBrush = Colors.Black;

            switch (手寫物件調色盤)
            {
                case 手寫物件調色盤.img_color_black:
                    solidColorBrush = ColorsHelper.Parse("ff1a1a1a");
                    break;
                case 手寫物件調色盤.img_color_gray:
                    solidColorBrush = ColorsHelper.Parse("ff999999");
                    break;
                case 手寫物件調色盤.img_color_r:
                    solidColorBrush = ColorsHelper.Parse("ffff0000");
                    break;
                case 手寫物件調色盤.img_color_b:
                    solidColorBrush = ColorsHelper.Parse("ff006cff");
                    break;
                case 手寫物件調色盤.img_color_g:
                    solidColorBrush = ColorsHelper.Parse("ff0da522");
                    break;
                case 手寫物件調色盤.img_opacity_b:
                    solidColorBrush = ColorsHelper.Parse("6627e8ff");
                    break;
                case 手寫物件調色盤.img_opacity_g:
                    solidColorBrush = ColorsHelper.Parse("66999999");
                    break;
                case 手寫物件調色盤.img_opacity_r:
                    solidColorBrush = ColorsHelper.Parse("66ff0000");
                    break;
                case 手寫物件調色盤.img_opacity_o:
                    solidColorBrush = ColorsHelper.Parse("66ffa200");
                    break;
                case 手寫物件調色盤.img_opacity_y:
                    solidColorBrush = ColorsHelper.Parse("66ffea00");
                    break;
                default:
                    break;
            }
            return solidColorBrush;
        }
        #endregion


    }
}
