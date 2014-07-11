using HnadWriting.ComonDX;
using Scenario1Component;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace HnadWriting
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ImageBrush d3dBrush;
        private ImageBrush d2dBrush;
        private DeviceManager deviceManager;
        private SurfaceImageSourceTarget d3dTarget;
        private SurfaceImageSourceTarget d2dTarget;
        private ShapeRenderer shapeRenderer;
        private DragHandler d2dDragHandler;

        int totalDrawing = 0;

        頁面手寫物件軌跡 頁面手寫物件軌跡_左 = new 頁面手寫物件軌跡();
        SolidColorBrush solidColorBrush1 = null;
        SolidColorBrush solidColorBrush2 = null;
        SolidColorBrush solidColorBrush3 = null;
        SolidColorBrush solidColorBrush4 = null;
        SolidColorBrush solidColorBrush5 = null;
        WriteableBitmap wbContentImage;

        Stopwatch Stopwatch = new Stopwatch();

        #region Scenario1
        private Scenario1ImageSource Scenario1Drawing;
        #endregion
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            產生手寫軌跡的測試資料();
            solidColorBrush1 = new SolidColorBrush(ColorsHelper.Parse("ff1a1a1a"));
            solidColorBrush2 = new SolidColorBrush(ColorsHelper.Parse("ff999999"));
            solidColorBrush3 = new SolidColorBrush(ColorsHelper.Parse("ffff0000"));
            solidColorBrush4 = new SolidColorBrush(ColorsHelper.Parse("ff006cff"));
            solidColorBrush5 = new SolidColorBrush(ColorsHelper.Parse("ff0da522"));

            wbContentImage = BitmapFactory.New(768, 1024);
            wbContentImage.Clear(Colors.Transparent);
            cnUsingWriteableBitmap.Source = wbContentImage;


            d2dBrush = new ImageBrush();
            cnUsingGeometries.Background = d2dBrush;

            // Safely dispose any previous instance
            // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
            deviceManager = new DeviceManager();

            shapeRenderer = new ShapeRenderer();
            DisplayInformation DisplayInformation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            int pixelWidth = (int)(cnUsingGeometries.Width * DisplayInformation.LogicalDpi / 96.0);
            int pixelHeight = (int)(cnUsingGeometries.Height * DisplayInformation.LogicalDpi / 96.0);

            d2dTarget = new SurfaceImageSourceTarget(pixelWidth, pixelHeight);
            d2dBrush.ImageSource = d2dTarget.ImageSource;
            imgUsingInkManager.Source = d2dTarget.ImageSource;

            // Add Initializer to device manager
            deviceManager.OnInitialize += d2dTarget.Initialize;
            deviceManager.OnInitialize += shapeRenderer.Initialize;

            // Render the cube within the CoreWindow
            d2dTarget.OnRender += shapeRenderer.Render;

            // Initialize the device manager and all registered deviceManager.OnInitialize 
            deviceManager.Initialize(DisplayProperties.LogicalDpi);

            // Setup rendering callback
            //CompositionTarget.Rendering += CompositionTarget_Rendering;

            // Callback on DpiChanged
            DisplayProperties.LogicalDpiChanged += DisplayProperties_LogicalDpiChanged;

            #region Scenario1
            Scenario1Drawing = new Scenario1ImageSource((int)cnUsingDirectXs.Width, (int)cnUsingDirectXs.Height, true);

            // Use Scenario1Drawing as a source for the Ellipse shape's fill
            cnUsingDirectXs.Background = new ImageBrush() { ImageSource = Scenario1Drawing };
            #endregion
        }

        void DisplayProperties_LogicalDpiChanged(object sender)
        {
            deviceManager.Dpi = DisplayProperties.LogicalDpi;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            d2dTarget.RenderAll();
            d2dTarget.RenderAll();
        }

        private void btnUsingGeometriesDrawing_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch.Reset();
            Stopwatch.Start();


            d2dTarget.RenderAll();
            //if (totalDrawing == 0)
            //{
            //    d2dTarget.RenderAll();
            //    totalDrawing++;
            //}
            //else if ((totalDrawing % 2) == 0)
            //{
            //    d2dTarget.RenderAll();
            //    d2dTarget.RenderAll();
            //    totalDrawing = 1;
            //}
            //else
            //{
            //    d2dTarget.RenderAll();
            //    d2dTarget.RenderAll();
            //    totalDrawing++;
            //}


            Stopwatch.Stop();
            tbUsingGeometriesDrawing.Text = string.Format("{0} ms", Stopwatch.ElapsedMilliseconds);
        }

        #region XAML Line
        private void btnUsingXAMLLineDrawing_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch.Reset();
            Stopwatch.Start();
            this.cnUsingXAMLLine.Children.Clear();
            Random randomGenerator = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < 1500; i++)
            {
                int p1x = randomGenerator.Next(10, 760);
                int p1y = randomGenerator.Next(10, 1000);
                int p2x = randomGenerator.Next(10, 760);
                int p2y = randomGenerator.Next(10, 1000);
                Line line = new Line()
                {
                    X1 = p1x,
                    X2 = p2x,
                    Y1 = p1y,
                    Y2 = p2y,
                    StrokeThickness = 1,
                    Stroke = solidColorBrush3,
                };
                this.cnUsingXAMLLine.Children.Add(line);
            }
            Stopwatch.Stop();
            tbUsingXAMLLineDrawing.Text = string.Format("{0} ms", Stopwatch.ElapsedMilliseconds);
        }

        #endregion

        #region WriteableBitmap
        private void btnUsingWriteableBitmapDrawing_Click(object sender, RoutedEventArgs e)
        {
            Stopwatch.Reset();
            Stopwatch.Start();
            Random randomGenerator = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < 100; i++)
            {
                int p1x = randomGenerator.Next(10, 760);
                int p1y = randomGenerator.Next(10, 1000);
                int p2x = randomGenerator.Next(10, 760);
                int p2y = randomGenerator.Next(10, 1000);
                wbContentImage.DrawLine(p1x, p1y, p2x, p2y, Colors.Beige);
            }
            //cnUsingWriteableBitmap.Source = wbContentImage;
            Stopwatch.Stop();
            tbUsingWriteableBitmapDrawing.Text = string.Format("{0} ms", Stopwatch.ElapsedMilliseconds);
        }

        #endregion
        #region SharpDX
        private void btnUsingDirectXDrawing_Click(object sender, RoutedEventArgs e)
        {
            // Begin updating the SurfaceImageSource
            Scenario1Drawing.BeginDraw();

            // Clear background
            Scenario1Drawing.Clear(Colors.Transparent);

            // Create a new pseudo-random number generator
            Random randomGenerator = new Random((int)DateTime.Now.Ticks);
            byte[] pixelValues = new byte[3]; // Represents the red, green, and blue channels of a color

            // Draw 50 random retangles
            for (int i = 0; i < 50; i++)
            {
                // Generate a new random color
                randomGenerator.NextBytes(pixelValues);
                Color color = new Color() { R = pixelValues[0], G = pixelValues[1], B = pixelValues[2], A = 255 };

                // Add a new randomly colored 50x50 rectangle that will fit somewhere within the bounds of the Image1 control
                Scenario1Drawing.FillSolidRect(
                    color,
                    new Rect(randomGenerator.Next((int)cnUsingDirectXs.Width - 50), randomGenerator.Next((int)cnUsingDirectXs.Height - 50), 50, 50)
                    );
            }

            // Stop updating the SurfaceImageSource and draw its contents
            Scenario1Drawing.EndDraw();
        }

        #endregion

        #region 手寫資料模擬
        private void 產生手寫軌跡的測試資料()
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            int 最多手寫次數 = random.Next(5, 20);

            for (int hi = 0; hi < 最多手寫次數; hi++)
            {
                手寫物件軌跡 手寫物件軌跡 = new 手寫物件軌跡();
                int 每次手寫軌跡資料數 = random.Next(100, 1500);
                for (int hj = 0; hj < 每次手寫軌跡資料數; hj++)
                {
                    手寫物件 手寫物件 = new 手寫物件();

                    手寫物件軌跡.手寫物件s.Add(手寫物件);
                }
                頁面手寫物件軌跡_左.手寫物件軌跡s.Add(手寫物件軌跡);
            }


            //foreach (var item in 頁面手寫物件軌跡_左.手寫物件軌跡s)
            //{
            //    foreach (var item1 in item.手寫物件s)
            //    {
            //        Line line = new Line()
            //        {
            //            X1 = item1.X1,
            //            X2 = item1.X2,
            //            Y1 = item1.Y1,
            //            Y2 = item1.Y2,
            //            StrokeThickness = item.StrokeThickness,
            //            Stroke = item.取得手寫物件調色盤的實際SolidColorBrush(),
            //        };
            //        this.canvas右邊手寫畫板.Children.Add(line);

            //    }
            //}
        }

        #endregion

        private void btnHandwriteDrawing_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BasicPage1));
        }

        private void btnUsingXAMLHandwriteDrawing_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BasicPage2));
        }

    }
}
