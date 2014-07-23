using Coding4Fun.Toolkit.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Xna.Framework.Media;
using Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using TencentWeiboSDK.Model;
using WeiboSdk.Models;

namespace iWeibo.WP8.ViewModels
{
    public class ImageViewModel : NotificationObject
    {
        private bool disposed;
        private List<string> picUrls;

        public List<string> PicUrls
        {
            get
            {
                return picUrls;
            }
            set
            {
                if (value != picUrls)
                {
                    picUrls = value;
                    RaisePropertyChanged(() => this.PicUrls);
                }
            }
        }

        private string selectedItem;

        public string SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                if (value != selectedItem)
                {
                    selectedItem = value;
                    RaisePropertyChanged(() => this.SelectedItem);
                    HandleSelectionChange();
                }
            }
        }

        private bool isGif;

        public bool IsGif
        {
            get
            {
                return isGif;
            }
            set
            {
                if (value != isGif)
                {
                    isGif = value;
                    RaisePropertyChanged(() => this.IsGif);
                }
            }
        }


        private int currentIndex;

        public int CurrentIndex
        {
            get
            {
                return currentIndex;
            }
            set
            {
                if (value != currentIndex)
                {
                    currentIndex = value;
                    RaisePropertyChanged(() => this.CurrentIndex);
                }
            }
        }

        public DelegateCommand SaveImageCommand { get; set; }

        public DelegateCommand<string> SelectedChangedCommand { get; set; }


        public ImageViewModel()
        {
            this.SaveImageCommand = new DelegateCommand(SaveImage);
        }

        public void Initialize(IEnumerable obj,int selectedIndex,ServiceProvider provider)
        {

            switch (provider)
            {
                case ServiceProvider.SinaWeibo:
                    var sinaPicUrls = obj as List<PicUrl>;
                    PicUrls = (from picUrl in sinaPicUrls
                               select picUrl.ThumbnailPic.Replace("thumbnail", "large"))
                               .ToList();
                    break;
                case ServiceProvider.TencentWeibo:
                    var tencnetPicUrls = obj as List<PicInfo>;
                    PicUrls = (from picUrl in tencnetPicUrls
                               select (picUrl.PicUrl + @"/2000"))
                               .ToList();
                    break;
                case ServiceProvider.WeChat:
                    break;
            }

            this.SelectedItem = PicUrls[selectedIndex];


        }
        


        private void HandleSelectionChange()
        {
            var index = this.PicUrls.FindIndex(t =>
                {
                    return t == this.SelectedItem ? true : false;
                });

            this.CurrentIndex = index + 1;

            this.IsGif = this.SelectedItem.EndsWith(".gif");
        }

        private async void SaveImage()
        {
            string imgUrl = this.SelectedItem;

            var toast = new ToastPrompt();
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imgUrl);
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

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                this.SelectedItem = string.Empty;
                this.PicUrls = null;
            }

            this.disposed = true;
        }

    }
}
