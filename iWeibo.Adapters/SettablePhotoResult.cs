using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace iWeibo.Adapters
{
    public class SettablePhotoResult : PhotoResult
    {
        public SettablePhotoResult(PhotoResult photoResult)
        {
            ChosenPhoto = photoResult.ChosenPhoto;
            OriginalFileName = photoResult.OriginalFileName;
            Error = photoResult.Error;
        }

        public SettablePhotoResult()
        {
        }

        public new Stream ChosenPhoto { get; set; }
        public new string OriginalFileName { get; set; }
        public new Exception Error { get; set; }
    }
}
