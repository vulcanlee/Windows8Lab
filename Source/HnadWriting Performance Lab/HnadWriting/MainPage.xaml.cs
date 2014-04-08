using HnadWriting.ComonDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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

        頁面手寫物件軌跡 頁面手寫物件軌跡_左 = new 頁面手寫物件軌跡();

        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            產生手寫軌跡的測試資料();

            d2dBrush = new ImageBrush();
            cnUsingGeometries.Background = d2dBrush;

            // Safely dispose any previous instance
            // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
            deviceManager = new DeviceManager();

            shapeRenderer = new ShapeRenderer();

            int pixelWidth = (int)(cnUsingGeometries.Width * DisplayProperties.LogicalDpi / 96.0);
            int pixelHeight = (int)(cnUsingGeometries.Height * DisplayProperties.LogicalDpi / 96.0);

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

        }

        void DisplayProperties_LogicalDpiChanged(object sender)
        {
            deviceManager.Dpi = DisplayProperties.LogicalDpi;
        }

        void CompositionTarget_Rendering(object sender, object e)
        {
            d2dTarget.RenderAll();
        }

        private void btnUsingGeometriesDrawing_Click(object sender, RoutedEventArgs e)
        {
            d2dTarget.RenderAll();
        }

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

    }
}
