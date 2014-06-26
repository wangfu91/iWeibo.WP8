using System;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TencentWeiboSDK.Model
{
    /// <summary>
    /// 标签 Model, 用于表示用户标签的对象.
    /// </summary>
    [DataContract]
    public class Tag : BaseModel
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        public Tag()
        { }

        /// <summary>
        /// 标签id
        /// </summary>
        [DataMember(Name="id")]
        public string Id { get; set; }


        
        /// <summary>
        /// 标签名
        /// </summary>
        [DataMember(Name="name")]
        public string Name { get; set; }

    }
}
