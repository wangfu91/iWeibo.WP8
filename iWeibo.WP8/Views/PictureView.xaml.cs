using Coding4Fun.Toolkit.Controls;
using ImageTools.Controls;
using ImageTools.IO.Gif;
using iWeibo.Utils;
using iWeibo.WP8.ViewModels;
using Microsoft.Phone;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Linq;
using System.Threading.Tasks;
using iWeibo.WP8.Resources;
using WeiboSdk.Models;
using System.Collections.Generic;
using TencentWeiboSDK.Model;
using System.Net.Http;
using System.IO.IsolatedStorage;

namespace iWeibo.WP8.Views
{
    public partial class PictureView : PhoneApplicationPage
    {
        private int selectedIndex = 0;
        private int initialIndex = 0;
        private string from = "";
        private List<string> largePicUrls;
        public PictureView()
        {
            InitializeComponent();

            // 用于本地化 ApplicationBar 的示例代码
            BuildLocalizedApplicationBar();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            initialIndex = this.NavigationContext.QueryString.ContainsKey("index") ? int.Parse(this.NavigationContext.QueryString["index"]) : 0;
            from = this.NavigationContext.QueryString.ContainsKey("from") ? this.NavigationContext.QueryString["from"] : "";

        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            object obj;
            if (PhoneApplicationService.Current.State.TryGetValue("PicUrls", out obj))
            {
                //List<PicUrl> sinaPicUrls=new List<PicUrl>();
                //List<PicInfo> tencnetPicUrls=new List<PicInfo>();

                if (from == "sina")
                {
                    var sinaPicUrls = obj as List<PicUrl>;
                    var urls = from picUrl in sinaPicUrls
                               select picUrl.ThumbnailPic.Replace("thumbnail", "large");
                    largePicUrls = urls.ToList(); 

                    //if(sinaPicUrls[selectedIndex].IsGif)
                    //{
                    //    this.staticImage.Visibility = Visibility.Collapsed;
                    //    this.animationImage.Visibility = Visibility.Visible;
                    //}

                }
                else
                {
                    var tencnetPicUrls = obj as List<PicInfo>;
                    var urls = from picUrl in tencnetPicUrls
                               select picUrl.PicUrl + @"/2000";
                    largePicUrls = urls.ToList();
                }
                this.slideView.ItemsSource = largePicUrls;
                this.slideView.SelectedItem = largePicUrls[this.initialIndex];
                HandleSelectionChange();
            }
        }

        private void slideView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HandleSelectionChange();
        }

        private void HandleSelectionChange()
        {
            var index = this.largePicUrls.FindIndex(t =>
                {
                    return t == this.slideView.SelectedItem.ToString() ? true : false;
                });
            this.selectedIndex = index;

            this.indexTextBox.Text = (index + 1).ToString();
        }


        //private void SavePicture()
        //{
            //var bmp = new BitmapImage();
            //bmp.UriSource = new Uri(this.slideView.SelectedItem.ToString(), UriKind.RelativeOrAbsolute);
            //var source = new TaskCompletionSource<object>();
            //bmp.ImageOpened += (o, e) => source.SetResult(e.OriginalSource);

            //var tmp = await source.Task;

            //var tmp2 = tmp as BitmapImage;

            //var wbmp = new WriteableBitmap(bmp);

            //ToastPrompt toast = new ToastPrompt();
            //try
            //{
            //    string picName = DateTime.Now.ToString("iWeibo_Saved_MMddhhmmss") + ".jpg";

            //    using (var ms = new MemoryStream())
            //    {
            //        // 将WriteableBitmap转换为JPEG流编码，并储存到独立存储里.
            //        Extensions.SaveJpeg(wbmp, (Stream)ms, wbmp.PixelWidth, wbmp.PixelHeight, 0, 100);
            //        ms.Seek(0L, SeekOrigin.Begin);
            //        var library = new MediaLibrary();
            //        //把图片加在WP7 手机的媒体库.
            //        library.SavePicture(picName, (Stream)ms);

            //        toast.Message = "保存成功...";
            //        toast.Show();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    toast.Title = "保存失败";
            //    toast.Message = ex.Message;
            //    toast.Show();
            //}

        //}


        public async void SavePicture(string imageurl)
        {
            var toast = new ToastPrompt();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imageurl);
                response.EnsureSuccessStatusCode();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    if (stream.CanSeek)
                        stream.Seek(0, SeekOrigin.Begin);
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);

                    var wbmp = new WriteableBitmap(bitmapImage);

                    string picName = DateTime.Now.ToString("iWeibo_Saved_MMddhhmmss") + ".jpg";

                    using (var ms = new MemoryStream())
                    {
                        // 将WriteableBitmap转换为JPEG流编码，并储存到独立存储里.
                        Extensions.SaveJpeg(wbmp, (Stream)ms, wbmp.PixelWidth, wbmp.PixelHeight, 0, 100);
                        ms.Seek(0L, SeekOrigin.Begin);
                        var library = new MediaLibrary();
                        //把图片加在WP7 手机的媒体库.
                        library.SavePicture(picName, (Stream)ms);

                        toast.Message = "保存成功...";
                        toast.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                toast.Title = "保存失败";
                toast.Message = ex.Message;
                toast.Show();
            }

        }



        //用于生成本地化 ApplicationBar 的示例代码
        private void BuildLocalizedApplicationBar()
        {
            // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
            ApplicationBar = new ApplicationBar();
            ApplicationBar.Mode = ApplicationBarMode.Minimized;

            // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
            ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.save.png", UriKind.Relative));
            appBarButton.Text = AppResources.SaveText;
            ApplicationBar.Buttons.Add(appBarButton);

            appBarButton.Click += appBarButton_Click;

            // 使用 AppResources 中的本地化字符串创建新菜单项。
            //ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.SaveText);
            //ApplicationBar.MenuItems.Add(appBarMenuItem);
        }

        void appBarButton_Click(object sender, EventArgs e)
        {
            SavePicture(largePicUrls[selectedIndex]);
        }

    }
}