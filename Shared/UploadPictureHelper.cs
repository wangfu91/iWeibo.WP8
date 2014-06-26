using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Shared
{
    public class UploadPictureHelper
    {

        public string FullPathName { get; set; }

        public string FileName
        {
            get
            {
                return Path.GetFileName(FullPathName);
            }
        }

        public string Extension
        {
            get
            {
                return Path.GetExtension(FileName);
            }
        }

        public UploadPictureHelper(BitmapImage bmp,string from)
        {
            Initlize(bmp,from);
        }

        private void Initlize(BitmapImage bmp,string from)
        {
            string fileName = string.Format("{0}_choosen_pic.jpg",from);
            string filePath = GetAppTempDir();
            string fullFileName = Path.Combine(filePath, fileName);
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isf.FileExists(fullFileName))
                    isf.DeleteFile(fullFileName);

                using (var stream = isf.CreateFile(fullFileName))
                {
                    var wbmp = new WriteableBitmap(bmp);
                    Extensions.SaveJpeg(wbmp, stream, bmp.PixelWidth, bmp.PixelHeight, 0, 80);

                }
            }

            this.FullPathName = fullFileName;
        }

        private string GetAppTempDir()
        {
            using (var isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var dir = @"\Temp";
                if (!isf.DirectoryExists(dir))
                    isf.CreateDirectory(dir);

                return dir;
            }
        }

    }
}
