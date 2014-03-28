using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace WeiboSdk
{
    public class UploadPicture
    {
        public UploadPicture(BitmapImage bmp)
        {
            InitPicture(bmp);
        }

        private void InitPicture(BitmapImage bmp)
        {
            string fileName = "sina_chosen_pic.jpg";
            string filePath = GetTmpDir();
            string fullFileName = System.IO.Path.Combine(filePath, fileName);
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(fullFileName))
                {
                    isf.DeleteFile(fullFileName);
                }
                using (IsolatedStorageFileStream stream = isf.CreateFile(fullFileName))
                {
                    Extensions.SaveJpeg(new WriteableBitmap(bmp), stream, bmp.PixelWidth, bmp.PixelHeight, 0, 100);
                }
            };
            FullPathName = fullFileName;
        }

        public string FullPathName { get; private set; }

        public string FileName
        {
            get
            {
                return System.IO.Path.GetFileName(FullPathName);
            }
        }

        public string Extention
        {
            get
            {
                return System.IO.Path.GetExtension(FileName);
            }
        }

        private string GetTmpDir()
        {
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.DirectoryExists(@"\SinaTemp\"))
                {
                    isf.CreateDirectory(@"\SinaTemp");
                }
            }
            return @"\SinaTemp\";
        }
    }
}
