using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iWeibo.Utils
{ 
    public enum ImageType
        {
            Null,
            Png,
            Jpg,
            Gif,
            Bmp
        }

    public class ImageFormatResolve
    {
        /// <summary>
        /// 定义图片格式
        /// </summary>


        /// <summary>
        /// 获取图片格式
        /// </summary>
        public static ImageType GetImageType(Stream stream)
        {
            //图片格式
            ImageType type = ImageType.Null;

            //读取图片文件头8个字节，并根据若干个字节来确定图片格式
            byte[] header = new byte[8];
            stream.Read(header, 0, 8);

            //确定图片格式
            if (header[0] == 0x89 &&
                header[1] == 0x50 && // P
                header[2] == 0x4E && // N
                header[3] == 0x47 && // G
                header[4] == 0x0D &&
                header[5] == 0x0A &&
                header[6] == 0x1A &&
                header[7] == 0x0A)
            {
                //Png图片 8字节：89 50 4E 47 0D 0A 1A 0A
                type = ImageType.Png;
            }
            else if (header[0] == 0xFF &&
                    header[1] == 0xD8)
            {
                //Jpg图片 2字节：FF D8
                type = ImageType.Jpg;
            }
            else if (header[0] == 0x47 &&   // G
                    header[1] == 0x49 &&    // I
                    header[2] == 0x46 &&    // F
                    header[3] == 0x38 &&    // 8
                    (header[4] == 0x39 ||   // 9
                    header[4] == 0x37) &&   // 7
                    header[5] == 0x61)      // a
            {
                //Gif图片 6字节：47 49 46 38 39|37 61
                type = ImageType.Gif;
            }
            else if (header[0] == 0x42 &&   //B
                    header[1] == 0x4D)      //M
            {
                //Bmp图片 2字节：42 4D
                type = ImageType.Bmp;
            }

            //关闭字节流
            //stream.Close();

            return type;
        }
    }


}
