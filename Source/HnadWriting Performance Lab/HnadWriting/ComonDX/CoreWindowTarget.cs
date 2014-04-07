using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace HnadWriting.ComonDX
{
    /// <summary>
    /// Target to render to a <see cref="CoreWindow"/>
    /// </summary>
    public class CoreWindowTarget : SwapChainTargetBase
    {
        protected CoreWindow window;

        /// <summary>
        /// Initialzies a new <see cref="CoreWindowTarget"/> instance.
        /// </summary>
        /// <param name="window"></param>
        public CoreWindowTarget(CoreWindow window)
        {
            this.window = window;

            // Register event on Window Size Changed
            // So that resources dependent size can be resized
            window.SizeChanged += window_SizeChanged;
        }

        protected override Windows.Foundation.Rect CurrentControlBounds
        {
            get { return window.Bounds; }
        }

        protected override int Width
        {
            get
            {
                return 0; // Returns 0 to fill the CoreWindow 
            }
        }

        protected override int Height
        {
            get
            {
                return 0; // Returns 0 to fill the CoreWindow 
            }
        }

        protected override SharpDX.DXGI.SwapChain1 CreateSwapChain(SharpDX.DXGI.Factory2 factory, SharpDX.Direct3D11.Device1 device, SharpDX.DXGI.SwapChainDescription1 desc)
        {
            // Creates a SwapChain from a CoreWindow pointer
            using (var comWindow = new ComObject(window))
                return factory.CreateSwapChainForCoreWindow(device, comWindow, ref desc, null);
        }

        private void window_SizeChanged(CoreWindow sender, WindowSizeChangedEventArgs args)
        {
            UpdateForSizeChange();
        }
    }
}
