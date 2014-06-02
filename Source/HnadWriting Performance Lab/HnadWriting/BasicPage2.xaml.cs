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
using Windows.UI.Xaml.Shapes;

// 基本頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234237

namespace HnadWriting
{
    /// <summary>
    /// 提供大部分應用程式共通特性的基本頁面。
    /// </summary>
    public sealed partial class BasicPage2 : Page
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
        double IntX = 0;
        double IntY = 0;
        #region SharpDX使用
        private ImageBrush d2dBrush;
        private DeviceManager deviceManager;
        private SurfaceImageSourceTarget d2dTarget;
        private 頁面手寫物件軌跡ShapeRender 頁面手寫物件軌跡ShapeRender_左;

        int totalDrawing = 0;
        DateTime 最後繪圖時間 = DateTime.Now;

        DispatcherTimer _timer = new DispatcherTimer();
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

        public BasicPage2()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            this.正在手寫中 = false;
            //_timer.Tick += _timer_Tick;
            //_timer.Interval = TimeSpan.FromMilliseconds(2000);
            //_timer.Start();
            使用中的繪圖工具.線條粗細 = 3;
            使用中的繪圖工具.手或筆與橡皮擦 = 手或筆與橡皮擦.筆;
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

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);

            this.canvas左邊手寫畫板.PointerMoved -= canvas左邊手寫畫板_PointerMoved;
            this.canvas左邊手寫畫板.PointerReleased -= canvas左邊手寫畫板_PointerReleased;
            this.canvas左邊手寫畫板.PointerPressed -= canvas左邊手寫畫板_PointerPressed;
            this.canvas左邊手寫畫板.PointerExited -= canvas左邊手寫畫板_PointerExited;
            this.canvas左邊手寫畫板.PointerCaptureLost -= canvas左邊手寫畫板_PointerCaptureLost;
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

            if (this.使用中的繪圖工具.手或筆與橡皮擦 == 手或筆與橡皮擦.橡皮擦)
            {
                手寫物件軌跡_左.手寫物件調色盤 = 手寫物件調色盤.img_color_g;
            }

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
                    Line line = null;
                    if (this.使用中的繪圖工具.手或筆與橡皮擦 == 手或筆與橡皮擦.筆)
                    {
                        line = new Line()
                        {
                            X1 = X1,
                            Y1 = Y1,
                            X2 = X2,
                            Y2 = Y2,
                            StrokeThickness = 使用中的繪圖工具.線條粗細,
                            Stroke = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
                            Fill = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
                        };
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
                        line = new Line()
                        {
                            X1 = X1,
                            Y1 = Y1,
                            X2 = X2,
                            Y2 = Y2,
                            StrokeThickness = 使用中的繪圖工具.線條粗細,
                            Stroke = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
                            Fill = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
                            Tag = "Erase",
                        };
                        檢查有交叉者要隱藏(line);
                        line = null;
                        //手寫物件軌跡_左.手寫物件s.Add(new 手寫物件()
                        //{
                        //    X1 = X1,
                        //    X2 = X2,
                        //    Y1 = Y1,
                        //    Y2 = Y2,
                        //});
                    }
                    PreviousContactPoint = CurrentContactPoint;

                    if (line != null)
                    {
                        this.canvas左邊手寫畫板.Children.Add(line);
                    }
                    //if ((DateTime.Now-最後繪圖時間).TotalSeconds >= 1.0)
                    //{
                    //    Debug.WriteLine(最後繪圖時間);
                    //    最後繪圖時間 = DateTime.Now;
                    //    更新SharpDx面板();
                    //}
                }
                PreviousContactPoint = CurrentContactPoint;
            }
        }

        private void canvas左邊手寫畫板_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            TouchID = 0;
            PenID = 0;
            e.Handled = true;
            this.正在手寫中 = false;
            totalDrawing = 0;
        }

        private void canvas左邊手寫畫板_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            this.正在手寫中 = false;
            totalDrawing = 0;
        }

        private void canvas左邊手寫畫板_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.正在手寫中 = false;
            Debug.WriteLine("canvas左邊手寫畫板_PointerExited");
        }

        #endregion

        void CompositionTarget_Rendering(object sender, object e)
        {
            d2dTarget.RenderAll();
        }

        #endregion

        #region 其他方法
        private double Distance(double x1, double y1, double x2, double y2)
        {
            double d = 0;
            d = Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return d;
        }

        #endregion

        private void btnUsing筆Drawing_Click(object sender, RoutedEventArgs e)
        {
            使用中的繪圖工具.手或筆與橡皮擦 = 手或筆與橡皮擦.筆;
        }

        private void btnUsing橡皮擦Drawing_Click(object sender, RoutedEventArgs e)
        {
            使用中的繪圖工具.手或筆與橡皮擦 = 手或筆與橡皮擦.橡皮擦;
        }

        private void btnUsingTestDrawing_Click(object sender, RoutedEventArgs e)
        {
            手寫物件軌跡_左.手寫物件調色盤 = 手寫物件調色盤.img_color_black;
            X1 = 167.936;
            Y1 = 743.3821;
            X2 = 167.936;
            Y2 = 741.3696;
            Line line1 = new Line()
            {
                X1 = X1,
                Y1 = Y1,
                X2 = X2,
                Y2 = Y2,
                StrokeThickness = 使用中的繪圖工具.線條粗細,
                Stroke = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
                Fill = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
            };
            this.canvas左邊手寫畫板.Children.Add(line1);
            手寫物件軌跡_左.手寫物件調色盤 = 手寫物件調色盤.img_color_r;
            X1 = 167.936;
            Y1 = 731.0748;
            X2 = 167.936;
            Y2 = 737.2672;
            Line line2 = new Line()
                       {
                           X1 = X1,
                           Y1 = Y1,
                           X2 = X2,
                           Y2 = Y2,
                           StrokeThickness = 使用中的繪圖工具.線條粗細,
                           Stroke = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
                           Fill = 手寫物件軌跡_左.取得手寫物件調色盤的實際SolidColorBrush(),
                       };
            this.canvas左邊手寫畫板.Children.Add(line2);

            Segment seg1 = new Segment();
            Segment seg2 = new Segment();
            seg1.Start = new Point(line1.X1, line1.Y1);
            seg1.End = new Point(line1.X2, line1.Y2);
            seg2.Start = new Point(line2.X1, line2.Y1);
            seg2.End = new Point(line2.X2, line2.Y2);
            var xx = Intersects(seg1, seg2);
            var yy=  lineSegmentIntersection(seg1.Start.X, seg1.Start.Y, seg1.End.X,seg1.End.Y, seg2.Start.X,seg2.Start.Y,seg2.End.X,seg2.End.Y, ref IntX, ref IntY);
        }

        #region 按鈕事件

        #endregion

        public void 檢查有交叉者要隱藏(Point a, Point b)
        {
            Line xline = null;
            bool flag = false;
            string ss = "";
            foreach (var item in this.canvas左邊手寫畫板.Children)
            {
                xline = (Line)item;
                if (xline.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    if (((string)xline.Tag) == "Erase")
                    {

                    }
                    else
                    {
                        flag = IsIntersecting(a, b, new Point(xline.X1, Y1), new Point(xline.X2, xline.Y2));
                        if (flag == true)
                        {
                            xline.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        public void 檢查有交叉者要隱藏(Line line1)
        {

            Line xline = null;

            bool flag = false;
            Segment seg1 = new Segment();
            Segment seg2 = new Segment();

            seg1.Start = new Point(line1.X1, line1.Y1);
            seg1.End = new Point(line1.X2, line1.Y2);
            foreach (var item in this.canvas左邊手寫畫板.Children)
            {
                xline = (Line)item;
                if (xline.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    if (((string)xline.Tag) == null)
                    {

                        seg2.Start = new Point(xline.X1, xline.Y1);
                        seg2.End = new Point(xline.X2, xline.Y2);
                        var xx = Intersects(seg1, seg2);
                        var yy = lineSegmentIntersection(seg1.Start.X, seg1.Start.Y, seg1.End.X, seg1.End.Y, seg2.Start.X, seg2.Start.Y, seg2.End.X, seg2.End.Y, ref IntX, ref IntY);
                        if (yy==true)
                        {
                            xline.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                            //break;

                            Debug.WriteLine("   {0} - {1}", seg1.Start, seg1.End);
                            Debug.WriteLine("-->{0} - {1}", seg2.Start, seg2.End);
                        }
                    }
                }
            }
        }





        public struct Segment
        {
            public Point Start;
            public Point End;
        }

        public Point? Intersects(Segment AB, Segment CD)
        {
            double deltaACy = AB.Start.Y - CD.Start.Y;
            double deltaDCx = CD.End.X - CD.Start.X;
            double deltaACx = AB.Start.X - CD.Start.X;
            double deltaDCy = CD.End.Y - CD.Start.Y;
            double deltaBAx = AB.End.X - AB.Start.X;
            double deltaBAy = AB.End.Y - AB.Start.Y;

            double denominator = deltaBAx * deltaDCy - deltaBAy * deltaDCx;
            double numerator = deltaACy * deltaDCx - deltaACx * deltaDCy;

            if (denominator == 0)
            {
                if (numerator == 0)
                {
                    // collinear. Potentially infinite intersection points.
                    // Check and return one of them.
                    if (AB.Start.X >= CD.Start.X && AB.Start.X <= CD.End.X)
                    {
                        return AB.Start;
                    }
                    else if (CD.Start.X >= AB.Start.X && CD.Start.X <= AB.End.X)
                    {
                        return CD.Start;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                { // parallel
                    return null;
                }
            }

            double r = numerator / denominator;
            if (r < 0 || r > 1)
            {
                return null;
            }

            double s = (deltaACy * deltaBAx - deltaACx * deltaBAy) / denominator;
            if (s < 0 || s > 1)
            {
                return null;
            }

            return new Point((float)(AB.Start.X + r * deltaBAx), (float)(AB.Start.Y + r * deltaBAy));
        }









        /// <summary>
        /// 判断直线2的两点是否在直线1的两边。
        /// </summary>
        /// <param name="line1">直线1</param>
        /// <param name="line2">直线2</param>
        /// <returns></returns>
        private bool CheckCrose(Line line1, Line line2)
        {
            Point v1 = new Point();
            Point v2 = new Point();
            Point v3 = new Point();

            v1.X = line2.X1 - line1.X2;
            v1.Y = line2.Y1 - line1.Y2;

            v2.X = line2.X2 - line1.X2;
            v2.Y = line2.Y2 - line1.Y2;

            v3.X = line1.X1 - line1.X2;
            v3.Y = line1.Y1 - line1.Y2;

            return (CrossMul(v1, v3) * CrossMul(v2, v3) <= 0);

        }
        /// <summary>
        /// 判断两条线段是否相交。
        /// </summary>
        /// <param name="line1">线段1</param>
        /// <param name="line2">线段2</param>
        /// <returns>相交返回真，否则返回假。</returns>
        private bool CheckTwoLineCrose(Line line1, Line line2)
        {
            return CheckCrose(line1, line2) && CheckCrose(line2, line1);
        }
        /// <summary>
        /// 计算两个向量的叉乘。
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        private double CrossMul(Point pt1, Point pt2)
        {
            return pt1.X * pt2.Y - pt1.Y * pt2.X;
        }

        bool IsIntersecting(Point a, Point b, Point c, Point d)
        {
            double denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            double numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            double numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            double r = numerator1 / denominator;
            double s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }











        public bool lineSegmentIntersection(double Ax, double Ay, double Bx, double By, double Cx, double Cy, double Dx, double Dy, ref double X, ref double Y)
        {

            double distAB, theCos, theSin, newX, ABpos;

            //  Fail if either line segment is zero-length.
            if (Ax == Bx && Ay == By || Cx == Dx && Cy == Dy)
            {
                if ((Cx == Dx) && (Cx >= Ax && Dx <= Bx) && (Cy == Dy && Ay == Cy))
                {
                    X = Cx;
                    Y = Cy;
                    return true;
                }
                if ((Ax == Bx) && (Ax >= Cx && Bx <= Dx) && (Ay == By && Cy == Ay))
                {
                    X = Ax;
                    Y = Ay;
                    return true;
                }
                return false;
            }

            //------custom--|-|-----------------------------//end of one line on the other line

            bool IsVertical = false;

            if (IsPointOnLineSegment(Ax, Ay, Cx, Cy, Dx, Dy, ref IsVertical))
            {
                if (IsVertical)
                {
                    if (Ax == Bx)
                    {
                        X = Cx;//D
                        Y = Cy;
                    }
                    else
                    {
                        X = Ax;
                        Y = Ay;
                    }

                }
                else
                {
                    X = Ax; Y = Ay;
                }
                return true;
            }
            if (IsPointOnLineSegment(Bx, By, Cx, Cy, Dx, Dy, ref IsVertical))
            {
                if (IsVertical)
                {
                    if (Ax == Bx)
                    {
                        X = Dx; //C
                        Y = Dy;
                    }
                    else
                    {
                        X = Bx;
                        Y = By;
                    }
                }
                else
                {
                    X = Bx; Y = By;
                }
                return true;
            }
            if (IsPointOnLineSegment(Cx, Cy, Ax, Ay, Bx, By, ref IsVertical))
            {
                if (IsVertical)
                {
                    X = Cx;
                    Y = Cy;
                }
                else
                {
                    X = Cx; Y = Cy;
                }
                return true;
            }
            if (IsPointOnLineSegment(Dx, Dy, Ax, Ay, Bx, By, ref IsVertical))
            {
                if (IsVertical)
                {
                    X = Dx;
                    Y = Dy;
                }
                else
                {
                    X = Dx; Y = Dy;
                }
                return true;
            }


            //------------------------------------------------
            //  Fail if the segments share an end-point.
            if (Ax == Cx && Ay == Cy || Bx == Cx && By == Cy
            || Ax == Dx && Ay == Dy || Bx == Dx && By == Dy)
            {
                return false;
            }

            //  (1) Translate the system so that point A is on the origin.
            Bx -= Ax; By -= Ay;
            Cx -= Ax; Cy -= Ay;
            Dx -= Ax; Dy -= Ay;

            //  Discover the length of segment A-B.
            distAB = Math.Sqrt(Bx * Bx + By * By);

            //  (2) Rotate the system so that point B is on the positive X axis.
            theCos = Bx / distAB;
            theSin = By / distAB;
            newX = Cx * theCos + Cy * theSin;
            Cy = Cy * theCos - Cx * theSin; Cx = newX;
            newX = Dx * theCos + Dy * theSin;
            Dy = Dy * theCos - Dx * theSin; Dx = newX;

            //  Fail if segment C-D doesn't cross line A-B.
            if (Cy < 0 && Dy < 0 || Cy >= 0 && Dy >= 0) return false;

            //  (3) Discover the position of the intersection point along line A-B.
            ABpos = Dx + (Cx - Dx) * Dy / (Dy - Cy);

            //  Fail if segment C-D crosses line A-B outside of segment A-B.
            if (ABpos < 0 || ABpos > distAB) return false;

            //  (4) Apply the discovered position to line A-B in the original coordinate system.
            X = Math.Round((Ax + ABpos * theCos), 3);
            Y = Math.Round((Ay + ABpos * theSin), 3);

            //  Success.
            return true;
        }


        public static bool IsPointOnLineSegment(double Px, double Py, double Ax, double Ay, double Bx, double By, ref bool IsVertical)
        {
            double least = 0;
            if (Px < least) least = Px;
            if (Ax < least) least = Ax;
            if (Bx < least) least = Bx;

            if (least < 0)
            {
                Px += Math.Abs(least);
                Ax += Math.Abs(least);
                Bx += Math.Abs(least);
            }

            if (!(Ax <= Px && Bx >= Px)) return false;
            if (Bx == Ax) //vertical line, slope = infinity
            {
                IsVertical = true;
                if ((Px == Ax && (Py >= Ay && Py <= By)) || (Px == Ax && (Py >= By && Py <= Ay)))
                {
                    return true;
                }

                return false;

            }
            double S = (By - Ay) / (Bx - Ax);//S=0 horizontal line
            double Y = Ay - (S * Ax);

            if (Math.Abs(Py - (S * Px + Y)) < 0.0009) return true;   //change the precision you want

            return false;
        }




    }

}
