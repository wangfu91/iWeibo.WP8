using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;

namespace TencentWeiboSDK.Services.Util
{
    /// <summary>
    /// UploadPic 用于"发表带图片的微博" API，帮助用户一键上传图片.
    /// </summary>
    public class UploadPic
    {
        /// <summary>
        /// 构造函数.
        /// </summary>
        /// <param name="pic">需要上传的图片对象.</param>
        public UploadPic(BitmapImage pic)
        {
            InitUploadPic(pic);
        }

        private void InitUploadPic(BitmapImage pic)
        {
            string fileName = "tencent_choosen_pic.jpg";
            string filePath = GetAppTempDir();
            string fullFileName = filePath + fileName;
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (file.FileExists(fullFileName))
                {
                    file.DeleteFile(fullFileName);
                }

                IsolatedStorageFileStream stream = file.CreateFile(fullFileName);
                System.Windows.Media.Imaging.Extensions.SaveJpeg(new WriteableBitmap(pic), stream, pic.PixelWidth, pic.PixelHeight, 0, 100);
                stream.Close();
            };

            FullPathName = fullFileName;
        }

        /// <summary>
        /// 获取上传图片的完整路径.
        /// </summary>
        public string FullPathName { get; private set; }

        /// <summary>
        /// 获取上传图片名称.
        /// </summary>
        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(FullPathName);
            }
        }

        /// <summary>
        /// 获取图片的后缀.
        /// </summary>
        public string Extention
        {
            get
            {
                return System.IO.Path.GetExtension(FileName);
            }
        }

        private string GetAppTempDir()
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.DirectoryExists(@"\TencnetTemp\"))
                {
                    file.CreateDirectory(@"\TencentTemp");
                }
            }
            return @"\TencentTemp\";
        }
    }
}
