using System;
using System.Collections.Generic;
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

namespace StaticMemoryLeak
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

        private void btn頁面2_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(BasicPage1));
        }
    }

    public class MainHelper
    {
        public static List<string> StaticDataManager = new List<string>();
        public static List<ShowMaterialSongFilesVM> ShowMaterialSongFilesVM = new List<ShowMaterialSongFilesVM>();
        //public static ShowMaterialSongFilesVM ShowMaterialSongFilesVM = new ShowMaterialSongFilesVM();
    }

    public class ShowMaterialSongFilesVM
    {
        /// <summary>
        /// 單曲編號
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 封面圖(縮圖)
        /// </summary>
        public string ProductCover { get; set; }

        /// <summary>
        /// 樂曲標題
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 音調
        /// </summary>
        public int Pitch { get; set; }

        /// <summary>
        /// 播放模式(目前用不到)
        /// </summary>
        public string Mode { get; set; }

        /// <summary>
        /// 多張樂譜圖(大圖)
        /// </summary>
        public List<ShowProductSheetVM> Sheet { get; set; }

        /// <summary>
        /// 多個音樂檔
        /// </summary>
        public List<ShowProductMusicVM> Music { get; set; }

        /// <summary>
        /// 樂器和分譜圖的對應表(請照回傳資料順序排)
        /// </summary>
        public List<ShowProductScoreMappingVM> ScoreMapping { get; set; }

        /// <summary>
        /// 秒數檔
        /// </summary>
        public string Second { get; set; }

        /// <summary>
        /// Zip檔
        /// </summary>
        public string Zip { get; set; }
    }

    public class ShowProductScoreMappingVM
    {
        /// <summary>
        /// 分部譜-對應樂器ID(為了顯示對應樂器圖)
        /// 請注意：clk001為特例，當InsID=clk001時，則要讀取Sheet.png
        /// </summary>
        public string InsID { get; set; }

        /// <summary>
        /// 分部譜-排序
        /// </summary>
        public int Sort { get; set; }
    }
    public class ShowProductMusicVM
    {
        /// <summary>
        /// 音樂檔-樂器ID(為了顯示對應樂器圖)
        /// </summary>
        public string InsID { get; set; }

        /// <summary>
        /// 音樂檔-預設音量(0~100)
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// 音樂檔-預設Panport(-100~100)
        /// </summary>
        public int Panport { get; set; }

        /// <summary>
        /// 音樂檔-聲道(1=單聲道、2=立體聲)
        /// </summary>
        public int Channel { get; set; }

        /// <summary>
        /// 音樂檔-檔名
        /// </summary>
        public string mp3File { get; set; }

        /// <summary>
        /// 音樂檔-排序
        /// </summary>
        public int Sort { get; set; }
    }
    public class ShowProductSheetVM
    {
        /// <summary>
        /// 樂譜圖檔名
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 樂譜圖排序
        /// </summary>
        public int Sort { get; set; }

    }
}
