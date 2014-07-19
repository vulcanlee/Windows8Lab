using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// 空白頁項目範本已記錄在 http://go.microsoft.com/fwlink/?LinkId=234238

namespace 存取我的文件資料夾
{
    /// <summary>
    /// 可以在本身使用或巡覽至框架內的空白頁面。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void btnTestMyDocument_Click(object sender, RoutedEventArgs e)
        {
            var myDocumentFolder = KnownFolders.DocumentsLibrary;

            var folders = await myDocumentFolder.GetFoldersAsync();

            foreach (var item in folders)
            {
                Debug.WriteLine(item.Name);
            }

            var files = await myDocumentFolder.GetFilesAsync();
            StringBuilder sb = new StringBuilder();
            foreach (var item in files)
            {
                sb.AppendLine(item.DisplayName);
            }
            Debug.WriteLine(sb.ToString());

            var MusicPlayAlong = await myDocumentFolder.GetFolderAsync("MusicPlayAlong");
            var fl = await MusicPlayAlong.CreateFileAsync("vulcan.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(fl, sb.ToString());
        }
    }
}
