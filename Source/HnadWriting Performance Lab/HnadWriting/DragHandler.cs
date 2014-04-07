using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace HnadWriting
{
    /// <summary>
    /// A <see cref="UIElement"/> handler to add drag behavior.
    /// </summary>
    public class DragHandler
    {
        private bool moveD3dCanvas = false;
        private PointerPoint lastPos;
        private CoreCursor lastCursor;
        private UIElement uiElement;

        /// <summary>
        /// Cursor to show when mouse is over the element
        /// </summary>
        public CoreCursor CursorOver { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="DragHandler"/>
        /// </summary>
        /// <param name="uiElement">The <see cref="UIElement"/> to bind this drag handler</param>
        public DragHandler(UIElement uiElement)
        {
            this.uiElement = uiElement;

            uiElement.PointerEntered += uiElement_PointerEntered;
            uiElement.PointerExited += uiElement_PointerExited;
            uiElement.PointerPressed += uiElement_PointerPressed;
            uiElement.PointerReleased += uiElement_PointerReleased;
            uiElement.PointerMoved += uiElement_PointerMoved;

        }

        protected virtual void uiElement_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            moveD3dCanvas = true;
            lastPos = e.GetCurrentPoint(null);
        }

        protected virtual void uiElement_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            moveD3dCanvas = false;
        }

        protected virtual void uiElement_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (moveD3dCanvas)
            {
                var newPosition = e.GetCurrentPoint(null);
                double deltaX = newPosition.Position.X - lastPos.Position.X;
                double deltaY = newPosition.Position.Y - lastPos.Position.Y;

                // Only support CompositeTransform and TranslateTransform
                // Is there any better way to handle this?
                if (uiElement.RenderTransform is CompositeTransform)
                {
                    var compositeTransform = (CompositeTransform)uiElement.RenderTransform;
                    compositeTransform.TranslateX += deltaX;
                    compositeTransform.TranslateY += deltaY;
                }
                else if (uiElement.RenderTransform is TranslateTransform)
                {
                    var translateTransform = (TranslateTransform)uiElement.RenderTransform;
                    translateTransform.X += deltaX;
                    translateTransform.Y += deltaY;
                }

                lastPos = newPosition;
            }
        }

        protected virtual void uiElement_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            lastCursor = Window.Current.CoreWindow.PointerCursor;
            Window.Current.CoreWindow.PointerCursor = CursorOver;
        }

        protected virtual void uiElement_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = lastCursor;
        }
    }
}
