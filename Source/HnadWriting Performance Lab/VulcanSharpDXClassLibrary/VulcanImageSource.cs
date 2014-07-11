using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using FeatureLevel = SharpDX.Direct3D.FeatureLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace VulcanSharpDXClassLibrary
{
    /// <summary>
    /// 使用 SharpDX 來動態產生 ImageSource 物件
    /// SurfaceImageSource 提供用來繪圖的 DirectX 共用表面，然後將位元組合成應用程式內容。
    /// 
    /// 共用表面是可調整大小的顯示區域，由 XAML 所定義，您可以使用 DirectX 直接在表面繪圖 
    /// (使用 Windows::UI::Xaml::Media::Brush 類型)。
    /// 對於共用表面，您不能控制呼叫來顯示交換鏈結。共用表面的更新會同步處理至 XAML 架構的更新。
    /// 
    /// 交換鏈結本身。這會提供 DirectX 轉譯管線的背景緩衝區，這是完成轉譯目標後用來進行顯示的記憶體區域。
    /// 
    /// 如果想製作靜態影像或在事件驅動間隔中繪製複雜的影像，
    /// 請使用 Windows::UI::Xaml::Media::Imaging::SurfaceImageSource 在共用表面上繪製。
    /// 這個類型會處理可調整大小的 DirectX 繪圖表面。
    /// 
    /// 若要顯示在文件或 UI 元素中，您通常會使用這個類型將影像或紋理製作成點陣圖。
    /// 它不適用於即時互動 (例如高階遊戲)。
    /// 因為 SurfaceImageSource 物件的更新會與 XAML 使用者介面的更新同步，
    /// 這會讓使用者感受延遲的視覺化回饋，例如波動的畫面播放速率或遲緩的即時輸入回應。
    /// 不過，更新的速度仍足以進行動態控制或資料模擬！
    /// 
    /// </summary>
    public sealed class VulcanImageSource : Windows.UI.Xaml.Media.Imaging.SurfaceImageSource
    {
        /// <summary>
        /// 含您不常呼叫的圖形方法, 用來取得和設定開始繪製像素時所需的一組資源
        /// </summary>
        private SharpDX.Direct3D11.Device d3dDevice;
        /// <summary>
        /// 含您不常呼叫的圖形方法, 用來取得和設定開始繪製像素時所需的一組資源
        /// </summary>
        private SharpDX.Direct2D1.Device d2dDevice;
        /// <summary>
        /// 包含您在每個框架呼叫的方法：在緩衝區與檢視及其他資源中載入、變更輸出合併與轉譯器狀態、管理著色器，以及繪製將這些資源在狀態與著色器之間傳遞的結果。
        /// </summary>
        public SharpDX.Direct2D1.DeviceContext d2dContext;
        public SharpDX.Direct2D1.Factory1 d2dFactory;

        /// <summary>
        /// 要繪製區域的寬度
        /// </summary>
        private readonly int width;

        /// <summary>
        /// 要繪製區域的高度
        /// </summary>
        private readonly int height;

        /// <summary>
        /// 用於為所有 Direct2D 基元指定幾何混合模式。
        /// </summary>
        public SharpDX.Direct2D1.PrimitiveBlend PrimitiveBlend = SharpDX.Direct2D1.PrimitiveBlend.Copy;

        public VulcanImageSource(int pixelWidth, int pixelHeight, bool isOpaque)
            : base(pixelWidth, pixelHeight, isOpaque)
        {
            width = pixelWidth;
            height = pixelHeight;

            CreateDeviceResources();

            Application.Current.Suspending += OnSuspending;
        }

        // Initialize hardware-dependent resources.
        // Device-independent resources, such as ID2D1Geometry, are kept on the CPU.
        // Device-dependent resources, such as ID2D1RenderTarget and ID2D1LinearGradientBrush, directly map to resources on the GPU 
        // (when hardware acceleration is available). 
        // Rendering calls are performed by combining vertex and coverage information from a geometry with texturing information produced by the device-dependent resources.
        private void CreateDeviceResources()
        {
            // http://msdn.microsoft.com/zh-tw/library/windows/apps/dn481540.aspx

            // Unlike the original C++ sample, we don't have smart pointers so we need to
            // dispose Direct3D objects explicitly
            Utilities.Dispose(ref d3dDevice);
            Utilities.Dispose(ref d2dDevice);
            Utilities.Dispose(ref d2dContext);
            Utilities.Dispose(ref d2dFactory);

#if DEBUG
            var debugLevel = SharpDX.Direct2D1.DebugLevel.Information;
#else
            var debugLevel = SharpDX.Direct2D1.DebugLevel.None;
#endif
            d2dFactory = new SharpDX.Direct2D1.Factory1(SharpDX.Direct2D1.FactoryType.SingleThreaded);

            // This flag adds support for surfaces with a different color channel ordering
            // than the API default. It is required for compatibility with Direct2D.
            var creationFlags = DeviceCreationFlags.BgraSupport;

#if DEBUG
            // If the project is in a debug build, enable debugging via SDK Layers.
            creationFlags |= DeviceCreationFlags.Debug;
#endif

            // This array defines the set of DirectX hardware feature levels this app will support.
            // Note the ordering should be preserved.
            // Don't forget to declare your application's minimum required feature level in its
            // description.  All applications are assumed to support 9.1 unless otherwise stated.
            FeatureLevel[] featureLevels =
            {
                FeatureLevel.Level_11_1,
                FeatureLevel.Level_11_0,
                FeatureLevel.Level_10_1,
                FeatureLevel.Level_10_0,
                FeatureLevel.Level_9_3,
                FeatureLevel.Level_9_2,
                FeatureLevel.Level_9_1,
            };

            // Create the Direct3D 11 API device object.
            d3dDevice = new Device(DriverType.Hardware, creationFlags, featureLevels);

            // Get the Direct3D 11.1 API device.
            // DXGI : DirectX Graphics Infrastructure
            // DXGI 是一組用來設定和管理低階圖形與圖形卡資源的 API
            // 為了直接存取 GPU 並管理其資源，必須有一個對應用程式描述它的方式。
            // 您所需最重要的 GPU 資訊就是繪製像素的位置，這樣它才能夠將這些像素傳送到螢幕上。
            // 這通常稱為「背景緩衝區」—GPU 記憶體中的一個位置，您可以在該處繪製像素，
            // 然後「翻轉」或「交換」，並在收到重新整理訊號時傳送到螢幕上。
            // DXGI 可讓您取得該位置以及使用該緩衝區 (稱為「交換鏈結」，
            // 因為這是可交換的緩衝區鏈結，允許多個緩衝處理策略) 的方法。
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device>())
            {
                // Create the Direct2D device object and a corresponding context.
                // 是 GPU 資源的虛擬表示法
                // ID3D11Device 包含您不常呼叫的圖形方法，通常是在任何轉譯發生之前呼叫這些方法，用來取得和設定開始繪製像素時所需的一組資源。
                d2dDevice = new SharpDX.Direct2D1.Device(dxgiDevice);

                // 是轉譯管線與處理程序的跨裝置抽象概念
                // ID3D11DeviceContext 則包含您在每個框架呼叫的方法：
                // 在緩衝區與檢視及其他資源中載入、變更輸出合併與轉譯器狀態、管理著色器，
                // 以及繪製將這些資源在狀態與著色器之間傳遞的結果。
                d2dContext = new SharpDX.Direct2D1.DeviceContext(d2dDevice, DeviceContextOptions.EnableMultithreadedOptimizations);

                // Query for ISurfaceImageSourceNative interface.
                using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
                    sisNative.Device = dxgiDevice;
            }
            setPrimitiveBlend();
        }

        public void setPrimitiveBlend()
        {
            d2dContext.PrimitiveBlend = PrimitiveBlend;
        }

        public void BeginDraw()
        {
            // 只會在 Rect 參數中指定的更新區域進行繪圖。
            BeginDraw(new Windows.Foundation.Rect(0, 0, width, height));
        }

        /// <summary>
        /// 使用 DirectX 在該表面上繪圖
        /// 只會在 updateRect 參數中指定的更新區域進行繪圖。
        /// </summary>
        /// <param name="updateRect"></param>
        public void BeginDraw(Windows.Foundation.Rect updateRect)
        {
            // Express target area as a native RECT type.
            var updateRectNative = new Rectangle
            {
                Left = (int)updateRect.Left,
                Top = (int)updateRect.Top,
                Right = (int)updateRect.Right,
                Bottom = (int)updateRect.Bottom
            };

            // Query for ISurfaceImageSourceNative interface.
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
            {
                // Begin drawing - returns a target surface and an offset to use as the top left origin when drawing.
                try
                {
                    Point offset;
                    using (var surface = sisNative.BeginDraw(updateRectNative, out offset))
                    {
                        var bitmapProperties = new SharpDX.Direct2D1.BitmapProperties1(
                            new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied),
                            96,
                            96,
                            SharpDX.Direct2D1.BitmapOptions.Target | SharpDX.Direct2D1.BitmapOptions.CannotDraw);

                        // Create render target.
                        using (var bitmap = new Bitmap1(d2dContext, surface, bitmapProperties))
                        {
                            // Set context's render target.
                            d2dContext.Target = bitmap;
                        }

                        // Begin drawing using D2D context.
                        d2dContext.BeginDraw();

                        // Apply a clip and transform to constrain updates to the target update area.
                        // This is required to ensure coordinates within the target surface remain
                        // consistent by taking into account the offset returned by BeginDraw, and
                        // can also improve performance by optimizing the area that is drawn by D2D.
                        // Apps should always account for the offset output parameter returned by 
                        // BeginDraw, since it may not match the passed updateRect input parameter's location.
                        d2dContext.PushAxisAlignedClip(
                            new RectangleF(
                                (offset.X),
                                (offset.Y),
                                (offset.X + (float)updateRect.Width),
                                (offset.Y + (float)updateRect.Height)
                                ),
                            AntialiasMode.Aliased
                            );

                        d2dContext.Transform = Matrix3x2.Translation(offset.X, offset.Y);
                    }
                }
                catch (SharpDXException ex)
                {
                    if (ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceRemoved ||
                        ex.ResultCode == SharpDX.DXGI.ResultCode.DeviceReset)
                    {
                        // If the device has been removed or reset, attempt to recreate it and continue drawing.
                        CreateDeviceResources();
                        BeginDraw(updateRect);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 完成點陣圖。將這個點陣圖傳遞到 ImageBrush
        /// </summary>
        public void EndDraw()
        {
            // Remove the transform and clip applied in BeginDraw since
            // the target area can change on every update.
            d2dContext.Transform = Matrix3x2.Identity;
            d2dContext.PopAxisAlignedClip();

            // Remove the render target and end drawing.
            d2dContext.EndDraw();

            d2dContext.Target = null;

            // Query for ISurfaceImageSourceNative interface.
            using (var sisNative = ComObject.QueryInterface<ISurfaceImageSourceNative>(this))
                sisNative.EndDraw();
        }

        /// <summary>
        /// 清除要開始繪製前的背景圖
        /// </summary>
        /// <param name="color"></param>
        public void Clear(Windows.UI.Color color)
        {
            d2dContext.Clear(ConvertToColorF(color));
            //d2dContext.Clear(SharpDX.Color.Transparent);
        }

        /// <summary>
        /// 使用 SharpDX 繪製矩形
        /// </summary>
        /// <param name="color">XMAL中用到的顏色列舉值</param>
        /// <param name="rect">矩形大小</param>
        public void FillSolidRect(Windows.UI.Color color, Windows.Foundation.Rect rect)
        {
            // Create a solid color D2D brush.
            using (var brush = new SolidColorBrush(d2dContext, ConvertToColorF(color)))
            {
                // Draw a filled rectangle.
                d2dContext.FillRectangle(ConvertToRectF(rect), brush);
            }
        }

        /// <summary>
        /// 使用 SharpDX 繪製線段
        /// </summary>
        /// <param name="p1x">起始點 X 座標值</param>
        /// <param name="p1y">起始點 Y 座標值</param>
        /// <param name="p2x">結束點 X 座標值</param>
        /// <param name="p2y">結束點 Y 座標值</param>
        /// <param name="brush">XMAL中用到的顏色列舉值</param>
        /// <param name="width">線條寬度</param>
        public void DrawLine(int p1x, int p1y, int p2x, int p2y, SolidColorBrush brush, int width)
        {
            d2dContext.DrawLine(new Vector2(p1x, p1y), new Vector2(p2x, p2y), brush, width);
        }

        /// <summary>
        /// 使用 SharpDX 繪製橢圓形
        /// </summary>
        /// <param name="p1x">中心點 X 座標值</param>
        /// <param name="p1y">中心點 Y 座標值</param>
        /// <param name="p2x">橢圓的水平尺寸</param>
        /// <param name="p2y">橢圓的垂直尺寸</param>
        /// <param name="brush">XMAL中用到的顏色列舉值</param>
        /// <param name="width">橢圓線條寬度</param>
        public void DrawEllipse(float p1x, float p1y, float p2x, float p2y, SolidColorBrush brush, int width)
        {
            Ellipse Ellipse = new SharpDX.Direct2D1.Ellipse(new Vector2(p1x, p1y), p2x, p2y);
            d2dContext.DrawEllipse(Ellipse, brush, width);
        }

        /// <summary>
        /// 使用 SharpDX 繪製橢圓形(有填滿顏色)
        /// </summary>
        /// <param name="p1x">中心點 X 座標值</param>
        /// <param name="p1y">中心點 Y 座標值</param>
        /// <param name="p2x">橢圓的水平尺寸</param>
        /// <param name="p2y">橢圓的垂直尺寸</param>
        /// <param name="brush">XMAL中用到的顏色列舉值</param>
        public void DrawFillEllipse(float p1x, float p1y, float p2x, float p2y, SolidColorBrush brush)
        {
            Ellipse Ellipse = new SharpDX.Direct2D1.Ellipse(new Vector2(p1x, p1y), p2x, p2y);
            d2dContext.FillEllipse(Ellipse, brush);
        }

        public void DrawGeometry(Geometry geometry, SolidColorBrush brush, int width)
        {
            d2dContext.DrawGeometry(geometry, brush, width);
        }

        private void OnSuspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            // Hints to the driver that the app is entering an idle state and that its memory can be used temporarily for other apps.
            using (var dxgiDevice = d3dDevice.QueryInterface<SharpDX.DXGI.Device3>())
                dxgiDevice.Trim();
        }

        /// <summary>
        /// 將 Windows.UI.Color 物件，轉換成為 SharpDX.Color 物件
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        private static Color ConvertToColorF(Windows.UI.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// 將 Windows.UI.Color 轉換成為 SharpDX.Direct2D1,SolidColorBrush
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public SolidColorBrush ConvertToSolidColorBrush(Windows.UI.Color color)
        {
            return new SolidColorBrush(d2dContext, ConvertToColorF(color));
        }

        /// <summary>
        /// 將 Windows.Foundation.Rect 轉換成為 SharpDX.RectangleF
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private static RectangleF ConvertToRectF(Windows.Foundation.Rect rect)
        {
            return new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);
        }
    }
}
