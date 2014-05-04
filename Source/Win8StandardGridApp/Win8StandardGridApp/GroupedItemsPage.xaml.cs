using Win8StandardGridApp.Data;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 群組項目頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234231

namespace Win8StandardGridApp
{
    /// <summary>
    /// 顯示項目的群組集合之頁面。
    /// </summary>
    public sealed partial class GroupedItemsPage : Win8StandardGridApp.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 巡覽期間以傳遞的內容填入頁面。從之前的工作階段
        /// 重新建立頁面時，也會提供儲存的狀態。
        /// </summary>
        /// <param name="navigationParameter">最初要求這個頁面時，傳遞到
        /// <see cref="Frame.Navigate(Type, Object)"/> 的參數。
        /// </param>
        /// <param name="pageState">這個頁面在先前的工作階段期間保留的
        /// 狀態字典。第一次瀏覽頁面時，這一項是 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: 為您的問題領域建立適合的資料模型，以取代資料範例
            var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// 在按下群組標頭時叫用。
        /// </summary>
        /// <param name="sender">選取的群組用來做為群組標頭的 Button。</param>
        /// <param name="e">描述如何啟始按一下的事件資料。</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // 判斷 Button 執行個體所代表的群組
            var group = (sender as FrameworkElement).DataContext;

            // 巡覽至適用的目的頁面，設定新的頁面
            // 方式是透過傳遞必要資訊做為巡覽參數
            this.Frame.Navigate(typeof(GroupDetailPage), ((SampleDataGroup)group).UniqueId);
        }

        /// <summary>
        /// 在按下群組內的項目時叫用。
        /// </summary>
        /// <param name="sender">GridView (或快照應用程式時為 ListView)
        /// 顯示已按下的項目。</param>
        /// <param name="e">描述已按下之項目的事件資料。</param>
        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // 巡覽至適用的目的頁面，設定新的頁面
            // 方式是透過傳遞必要資訊做為巡覽參數
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            this.Frame.Navigate(typeof(ItemDetailPage), itemId);
        }
    }
}
