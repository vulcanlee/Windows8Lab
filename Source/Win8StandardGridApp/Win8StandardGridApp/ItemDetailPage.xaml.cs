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

// 項目詳細資料頁面項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234232

namespace Win8StandardGridApp
{
    /// <summary>
    /// 顯示群組內單一項目之詳細資料的頁面，同時允許筆勢
    /// 經由屬於相同群組的其他項目翻轉。
    /// </summary>
    public sealed partial class ItemDetailPage : Win8StandardGridApp.Common.LayoutAwarePage
    {
        public ItemDetailPage()
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
            // 允許儲存的頁面狀態覆寫要顯示的初始項目
            if (pageState != null && pageState.ContainsKey("SelectedItem"))
            {
                navigationParameter = pageState["SelectedItem"];
            }

            // TODO: 為您的問題領域建立適合的資料模型，以取代資料範例
            var item = SampleDataSource.GetItem((String)navigationParameter);
            this.DefaultViewModel["Group"] = item.Group;
            this.DefaultViewModel["Items"] = item.Group.Items;
            this.flipView.SelectedItem = item;
        }

        /// <summary>
        /// 在應用程式暫停或從巡覽快取中捨棄頁面時，
        /// 保留與這個頁面關聯的狀態。值必須符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化需求。
        /// </summary>
        /// <param name="pageState">即將以可序列化狀態填入的空白字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            var selectedItem = (SampleDataItem)this.flipView.SelectedItem;
            pageState["SelectedItem"] = selectedItem.UniqueId;
        }
    }
}
