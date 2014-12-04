using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace MultiFinger
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        uint numActiveContacts;
        Dictionary<uint, Windows.UI.Input.PointerPoint> contacts;
        Windows.Devices.Input.TouchCapabilities SupportedContacts = new Windows.Devices.Input.TouchCapabilities();

        public MainPage()
        {
            this.InitializeComponent();

            contacts = new Dictionary<uint, Windows.UI.Input.PointerPoint>((int)SupportedContacts.Contacts);
            numActiveContacts = 0;
        }

        private void Grid_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {
            
            Debug.WriteLine("ManipulationStarting");
        }

        private void Grid_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            
            Debug.WriteLine("ManipulationStarted");
        }

        private void Grid_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Debug.WriteLine("ManipulationCompleted");
        }
        
        private void rect觸控測試區_ManipulationInertiaStarting(object sender, ManipulationInertiaStartingRoutedEventArgs e)
        {
            Debug.WriteLine("ManipulationInertiaStarting");
        }

        private void rect觸控測試區_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            Debug.WriteLine("ManipulationDelta");

            if (e.Cumulative.Translation.X > 150)
            {
                Refresh手勢操作結果("向右滑動");
                e.Handled = true;
            }
            else if (e.Cumulative.Translation.X < -150)
            {
                Refresh手勢操作結果("向左滑動");
                e.Handled = true;
            }
            else if (e.Cumulative.Translation.Y > 150)
            {
                Refresh手勢操作結果("向下滑動");
                e.Handled = true;
            }
            else if (e.Cumulative.Translation.Y < -150)
            {
                Refresh手勢操作結果("向上滑動");
                e.Handled = true;
            }
        }

        private void Rectangle_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(rect觸控測試區);

            // Check if pointer already exists (if enter occurred prior to down).
            if (contacts.ContainsKey(pt.PointerId)==false)
            {
                contacts[pt.PointerId] = pt;
                ++numActiveContacts;
                Refresh手勢操作結果();
            }
        }

        private void Rectangle_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(rect觸控測試區);

            if (contacts.ContainsKey(pt.PointerId))
            {
                contacts[pt.PointerId] = null;
                contacts.Remove(pt.PointerId);
                --numActiveContacts;
                Refresh手勢操作結果();
            }
        }

        private void Rectangle_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(rect觸控測試區);

            if (contacts.ContainsKey(pt.PointerId))
            {
                contacts[pt.PointerId] = null;
                contacts.Remove(pt.PointerId);
                --numActiveContacts;
                Refresh手勢操作結果();
            }
        }

        private void Rectangle_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(rect觸控測試區);

            if (contacts.ContainsKey(pt.PointerId))
            {
                contacts[pt.PointerId] = null;
                contacts.Remove(pt.PointerId);
                --numActiveContacts;
                Refresh手勢操作結果();
            }
        }

        private void Rectangle_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(rect觸控測試區);

            // Check if pointer already exists (if enter occurred prior to down).
            if (contacts.ContainsKey(pt.PointerId) == false)
            {
                contacts[pt.PointerId] = pt;
                ++numActiveContacts;
                Refresh手勢操作結果();
            }
        }

        private void Rectangle_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(rect觸控測試區);

            if (contacts.ContainsKey(pt.PointerId))
            {
                contacts[pt.PointerId] = null;
                contacts.Remove(pt.PointerId);
                --numActiveContacts;
                Refresh手勢操作結果();
            }
        }

        private void Rectangle_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            //Debug.WriteLine("PointerMoved");
        }

        private void Refresh手勢操作結果(string para手勢操作結果="")
        {
            if (contacts.Count == 0)
            {
                tbk手勢結果.Text = "沒有手勢操作";
            }
            else
            {
                tbk手勢結果.Text = string.Format("{0}指手勢操作  {1}", contacts.Count, para手勢操作結果);
            }
        }
    }
}
