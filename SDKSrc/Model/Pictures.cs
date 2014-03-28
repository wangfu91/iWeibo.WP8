
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace TencentWeiboSDK.Model
{
    [DataContract]
    public class Pictures
    {
        [DataMember(Name="info")]
        public List<PicInfo> Info { get; set; }
    }

    [DataContract]
    public class PicInfo
    {
        [DataMember(Name="pic_XDPI")]
        public List<int> Pic_XDPI{ get; set; }

        [DataMember(Name="pic_YDPI")]
        public List<int> Pic_YDPI{ get; set; }

        [DataMember(Name="pic_height")]
        public List<int> Pic_height{ get; set; }

        [DataMember(Name="pic_size")]
        public List<int> Pic_size{ get; set; }

        [DataMember(Name="pic_type")]
        public List<int> Pic_type { get; set; }

        [DataMember(Name="pic_width")]
        public List<int> Pic_width { get; set; }

        [DataMember(Name="url")]
        public List<string> Url{ get; set; }

    }
}
