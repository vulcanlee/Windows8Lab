using HnadWriting.Common;
using HnadWriting.ComonDX;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace HnadWriting
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class BasicPage1 : Page
    {
        #region 欄位
        public bool 正在手寫中 = false;

        頁面手寫物件軌跡 頁面手寫物件軌跡_左 = new 頁面手寫物件軌跡();
        頁面手寫物件軌跡 頁面手寫物件軌跡_右 = new 頁面手寫物件軌跡();
        手寫物件軌跡 手寫物件軌跡_左 = new 手寫物件軌跡();
        手寫物件軌跡 手寫物件軌跡_右邊 = new 手寫物件軌跡();
        使用中的繪圖工具 使用中的繪圖工具 = new 使用中的繪圖工具();
        uint PenID, TouchID;
        double X1, X2, Y1, Y2, StrokeThickness = 1;
        Point StartPoint, PreviousContactPoint, CurrentContactPoint;

        #region SharpDX使用
        private ImageBrush d2dBrush;
        private DeviceManager deviceManager;
        private SurfaceImageSourceTarget d2dTarget;
        private 頁面手寫物件軌跡ShapeRender 頁面手寫物件軌跡ShapeRender_左;

        int totalDrawing = 0;
        DateTime 最後繪圖時間 = DateTime.Now;

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

        public BasicPage1()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.正在手寫中 = false;
        }

        #region 頁面狀態

        /// <summary>
        /// 巡覽期間以傳遞的內容填入頁面。從之前的工作階段
        /// 重新建立頁面時，也會提供儲存的狀態。
        /// </summary>
        /// <param name="sender">
        /// 事件之來源；通常是<see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">提供傳遞出去之巡覽參數之事件資料
        /// <see cref="Frame.Navigate(Type, Object)"/> 初始要求本頁面時及
        /// 這個頁面在先前的工作階段期間保留的狀態字典
        /// 工作階段。第一次瀏覽頁面時，狀態是 null。</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// 在應用程式暫停或從巡覽快取中捨棄頁面時，
        /// 保留與這個頁面關聯的狀態。值必須符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化需求。
        /// </summary>
        /// <param name="sender">事件之來源；通常是<see cref="NavigationHelper"/></param>
        /// <param name="e">事件資料，此資料提供即將以可序列化狀態填入的空白字典
        ///。</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

            左邊頁面繪圖裝置初始化();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
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
                手寫物件s = new List<手寫物件>(),
            };

            手寫物件軌跡_左.手或筆與橡皮擦 = this.使用中的繪圖工具.手或筆與橡皮擦;

            頁面手寫物件軌跡_左.手寫物件軌跡s.Add(手寫物件軌跡_左);
            PreviousContactPoint = e.GetCurrentPoint(this.canvas左邊手寫畫板).Position;

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
                // Distance() is an application-defined function that tests
                // whether the pointer has moved far enough to justify 
                // drawing a new line.
                CurrentContactPoint = e.GetCurrentPoint(this.canvas左邊手寫畫板).Position;
                X1 = PreviousContactPoint.X;
                Y1 = PreviousContactPoint.Y;
                X2 = CurrentContactPoint.X;
                Y2 = CurrentContactPoint.Y;

                if (Distance(X1, Y1, X2, Y2) > 2.0)
                {
                    if (this.使用中的繪圖工具.手或筆與橡皮擦 == 手或筆與橡皮擦.筆)
                    {
                        手寫物件軌跡_左.手寫物件s.Add(new 手寫物件()
                        {
                            X1 = X1,
                            X2 = X2,
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
                    PreviousContactPoint = CurrentContactPoint;

                    if ((DateTime.Now-最後繪圖時間).TotalSeconds >= 1.0)
                    {
                        最後繪圖時間 = DateTime.Now;
                        更新SharpDx面板();
                    }
                }
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

        #endregion

        #region 其他方法
        private double Distance(double x1, double y1, double x2, double y2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return d;
        }

        public void 左邊頁面繪圖裝置初始化()
        {
            d2dBrush = new ImageBrush();
            canvas左邊手寫畫板.Background = d2dBrush;

            // Safely dispose any previous instance
            // Creates a new DeviceManager (Direct3D, Direct2D, DirectWrite, WIC)
            deviceManager = new DeviceManager();

            頁面手寫物件軌跡ShapeRender_左 = new 頁面手寫物件軌跡ShapeRender();
            頁面手寫物件軌跡ShapeRender_左.頁面手寫物件軌跡 = 頁面手寫物件軌跡_左;

            //Rect cnUsingGeometries = new Rect();
            //cnUsingGeometries.Width = 1024;
            //cnUsingGeometries.Height = 768;
            DisplayInformation DisplayInformation = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            int pixelWidth = (int)(canvas左邊手寫畫板.Width * DisplayInformation.LogicalDpi / 96.0);
            int pixelHeight = (int)(canvas左邊手寫畫板.Height * DisplayInformation.LogicalDpi / 96.0);

            d2dTarget = new SurfaceImageSourceTarget(pixelWidth, pixelHeight);
            d2dBrush.ImageSource = d2dTarget.ImageSource;

            // Add Initializer to device manager
            deviceManager.OnInitialize += d2dTarget.Initialize;
            deviceManager.OnInitialize += 頁面手寫物件軌跡ShapeRender_左.Initialize;

            // Render the cube within the CoreWindow
            d2dTarget.OnRender += 頁面手寫物件軌跡ShapeRender_左.Render;

            // Initialize the device manager and all registered deviceManager.OnInitialize 
            deviceManager.Initialize(DisplayInformation.LogicalDpi);

            // Setup rendering callback
            //CompositionTarget.Rendering += CompositionTarget_Rendering;

            // Callback on DpiChanged
        }

        public void 更新SharpDx面板()
        {
            if (totalDrawing == 0)
            {
                d2dTarget.RenderAll();
                totalDrawing++;
            }
            else if ((totalDrawing % 2) == 0)
            {
                d2dTarget.RenderAll();
                d2dTarget.RenderAll();
                totalDrawing = 1;
            }
            else
            {
                d2dTarget.RenderAll();
                d2dTarget.RenderAll();
                totalDrawing++;
            }
        }
        #endregion

        #region 按鈕事件
        private void btnUsingGeometriesDrawing_Click(object sender, RoutedEventArgs e)
        {
            //手寫物件軌跡 手寫物件軌跡xy = new 手寫物件軌跡()
            //{
            //    手或筆與橡皮擦 = 手或筆與橡皮擦.筆,
            //    StrokeThickness = 3,
            //    手寫物件調色盤 = 手寫物件調色盤.img_color_g,
            //    手寫物件s = new List<手寫物件>(),
            //};
            //手寫物件軌跡xy.手寫物件s.Add(new 手寫物件
            //{
            //    X1 = 10,
            //    Y1 = 10,
            //    X2 = 100,
            //    Y2 = 50
            //});
            //手寫物件軌跡xy.手寫物件s.Add(new 手寫物件
            //{
            //    X1 = 100,
            //    Y1 = 50,
            //    X2 = 300,
            //    Y2 = 450
            //});
            //頁面手寫物件軌跡_左.手寫物件軌跡s.Add(手寫物件軌跡xy);
            更新SharpDx面板();
        }
        #endregion
    }
}
