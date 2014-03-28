using System;
using System.Net;

namespace TencentWeiboSDK.Services.Util
{
    /// <summary>
    /// ContentType 枚举指示需要过滤的内容.
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 所有类型.
        /// </summary>
        All = 0,
        
        /// <summary>
        /// 带文本.
        /// </summary>
        Text  = 1,

        /// <summary>
        /// 带链接.
        /// </summary>
        Link = 2,

        /// <summary>
        /// 带图片.
        /// </summary>
        Pictrue = 4,

        /// <summary>
        /// 带视频.
        /// </summary>
        Video = 8,

        /// <summary>
        /// 带音频.
        /// </summary>
        Audio = 10
    }
}
