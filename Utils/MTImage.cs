using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using Windows.Storage;

namespace iWeibo.Utils
{
    /// <summary>
    /// 功能：同系统Image控件一同使用，接管下载图片的工作，提供缓存功能，对于缓存中已存在的图片，将使用缓存图片
    /// 用法：作为附加属性写到Image控件内，其中Source属性为真实的网络图片地址，LoadingSource为加载图片过程中占位在Image内的“加载中”图片
    /// </summary>
    public class MTImage
    {
        #region Source Attach Property

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.RegisterAttached("Source", typeof(string), typeof(MTImage), new PropertyMetadata(default(string), SourcePropertyChanged));

        public static void SetSource(Image image, string value)
        {
            image.SetValue(SourceProperty, value);
        }

        public static string GetSource(Image image)
        {
            return (string)image.GetValue(SourceProperty);
        }
        private static void SourcePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs ags)
        {
            if (ags.NewValue == ags.OldValue)
                return;

            var image = dependencyObject as Image;
            if (image != null)
            {
                //获取加载真正图片过程中显示的加载中的图片路径
                SetLoadingImageSource(image);
                var imageurl = ags.NewValue as string;
                SetRealImageSource(image, imageurl);
            }
        }

        #endregion

        #region LoadingSource Attach Property

        public static readonly DependencyProperty LoadingSourceProperty =
            DependencyProperty.RegisterAttached("LoadingSource", typeof(string), typeof(MTImage), new PropertyMetadata(default(string)));

        public static void SetLoadingSource(UIElement element, string value)
        {
            element.SetValue(LoadingSourceProperty, value);
        }

        public static string GetLoadingSource(UIElement element)
        {
            return (string)element.GetValue(LoadingSourceProperty);
        }

        #endregion

        #region Field

        private static readonly IsolatedStorageFile IsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
        private const string CachePath = "ImageCache";

        #endregion

        #region Private Method


        /// <summary>
        /// 设置获取真实图片过程中显示的加载中图片
        /// </summary>
        /// <param name="image"></param>
        private static void SetLoadingImageSource(Image image)
        {
            var loadingImageSource = GetLoadingSource(image);
            if (string.IsNullOrEmpty(loadingImageSource))
            {
                image.Source = null;
            }
            else
            {
                image.Source = new BitmapImage(new Uri(loadingImageSource, UriKind.Relative));
            }
        }

        private static async void SetRealImageSource(Image image, string imageurl)
        {
            if (imageurl == null) return;
            //如果不是网络图片则将图片路径给Image进行展示
            if (!imageurl.StartsWith("http"))
                image.Source = new BitmapImage(new Uri(imageurl, UriKind.RelativeOrAbsolute));
            else
            {
                if (CacheImageExists(imageurl))
                {
                    image.Source = await GetImageSourceFromCache(imageurl);
                }
                else
                {
                    image.Source = await GetImageFromNetWork(imageurl);
                }
                StartStotyboard(image);
            }
        }

        /// <summary>
        /// 为image执行一段渐进的动画
        /// </summary>
        /// <param name="img"></param>
        private static void StartStotyboard(Image img)
        {
            var sb = new Storyboard();
            var anim = new DoubleAnimation { From = 0, To = 1, Duration = TimeSpan.FromMilliseconds(500) };
            Storyboard.SetTarget(anim, img);
            Storyboard.SetTargetProperty(anim, new PropertyPath("Opacity"));
            sb.Children.Add(anim);
            sb.Begin();
        }

        /// <summary>
        /// 获取网络图片
        /// </summary>
        /// <param name="imageurl"></param>
        /// <returns></returns>
        private static async Task<ImageSource> GetImageFromNetWork(string imageurl)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(imageurl);
                response.EnsureSuccessStatusCode();
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    //检测图片缓存文件夹是否存在，不存在则创建
                    if (!IsolatedStorage.DirectoryExists(CachePath))
                    {
                        IsolatedStorage.CreateDirectory(CachePath);
                    }
                    //将下载的图片缓存到独立存储区中
                    using (var isoStream = new IsolatedStorageFileStream(GetCacheFilePath(imageurl), FileMode.Create, IsolatedStorage))
                    {
                        var bytesInStream = new byte[stream.Length];
                        stream.Read(bytesInStream, 0, bytesInStream.Length);
                        isoStream.Write(bytesInStream, 0, bytesInStream.Length);
                    }
                    if (stream.CanSeek)
                        stream.Seek(0, SeekOrigin.Begin);
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(stream);

                    return bitmapImage;
                }
            }
            catch (HttpRequestException)
            {
                return new BitmapImage();
            }
            catch (TaskCanceledException)
            {
                return new BitmapImage();
            }
        }

        /// <summary>
        /// 获取缓存的图片
        /// </summary>
        /// <param name="imageurl"></param>
        /// <returns></returns>
        private static async Task<ImageSource> GetImageSourceFromCache(string imageurl)
        {
            string filePath = GetCacheFilePath(imageurl);
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(filePath);
            using (var stream = await file.OpenStreamForReadAsync())
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.SetSource(stream);
                return bitmapImage;
            }
        }

        /// <summary>
        /// 判断指定名称的缓存文件是否存在
        /// </summary>
        /// <param name="imageurl"></param>
        /// <returns></returns>
        private static bool CacheImageExists(string imageurl)
        {
            return IsolatedStorage.FileExists(GetCacheFilePath(imageurl));
        }

        /// <summary>
        /// 根据文件名称组合缓存文件的路径
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        private static string GetCacheFilePath(string imageUrl)
        {
            var imageName = HttpUtility.UrlEncode(imageUrl);
            return Path.Combine(CachePath, imageName);
        }

        #endregion

        #region Public Method

        /// <summary>
        /// 获取当前图片缓存占用的空间
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetImageCacheSizeAsync()
        {
            var total = await Task.Run(() =>
            {
                long totalsize = 0;
                if (IsolatedStorage.DirectoryExists(CachePath))
                {
                    foreach (var fileName in IsolatedStorage.GetFileNames(CachePath + "/"))
                    {
                        using (var file = IsolatedStorage.OpenFile(CachePath + "/" + fileName, FileMode.Open))
                        {
                            totalsize += file.Length;
                        }
                    }
                }
                return totalsize;
            });
            return SizeSuffix(total);
        }

        /// <summary>
        /// 清理当前的图片缓存
        /// </summary>
        public static Task ClearImageCacheAsync()
        {
            return Task.Run(() =>
            {
                if (IsolatedStorage.DirectoryExists(CachePath))
                {
                    foreach (var file in IsolatedStorage.GetFileNames(CachePath + "/"))
                    {
                        IsolatedStorage.DeleteFile(CachePath + "/" + file);
                    }
                }
            });
        }

        #endregion

        #region Helper Method

        static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        /// <summary>
        /// 将bytes大小的数据转换成友好的显示
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static string SizeSuffix(long value)
        {
            if (value == 0)
                return "0 bytes";
            var mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1 << (mag * 10));

            return string.Format("{0:n1} {1}", adjustedSize, SizeSuffixes[mag]);
        }

        #endregion
    }
}